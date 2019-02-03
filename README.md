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

# Dependencies
* [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack)
* [MimeKit](https://www.nuget.org/packages/MimeKit)
