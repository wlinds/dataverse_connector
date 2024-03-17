# dataverse_connector

Microsoft Dataverse is a cloud storage platform which can be a bit tedious to integrate into none-Power Platform applications. This repo serves as a boilerplate C# codebase for CRUD operations on Dataverse tables.

## Notes

General about Power Platform environment:

- Always refer to a table or a column by its logical name.

Datatypes:

- System.Decimal columns is NOT System.Double (or Float)
- Datetime columns are NOT Datetime objects

## Intallation (Linux)

1. Download and install [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)

2. Run the command `dotnet add package Microsoft.PowerPlatform.Dataverse.Client` to install the [NuGet package](https://www.nuget.org/packages/Microsoft.PowerPlatform.Dataverse.Client).

3. Run the command `dotnet add package Microsoft.Extensions.Configuration`

4. Actually run the command `dotnet add package Microsoft.Extensions.Configuration.Json` instead.

5. Actually no, just use `Newtonsoft.Json.Linq`

6. Ok uh, I'll have to rewrite this

7. Make sure you have an `appsettings.json` as such:

```
{
    "EnvironmentVariables": {
      "Url": "",
      "UserName": "",
      "Password": "",
      "AppId": "",
      "RedirectUri": ""
    }
}
```

Find the values within the jungle of Microsoft/Azure/Entra/PowerApps etc. Depending on your application you can find these values in different places. Ask your administrator.

