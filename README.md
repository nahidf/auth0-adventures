# auth0-adventures

This solution is to demonstrate Authentication using OpenID Connect and Authorization using OAuth 2.0 against Auth0. 
It contains calls to Auth0 API management to retrieve some data.
This solution is using **.NET 5**.

The exercise structure is: 

```
src
│   Blazor-Auth0.sln
├── Client : ASP.NET Core Blazor WASM app
├── API : ASP.NET Core API
└── Models : Shared library 
```

**Client** is a ASP.NET Core Blazor WASM app, which contains following features:

- Login using OpenId Connect against Auth0 

**API** is a ASP.NET Core API, which contains following features:

- Endpoint authorization using OAuth 2.0
- Remotely queries the Auth0 Management API 

## Run the solution locally

To run the solution using your own settings on Auth0 and also local development env, you need to setup the `Auth0 Tenant`, `Client` and `API`.

### Auth0 Tenant - Auth0 dashboard

To setup this application, you need to create a `tenant` on Auth0 dashboard. The `Domain`(unique identifier) of this tenant is what we will reference later in this document as `Authority` 
Also to try out the login, create some users via `users` link on the Auth0 dashboard. You login using any of users using email/password. List of users will be shown on client app.

### Client - Auth0 dashboard:

To setup the client on auth0 dashboard please follow these steps: 

- Go to `Applications` link 
- Create a `SINGLE PAGE APPLICATION`
- Decide about a URL for client app. This is the URL you will use to run the client app locally. It can be a URL like `https://localhost:6006`
- Set `Allowed Callback URLs`

```
{client-url}/authentication/login-callback
```

- Set `Allowed Logout URLs` 

```
{client-url}/authentication/logout-callback
```

<img width="400" alt="Screen Shot 2020-08-18 at 2 40 44 PM" src="https://user-images.githubusercontent.com/4095071/90553770-ea2bb900-e162-11ea-87e4-e58eab1f62ff.png">

### Client - Code: 

To setup the ASP.NET Core Blazor WASM app: 

1. **URL**: Open `launchSettings.json` on properties folder of client(`src\Client\Properties\launchSettings.json`) and adjust the application urls to match what you set on [Client - Auth0 dashboard](#client---auth0-dashboard) for client url.

2. **Configuration**: Open `appsettings.json` on wwwroot folder of client(`src\Client\wwwroot\appsettings.json`) and adjust config values. There is two sets of values to adjust: 

**`Auth0`**: These values are required to authenticate via Auth0 using OpenId Connect, and to receive access_token to call the custom API.

- `Authority`: Set this with tenant's domain value. This value defined on [Auth0 Tenant - Auth0 dashboard](#auth0-tenant---auth0-dashboard).
- `Audience`: Set this to the API key. This value is defined on [API - Auth0 dashboard](#api---auth0-dashboard).
- `ClientId`: Set this to the ClientId for the SPA app. This value is defined on [Client - Auth0 dashboard](#client---auth0-dashboard).

Here is example values set: 

```
{
  "Auth0": {
    "Authority": "https://dev-test.us.auth0.com",
    "Audience": "https://localhost:7006",
    "ClientId": "{client-id}"
  }
}
```

**`Api`**: These are values required to call the Custom API.

- BaseAddress: Set this to the API base address. This value is defined on [API - Code](#api---code).

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
- Decide about a URL for API. This is the URL you will use to run the API locally. It can be a URL like `https://localhost:7006`
- Set `Identifier` to the URL decided. This value is what we use as `audience` on client application.  

<img width="400" alt="Screen Shot 2020-08-18 at 2 58 32 PM" src="https://user-images.githubusercontent.com/4095071/91125583-1322e100-e670-11ea-861c-6da2561d2ac2.png">

- Go to `Machine to Machine Applications` and change `API Explorer Application` to be `Authorized` 

<img width="400" alt="Screen Shot 2020-08-18 at 3 00 40 PM" src="https://user-images.githubusercontent.com/4095071/90554326-b00ee700-e163-11ea-8e9d-0ab6c57e785c.png">

- Go to `Applications` link 
- Select the api application you created in last steps
- Copy Client ID and secret. These values will be used later on API code 

<img width="400" alt="Screen Shot 2020-08-19 at 10 42 30 AM" src="https://user-images.githubusercontent.com/4095071/91125723-6301a800-e670-11ea-8faa-d3cf61ff9e8b.png">


### API - Code: 

To setup the ASP.NET API: 

1. **URL**: Open `launchSettings.json` on properties folder of api(`src\API\Properties\launchSettings.json`) and adjust the application urls to match what you set on [API - Auth0 dashboard](#api---auth0-dashboard) for API's identifier(URL).

2. **Configuration**: Open `appsettings.json` on root folder of api(`src\api\appsettings.json`) and adjust config values. There is two sets of values to adjust: 

**`Auth0`**: These values are required to authorize via Auth0 using OAuth2, and to receive access_token to call the API Management.

- `Authority`: Set this with tenant's domain value. This value defined on [Auth0 Tenant - Auth0 dashboard](#auth0-tenant---auth0-dashboard).
- `Audience`: Set this to the API identifier(URL). This value is defined on [API - Auth0 dashboard](#api---auth0-dashboard).
- `ClientId`: Set this to the ClientId for the API app. This value is captured on [API - Auth0 dashboard](#api---auth0-dashboard).
- `ClientSecret`: Set this to the ClientSecret for the API app. This value is captured on [API - Auth0 dashboard](#api---auth0-dashboard).

```
"Auth0": {
    "Authority": "https://dev-test.us.auth0.com",
    "Audience": "https://localhost:7006",
    "ClientId": "{client-id}",
    "ClientSecret": "{client-secret}"
  },
```

**`Client`**: Values required for client call to the API.

`Origin`: Set this to the Client app URL. This value defined on [Client - Code](#client---code).

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

