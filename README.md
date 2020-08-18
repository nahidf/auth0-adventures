# auth0-adventures

The exercise structure is: 

```
src
│   Exercise.sln
├── Client : ASP.NET Core Blazor WASM app
├── API : ASP.NET Core API
└── Models : Shared library 
```

### Auth0 Tenant 

To run this application, create a `tenant` on Auth0 dashboard. Also need to create some users via `users` link on the Auth0 dashboard. We can login using any of added users and also view list of users on the client app.

### Client 

Client is a ASP.NET Core Blazor WASM app, which contains following features 

- Prompts the user to log-in to Auth0
- Calls the ASP.NET Core web application to get the list of users

##### Set up on Auth0 dashboard:

To setup the client on auth0 dashboard please follow these steps: 

- Go to `Applications` link 
- Create a `SINGLE PAGE APPLICATION`
- Set `Allowed Callback URLs` 

```
{client-url}/authentication/login-callback
```

- Set `Allowed Logout URLs` 

```
{client-url}/authentication/logout-callback
```

<img width="400" alt="Screen Shot 2020-08-18 at 2 40 44 PM" src="https://user-images.githubusercontent.com/4095071/90553770-ea2bb900-e162-11ea-87e4-e58eab1f62ff.png">

##### Set up code: 

To setup the ASP.NET Core Blazor WASM app, open `appsettings.json` on wwwroot folder of client(`src\Client\wwwroot\appsettings.json`) and adjust config values. There is two sets of values to adjust: 

**`Auth0`**: These values are required to authenticate via Auth0 using OpenId Connect, and to recieve access_token to call the custom API.

- `Authority`: Set this with tenant domain value. 
- `Audience`: Set this to the API key.
- `ClientId`: Set this to the ClientId for the SPA app 

```
{
  "Auth0": {
    "Authority": "https://dev-fh3a8ca8.us.auth0.com",
    "Audience": "https://localhost:7006",
    "ClientId": "ldlHCnD8HmQSqip79Kn92QFNXT77Ixzl"
  }
}
```

**`Api`**: These are values required to call the Custom API.

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

##### Set up on Auth0 dashboard:

To setup the API on auth0 dashboard please follow these steps: 

- Go to `APIs` link 
- Create an `API`
- Set `Identifier`, this value is what we use as `audience` on client application  

<img width="400" alt="Screen Shot 2020-08-18 at 2 58 32 PM" src="https://user-images.githubusercontent.com/4095071/90554102-64f4d400-e163-11ea-8405-7e9d4c9728b6.png">

- Go to `Machine to Machine Applications` and change `API Explorer Application` to be `Authorized` 

<img width="400" alt="Screen Shot 2020-08-18 at 3 00 40 PM" src="https://user-images.githubusercontent.com/4095071/90554326-b00ee700-e163-11ea-8e9d-0ab6c57e785c.png">

##### Set up code: 

To setup the ASP.NET API, open `appsettings.json` on root folder of api(`src\api\appsettings.json`) and adjust config values. There is two sets of values to adjust: 

**`Auth0`**: These values are required to authorize via Auth0 using OAuth2, and to recieve access_token to call the API Management.

```
"Auth0": {
    "Authority": "https://dev-fh3a8ca8.us.auth0.com",
    "Audience": "https://localhost:7006",
    "ClientId": "N3Inh5trdEImWTKnPVeks4Fl0dvPl5QE",
    "ClientSecret": "35zmrp5nqmX-lH0mWYQ3iGOJMT8nSSqTEmHkqPEyQLFDKwX2b8tMEsWchk4dgwsL"
  },
```

**`Client`**: Values required for client call to the API.

```
 "Client": {
    "Origin": "https://localhost:6006"
  }
 ```


### Local Https with ASP.NET Core

To develop locally with ASP.NET Core under HTTPS, SSL, and Self-Signed Certs, follow these steps: 

1. Install the certificate on the machine, by running in cmd:

```
dotnet dev-certs https --trust
```

2. Copy `Certificate thumbprint`: 

![image](https://user-images.githubusercontent.com/4095071/90553572-9c16b580-e162-11ea-8d5f-2fa74dd59652.png)

![image](https://user-images.githubusercontent.com/4095071/90553451-67a2f980-e162-11ea-8cd1-0d5dfb50c0cf.png)


3. Then run in cmd for each URL:

```
"C:\Program Files (x86)\IIS Express\IisExpressAdminCmd.exe" setupSslUrl -url:https://my.domain.name:<port> -CertHash:<Certificate thumbprint>

```

