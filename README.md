# Peakup.Integrations.Google | C# SDK
Integration helper for google api's.

## Installation

#### Package Manager Console
```
Install-Package PeakUp.Integrations.Google -Version 0.1.5 
```

#### .NET CLI
```
dotnet add package PeakUp.Integrations.Google --version 0.1.5 
```

#### Paket CLI
```
paket add PeakUp.Integrations.Google --version 0.1.5
```


## Initializing GoogleService

```
private GoogleService googleService = new GoogleService(new Credentials()
        {
            ApiKey = "YOUR_GOOGLE_API_KEY",
            ClientId = "YOUR_GOOGLE_CLIENT_ID",
            ClientSecret = "YOUR_GOOGLE_CLIENT_SECRET",
            RedirectUrl = "YOUR_REDIRECT_URL"
        });
```

**All fields are required!**

## Authentication

#### Declaring scopes

```
 var scopes = new Dictionary<Scope, ScopeType>()
            {
                {Scope.GooglePlus, ScopeType.Login },
                { Scope.Youtube, ScopeType.Readonly},
                { Scope.YoutubeAnalytics, ScopeType.Readonly},
                { Scope.Emails, ScopeType.NoScopeType },
                { Scope.Me, ScopeType.NoScopeType },
                { Scope.AgeRange, ScopeType.NoScopeType }
            };
```

#### Getting Outh2 Url

```
var outh2Url = googleService.GetOuth2Url(new Outh2RequestBody()
            {
                AccessType = AccessType.Offline,
                IncludeGrantedScopes = true,
                ResponseType = ResponseType.Code,
                State = "ANY_STRING_VALUE",
                Scopes = scopes

            })

```
Redirect your users to the 'outh2url' and after the login process, you can retreive the 'code' from the redirect url you just declared at the 'googleService' credentials.
With that code, you can ask for the token;

```

var authResponse = await googleService.Auth(code);

```

##### Auth Response Parameters

| Parameter  | Description |
| ------------- | ------------- |
| Value  | Access token  |
| Type  | Access token type (e.g Bearer)  |
| ExpiresIn  | The duration of the expiration of the validity of your token. (seconds)  |
| RefreshToken  | Refresh token  |


### Or You Have a Token Already

```
//myToken type is 'Token' class
googleService.SetToken(myToken);

```

### Getting Current Token

```
var currentToken = googleService.GetToken();

```




