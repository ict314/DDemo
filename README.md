# C# DocuSign® eSignature API Example

## Introduction
This repo is a C# .NET Core 9 console application that demonstrates:

* Authentication with Docusign via [JSON Web Token (JWT) Grant](https://developers.docusign.com/platform/auth/jwt/).

* Generating and Sending a new Envelope

## Installation

### Prerequisites
1. A free [Docusign developer account](https://www.docusign.com/developers/sandbox); create one if you don't already have one.
2. A Docusign app and integration key that is configured to use [JWT Grant](https://developers.docusign.com/platform/auth/jwt/) authentication.
3. Your User ID. On the [Apps and Keys](https://admindemo.docusign.com/authenticate?goTo=apiIntegratorKey) page, under **My Account Information**, copy the **User ID** GUID
4. RSA private key. Under **Authentication**, select **+ GENERATE RSA**. Copy the private key, and save it in a new file e.g. pkey.pem
5. [Microsoft .NET](https://dotnet.microsoft.com/en-us/download) 9.0
6. [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)

### Installation steps

1. Download this repository.
2. In File Explorer, open your Quickstart folder or your code-examples-csharp folder.
3. Open the project with Visual Studio by double-clicking the DDemo.sln file.
4. Choose signatures placement method at DDemo.Envelope.Create.New 
```csharp
foreach (var recipient in recipients) {
//choose x,y coordinates
postObject.Recipients.Signers.Add(new(recipient.Key, recipient.Value, index, emailSubject, emailBody,"", 40, 200 * index, 1));

//or anchor tags
postObject.Recipients.Signers.Add(new(recipient.Key, recipient.Value, index, emailSubject, emailBody, $"\\S{index}\\"));
```

### Execution
Build the project and locate the DDemo.dll
```sh
dotnet DDemo.dll --help
```
```cmd
DDemo 1.0.0
Copyright (C) 2025 DDemo

  -i, --IntegrationKey    Required. Integration Key, GUID value that identifies your integration

  -u, --UserId            Required. User Id, GUID of the user to impersonate

  -a, --ApiAccountId      Required. Api Account Id, GUID of application account

  -p, --RSAPrivateKey     Required. RSA Private key file path

  -s, --Signers           Required. in the format '1st signer Name:Email,2nd Name:Email'

  -d, --Document          Required. Document to sign

  -e, --Environment       Development (default) or Production

  --help                  Display this help screen.

  --version               Display version information.
```

```sh
#sample
dotnet DDemo.dll -i 7******a-5**a-4**1-9**c-c**********0 -u 0******1-a**b-4**0-b**d-e**********b -a c******6-d**4-4**c-a**9-e**********8 -p ~/pkey.pem -s "John Doe:jdoe@doe.com,Bob Martin:b.martin@nobo.com" -d ~/contract.pdf
```

## License and additional information

### License
This repository uses the MIT License. See [LICENSE](./LICENSE) for details.

### Pull Requests
Pull requests are welcomed. Pull requests will only be considered if their content
uses the MIT License.
