# Temp-Mail-API
Unofficial API for [TempMail](https://temp-mail.org) in .NET

It can be used to generate temporary emails, it can help in making bots or anything else.

# Usage
```csharp

var client = new Client();

// To get the available domains
var availableDomains = client.AvailableDomains;

// To get Mailbox
var mails = client.Inbox.Refresh();

var ms = client.Inbox.ExtractSimpleMails();

// To change email to a specific login@domain
client.Change("loginexample", availableDomains[0]);

// To delete email and get a new one
client.Delete();

// To get the current email
var email = client.Email;

```

Full Test Example [Here](https://github.com/RyuzakiH/Temp-Mail-API/blob/master/src/TempMail.Example/Program.cs)

# Supported Platforms
[.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions.md)

# Dependencies
* [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack)
* [MimeKit](https://www.nuget.org/packages/MimeKit)
* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)
