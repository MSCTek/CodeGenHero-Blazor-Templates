# CodeGenHero Blazor Enterprise Architecture

> Congratulations, you have successfully created a new solution designed for use with CodeGenHero!

## Next Steps

1.	Open the appsettings.json file in the IDP project
- The value of the 'UserDbContextConnection' connection string setting should match the value you provided to the project creation wizard.

> ### @icon-info-circle Bad practice
> Note that it is bad practice to store the database connection string in a file that may end up checked into source control. Right click on the IDP project in Visual Studio's Solution Explorer and select **Manage User Secrets** from the context menu. For more info, see: [https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)

- Transfer (Cut / Paste) the connection string value from appsettings.json into the secrets.json file. Note that the format of the key gets collapsed in the secrets.json file to "ConnectionStrings:UserDbContextConnection": "\<your connection string\>" instead of the way it is nested in the appsettings.json file.

- Repeat the above process for the Api project to transfer the "ConnectionStrings:DefaultConnection" value from appsettings.jon to the secrets.json file for the Api project.

2.	Right-click on the solution in Solution Explorer and select Properties.
- The multiple startup projects option should be selected and the action should be set to start for the Api, Client, and IDP projects.

3.	Try running / debugging the solution
- Did all three projects start up (IdentityServer4, Your Blazor App (Blazor Wasm), and the Api project?
- Look at the console window for the running Api project. What port is it listening on - 5301 (note that it may also be listening on 5302 for gRPC calls)?
- Navigate to the port the API project is running on [https://localhost:5301/](https://localhost:5301/) and you should see the  assembly version displayed as "Version 1.0.0".
- Navigate to the Swagger page [https://localhost:5301/swagger/index.html](https://localhost:5301/swagger/index.html)
- Click the **GET** button for the "/api/TestAuth/TestAdmin" function to expand it, then click the **Try it out** button, then click the **Execute** button. You should see a "Error: response status is 401" message indicating you are not authorized.
- Click the **Authorize** button at the top right of Swagger's index page. Notice that it wants a JWT Bearer token. Next, we will use Postman to get that Bearer token.

4. Use Postman to get a Bearer token from the running IdentityServer4 project.
- TBD

