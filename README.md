# auth0-adventures

The exercise structure is: 

```
src
│   Exercise.sln
├── Client : ASP.NET Core Blazor WASM app
├── API : ASP.NET Core API
└── Models : Shared library 
```


### Client 

Client is a ASP.NET Core Blazor WASM app, which contains following features 

- Prompts the user to log-in to Auth0
- Calls the ASP.NET Core web application to get the list of users

##### Set up: 

To setup the ASP.NET Core Blazor WASM app, open `appsettings.json` on wwwroot folder of client(`src\Client\wwwroot\appsettings.json`) and adjust config values. There is two sets of values to adjust: 

1. `Auth0`: These values are required to authenticate via Auth0 using OpenId Connect, and to recieve access_token to call the custom API.

- Authority: Set this with tenant domain value. 
- Audience: Set this to the API key.
- ClientId: Set this to the ClientId for the SPA app 

```
{
  "Auth0": {
    "Authority": "https://dev-fh3a8ca8.us.auth0.com",
    "Audience": "https://localhost:7006",
    "ClientId": "ldlHCnD8HmQSqip79Kn92QFNXT77Ixzl"
  }
}
```

2. `Api`: These are values required to call the Custom API.

- BaseAddress: Set this to the API base address.

```
{
  "Api": {
    "BaseAddress": "https://localhost:7006"
  }
}
```

### API 

API is a ASP.NET Core API, which contains following features 

- Ensures the request being made is authorized
- Takes a parameter specifying a search term
- Remotely queries the Auth0 Management API to search users for this term
- Returns that a list of users for the tenant as a JSON response

##### Set up: 

To setup the ASP.NET API, open `appsettings.json` on root folder of api(`src\api\appsettings.json`) and adjust config values. There is two sets of values to adjust: 

1. `Auth0`: These values are required to authorize via Auth0 using OAuth2, and to recieve access_token to call the API Management.

```
"Auth0": {
    "Authority": "https://dev-fh3a8ca8.us.auth0.com",
    "Audience": "https://localhost:7006",
    "ClientId": "N3Inh5trdEImWTKnPVeks4Fl0dvPl5QE",
    "ClientSecret": "35zmrp5nqmX-lH0mWYQ3iGOJMT8nSSqTEmHkqPEyQLFDKwX2b8tMEsWchk4dgwsL"
  },
```

2. `Client`: Values required for client call to the API.

```
 "Client": {
    "Origin": "https://localhost:6006"
  }
 ```

