# Temp-Mail-API
Unofficial API for [TempMail](https://temp-mail.org) in .NET

It can be used to generate temporary emails, it can help in making bots or anything else.

# Usage
```csharp

var session = new Session();

// To get the available domains
var availableDomains = session.AvailableDomains;

// To get Mailbox
var mails = session.Inbox.Refresh();

// To change email to a specific login@domain
session.Change("loginexample", availableDomains[0]);

// To delete email and get a new one
session.Delete();

// To get the current email
session.Email;

```

# Dependencies
* [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack)
* [MimeKit](https://www.nuget.org/packages/MimeKit)
