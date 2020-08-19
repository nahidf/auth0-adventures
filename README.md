# auth0-adventures

This solution is an exercise to demonstrate Authentication using OpenId Connect and Authorization using OAuth 2.0 against Auth0. It contains calls to Auth0 API management to retrieve some data.

The exercise structure is: 

```
src
│   Exercise.sln
├── Client : ASP.NET Core Blazor WASM app
├── API : ASP.NET Core API
└── Models : Shared library 
```

**Client** is a ASP.NET Core Blazor WASM app, which contains following features 

- Prompts the user to log-in to Auth0
- Calls the ASP.NET Core web application to get the list of users

**API** is a ASP.NET Core API, which contains following features 

- Ensures the request being made is authorized
- Takes a parameter specifying a search term
- Remotely queries the Auth0 Management API to search users for this term
- Returns that a list of users for the tenant as a JSON response

## Run the solution locally using default settings 

**Local Https**: To run the project locally using the default configuration in the repo, you need setup to use local Https with ASP.NET Core.  

1. Install ASP.NET Core self signed certificate for development on your machine 

```
dotnet dev-certs https --trust
```
2. Copy the generated certificate's thumbprint

3. Setup SSL URL for `Client` and `API` project by running following two commands. 

```
"C:\Program Files (x86)\IIS Express\IisExpressAdminCmd.exe" setupSslUrl -url:https://localhost:6006 -CertHash:<Certificate thumbprint>
``` 
```
"C:\Program Files (x86)\IIS Express\IisExpressAdminCmd.exe" setupSslUrl -url:https://localhost:7006 -CertHash:<Certificate thumbprint>
```

If you have any trouble on this please checkout [Local Https with ASP.NET Core](#local-https-with-aspnet-core) section on this page.

**User Login**: After running the project you can login using `nahid@test.com`/`Test@123` or any other user listed in the client using their email/`Test@123`.

## Run the solution locally using your own settings 

To run the solution using your own settings on Auth0 and also local development env, you need to setup the `Auth0 Tenant`, `Client` and `API`.

### Auth0 Tenant - Auth0 dashboard

To setup this application, you need to create a `tenant` on Auth0 dashboard. The `Domain`(unique identifier) of this tenant is what we will refrence later in this document as `Authority` 
Also to try out the login, create some users via `users` link on the Auth0 dashboard. You login using any of users using email/password. List of users will be shown on client app.

### Client - Auth0 dashboard:

To setup the client on auth0 dashboard please follow these steps: 

- Go to `Applications` link 
- Create a `SINGLE PAGE APPLICATION`
- Decide about a URL for client app. This is the URL you will use to run the client app localy. It can be a URL like `https://localhost:6006`
- Set `Allowed Callback URLs`

```
{client-url}/authentication/login-callback
```

- Set `Allowed Logout URLs` 

```
{client-url}/authentication/logout-callback
```

<img width="400" alt="Screen Shot 2020-08-18 at 2 40 44 PM" src="https://user-images.githubusercontent.com/4095071/90553770-ea2bb900-e162-11ea-87e4-e58eab1f62ff.png">

##### Client - Code: 

To setup the ASP.NET Core Blazor WASM app, open `appsettings.json` on wwwroot folder of client(`src\Client\wwwroot\appsettings.json`) and adjust config values. There is two sets of values to adjust: 

**`Auth0`**: These values are required to authenticate via Auth0 using OpenId Connect, and to recieve access_token to call the custom API.

- `Authority`: Set this with tenant's domain value. This value defined on [Auth0 Tenant - Auth0 dashboard](#auth0-tenant---auth0-dashboard).
- `Audience`: Set this to the API key. This value is defined on [API - Auth0 dashboard](#api---auth0-dashboard).
- `ClientId`: Set this to the ClientId for the SPA app. This value is defined on [Client - Auth0 dashboard](#client---auth0-dashboard).

Here is example values set: 

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

- BaseAddress: Set this to the API base address. This value is defined on [API - Code]((#api---code)).

```
{
  "Api": {
    "BaseAddress": "https://localhost:7006"
  }
}
```

### API - Auth0 dashboard:

To setup the API on auth0 dashboard please follow these steps: 

- Go to `APIs` link 
- Create an `API`
- Set `Identifier`, this value is what we use as `audience` on client application.  

<img width="400" alt="Screen Shot 2020-08-18 at 2 58 32 PM" src="https://user-images.githubusercontent.com/4095071/90554102-64f4d400-e163-11ea-8405-7e9d4c9728b6.png">

- Go to `Machine to Machine Applications` and change `API Explorer Application` to be `Authorized` 

<img width="400" alt="Screen Shot 2020-08-18 at 3 00 40 PM" src="https://user-images.githubusercontent.com/4095071/90554326-b00ee700-e163-11ea-8e9d-0ab6c57e785c.png">

##### Code: 

To setup the ASP.NET API, open `appsettings.json` on root folder of api(`src\api\appsettings.json`) and adjust config values. There is two sets of values to adjust: 

**`Auth0`**: These values are required to authorize via Auth0 using OAuth2, and to recieve access_token to call the API Management.

- `Authority`: Set this with tenant's domain value. This value defined on [Auth0 Tenant - Auth0 dashboard](#auth0-tenant---auth0-dashboard).
- `Audience`: Set this to the API key. This value is defined on [API - Auth0 dashboard]((#api---auth0-dashboard)).
- `ClientId`: Set this to the ClientId for the API app. This value is defined on [API - Auth0 dashboard]((#api---auth0-dashboard)).
- `ClientId`: Set this to the ClientSecret for the API app. This value is defined on [API - Auth0 dashboard]((#api---auth0-dashboard)).

```
"Auth0": {
    "Authority": "https://dev-fh3a8ca8.us.auth0.com",
    "Audience": "https://localhost:7006",
    "ClientId": "N3Inh5trdEImWTKnPVeks4Fl0dvPl5QE",
    "ClientSecret": "35zmrp5nqmX-lH0mWYQ3iGOJMT8nSSqTEmHkqPEyQLFDKwX2b8tMEsWchk4dgwsL"
  },
```

**`Client`**: Values required for client call to the API.

`Origin`: Set this to the Client app URL. This value defined on [Client - Code]((#client---code)).

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

