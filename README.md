Temp-Mail-API
[![AppVeyor](https://img.shields.io/appveyor/ci/RyuzakiH/Temp-Mail-API/master.svg?maxAge=60)](https://ci.appveyor.com/project/RyuzakiH/Temp-Mail-API)
[![NuGet](https://img.shields.io/nuget/v/TempMail.API.svg?maxAge=60)](https://www.nuget.org/packages/TempMail.API)
===============

Unofficial API for [TempMail](https://temp-mail.org) in .NET

It can be used to generate temporary emails, it can help in making bots or anything else.

NuGet: https://www.nuget.org/packages/TempMail.API


# Usage

Synchronous Example

```csharp

var client = new Client();

// To get a new temporary email
client.StartNewSession();

// To get the available domains
var availableDomains = client.AvailableDomains;

// To get Mailbox
var mails = client.Inbox.Refresh();

// To change email to a specific login@domain
client.Change("loginexample", availableDomains[0]);

// To delete email and get a new one
client.Delete();

// To get the current email
var email = client.Email;

```

Asynchronous Example

```csharp

var client = new Client();

// To get a new temporary email
await client.StartNewSessionAsync();

// To get the available domains (not async)
var availableDomains = await Task.Run(() => client.AvailableDomains);

// To get Mailbox
var mails = await client.Inbox.RefreshAsync();

// To change email to a specific login@domain
await client.ChangeAsync("loginexample", availableDomains[0]);

// To delete email and get a new one
await client.DeleteAsync();

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
