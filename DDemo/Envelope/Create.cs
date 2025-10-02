using DDemo.OAuth;
using System.Net.Http.Headers;
using System.Text;

namespace DDemo.Envelope
{
    internal static class Create
    {
        internal static async Task<Guid> New(string baseUri, string documentFileName, string accessToken, Guid apiAccountId, string signers)
        {
            return await New(baseUri, documentFileName, accessToken, apiAccountId, signers.ToDictionary());
        }

        internal static async Task<Guid> New(string baseUri, string documentFileName, string accessToken, Guid apiAccountId,
            Dictionary<string, string> recipients)
        {
            HttpClient httpClient = new() { BaseAddress = new Uri(baseUri) };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string method = $"restapi/v2.1/accounts/{apiAccountId}/envelopes/create";

            string emailSubject = $"Contract - {recipients.First().Key}";
            string emailBody = "<p>Dear Partner,</p><p>Please review and <strong>sign</strong> this contract issued by...</p>";

            PostObject postObject = new(emailSubject);

            Doc _document = new(documentFileName, $"Contract {recipients.First().Key}".Replace(" ", "_"));

            postObject.Documents.Add(_document);

            int index = 1;
            foreach (var recipient in recipients)
            {
                //postObject.Recipients.Signers.Add(new(recipient.Key, recipient.Value, index, emailSubject, emailBody,
                //    $"\\S{index}\\", 0, 0, 0)); //document contains anchor tags

                postObject.Recipients.Signers.Add(new(recipient.Key, recipient.Value, index, emailSubject, emailBody,
                    "", 40, 200 * index, 1)); //position signatures on x,y

                index++;
            }

            using HttpContent stringContent = new StringContent(postObject.Serialize(), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await httpClient.PostAsync(method, stringContent);
            string reply = await response.Content.ReadAsStringAsync();

            ReplyObject replyObject = reply.Deserialize<ReplyObject>() ?? new ReplyObject();
            if (string.IsNullOrWhiteSpace(replyObject.EnvelopeId) || !string.IsNullOrWhiteSpace(replyObject.ErrorCode))
            {
                Console.WriteLine(replyObject.ErrorCode + " " + replyObject.Message);
                return Guid.Empty;
            }

            return new Guid(replyObject.EnvelopeId);
        }

        private sealed class PostObject(string email_subject)
        {
            public string Status { get; init; } = "sent";
            public string EmailSubject { get; set; } = email_subject;
            public List<Doc> Documents { get; set; } = [];
            public Recipients Recipients { get; set; } = new();
        }

        private sealed class Recipients
        {
            public List<Signer> Signers { get; set; } = [];
        }

        private sealed class Signer(string name, string email, int index, string email_subject, string email_body,
            string anchorTag, int x = 0, int y = 0, int page = 0)
        {
            public string Name { get; set; } = name;
            public string Email { get; set; } = email;
            public string RecipientId { get; set; } = index.ToString();
            public string RoutingOrder { get; set; } = index.ToString();
            public RecipientEmailNotification EmailNotification { get; set; } = new(email_subject, email_body);
            public string InheritEmailNotificationConfiguration { get; init; } = "false";
            public Tabs Tabs { get; set; } = new(anchorTag, x, y, page);
        }

        private sealed class RecipientEmailNotification(string emailSubject, string emailBody)
        {
            public string EmailSubject { get; set; } = emailSubject;
            public string EmailBody { get; set; } = emailBody;
            public string SupportedLanguage { get; set; } = "el";
        }

        private sealed class Tabs
        {
            public Tabs(string anchorTag = "", int x = 0, int y = 0, int page = 0)
            {
                if (string.IsNullOrWhiteSpace(anchorTag) && page <= 0)
                    throw new ApplicationException("choose anchor tag or x-y");

                if (!string.IsNullOrWhiteSpace(anchorTag))
                    SignHereTabs = [new SignHereTabAnchor(anchorTag)];
                else
                {
                    SignHereTabs = [new SignHereTabXY(x, y, page)];
                }
            }
            public List<object> SignHereTabs { get; init; } = [];
        }

        private sealed class SignHereTabAnchor(string anchorTag)
        {
            public string AnchorString { get; init; } = anchorTag;
            public string AnchorXOffset { get; init; } = "8";
            public string AnchorYOffset { get; init; } = "5";
            public string AnchorIgnoreIfNotPresent { get; init; } = false.ToString();
            public string AnchorUnits { get; init; } = "mms";
        }

        private sealed class SignHereTabXY(int x, int y, int page, int documentId = 1)
        {
            public string XPosition { get; set; } = x.ToString();
            public string YPosition { get; set; } = y.ToString();
            public string DocumentId { get; set; } = documentId.ToString();
            public string PageNumber { get; set; } = page.ToString();
        }

        private sealed class Doc
        {
            public Doc(string documentFileName, string newFileName)
            {
                if (!File.Exists(documentFileName))
                    throw new ApplicationException($"{documentFileName} not found");

                DocumentBase64 = Convert.ToBase64String(File.ReadAllBytes(documentFileName));
                FileExtension = Path.GetExtension(documentFileName).TrimStart('.');
                Name = newFileName;
            }

            public string DocumentId { get; init; } = 1.ToString();
            public string Name { get; init; } = string.Empty;
            public string FileExtension { get; init; } = "pdf";
            public string DocumentBase64 { get; init; } = string.Empty;
        }

        private sealed class ReplyObject
        {
            public string EnvelopeId { get; init; } = string.Empty;
            public string Uri { get; init; } = string.Empty;
            public DateTime StatusDateTime { get; init; } = DateTime.MinValue;
            public string Status { get; init; } = string.Empty;
            public string ErrorCode { get; init; } = string.Empty;
            public string Message { get; init; } = string.Empty;
        }
    }
}
