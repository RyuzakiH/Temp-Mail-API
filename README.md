Temp-Mail-API
[![AppVeyor](https://img.shields.io/appveyor/ci/RyuzakiH/Temp-Mail-API/master.svg?maxAge=60)](https://ci.appveyor.com/project/RyuzakiH/Temp-Mail-API)
[![NuGet](https://img.shields.io/nuget/v/TempMail.API.svg?maxAge=60)](https://www.nuget.org/packages/TempMail.API)
===============

Unofficial API Client Library for [TempMail](https://temp-mail.org) in .NET Standard

> Disposable email - is a service that allows to receive email at a temporary address that self-destructed after a certain time elapses. It is also known by names like : tempmail, 10minutemail, throwaway email, fake-mail or trash-mail. Many forums, Wi-Fi owners, websites and blogs ask visitors to register before they can view content, post comments or download something. Temp-Mail - is most advanced throwaway email service that helps you avoid spam and stay safe.

# Installation

`PM> Install-Package TempMail.API`

# Usage

- #### Initialize Client
_Creates and initializes a new temp-mail client with a temporary email_

```csharp
var client = TempMailClient.Create();
```

```csharp
var client = await TempMailClient.CreateAsync();
```

- #### Current Email
_Gets current temporary email_
```csharp
var email = client.Email;
```

- #### Available Domains
_Gets available domains_

```csharp
var availableDomains = client.AvailableDomains;
```

- #### Change Email
_Changes the temporary email to a specific email (ex: login@domain)_

```csharp
client.ChangeEmail("loginexample", availableDomains[0]);
```
```csharp
await client.ChangeEmailAsync("loginexample", availableDomains[0]);
```

- #### Delete
_Deletes the temporary email and gets a new one_

```csharp
client.Delete();
```
```csharp
await client.DeleteAsync();
```

- ### Refresh
_Gets all mails in mailbox_

```csharp
var mails = client.Inbox.Refresh();
```
```csharp
var mails = await client.Inbox.RefreshAsync();
```

- #### Mailbox
_Gets all mails in mailbox (doesn't update from temp-mail)_

```csharp
var mails = client.Inbox.Mails;
```

- #### Inbox Auto Check
_Checks for incoming mails every period of time_

```csharp
client.StartAutoCheck(delay: 10000); // default delay is 10s
```

```csharp
client.StopAutoCheck();
```

- #### Events

_Occurs when the temporary email changes_

```csharp
client.EmailChanged += (o, e) => Console.WriteLine($"Email changed: {e.Email}");
```

_Occurs when a new mail is received by client_

```csharp
client.Inbox.NewMailReceived += (o, e) => Console.WriteLine($"\tSender: {e.Mail.SenderName}\n\tSubject: {e.Mail.Subject}\n\tBody: {e.Mail.TextBody}");
```

# Additional Features

- #### Cloudflare Protection
Website sometimes uses Cloudflare protection, so we use [CloudflareSolverRe](https://www.nuget.org/packages/CloudflareSolverRe) to bypass it.

Sometimes Cloudflare forces captcha challenges or Js challenge cannot be solved, so we need to use [CloudflareSolverRe.Captcha](https://www.nuget.org/packages/CloudflareSolverRe.Captcha) package to bypass it using (2captcha or AntiCaptcha)

```csharp
var client = TempMailClient.Create(
    captchaProvider: new AntiCaptchaProvider("YOUR_API_KEY"));
```

```csharp
var client = TempMailClient.Create(
    captchaProvider: new TwoCaptchaProvider("YOUR_API_KEY"));
```

_for more information read the documentation here [CloudflareSolverRe](https://github.com/RyuzakiH/CloudflareSolverRe)_

- #### Proxy

```csharp
var client = TempMailClient.Create(
    proxy: new WebProxy("163.172.220.221", 8888));
```

**Full Examples [Here](https://github.com/RyuzakiH/Temp-Mail-API/tree/master/sample/TempMail.Sample)**

# Supported Platforms
[.NET Standard 1.3](https://github.com/dotnet/standard/blob/master/docs/versions.md)

# Dependencies
* [CloudflareSolverRe](https://www.nuget.org/packages/CloudflareSolverRe)
* [MimeKitLite](https://www.nuget.org/packages/MimeKitLite)
