
# Development approach for .NET Exercise 

- [X] Create GitHub repo for project
- [X] Signup on Auth0 
- [X] Setup the SPA app on Auth0 dashboard
- [X] Setup the API app on Auth0 dashboard
- [X] Add some users on Auth0 dashboard
- [X] Setup the solution on VisualStudio consist on these project:
  - [X] SPA app - ASP.NET Core Blazor WebAssembly: create the app with authentication 
  - [X] API - ASP.NET Core API 
  - [X] Models Library - .NET Standard Class Library : this is to share `User` model between API and the Blazor app 
- [X] Setup SPA app authentication to login via Auth0
  - [X] Setup logout 
  - [X] Add users page - this page is just available to authenticated users 
- [X] Change API to add Authentication via Auth0
  - [X] Add users endpoint to API: return mock data 
  - [X] Change users endpoint to be protected(Authorized) 
- [X] Change SPA app to call users API using access_token 
- [X] Change API code to match requirements 
  - [X] Call Auth0 token endpoint to get client auth token 
  - [X] Call Auth0 users API to retrieve users data using client auth token   
  - [X] Apply search filtering on API 
