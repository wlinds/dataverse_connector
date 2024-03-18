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

- AppID: Find under Home > Your App > App Details
- Redirect URI: ???


---

# About Dataverse Tables

Tables will always come with default columns:

- createdby - Type: Lookup
- createdbyname - Type: String
- createdbyyominame - Type: String
- createdon - Type: DateTime
- createdonbehalfby - Type: Lookup
- createdonbehalfbyname - Type: String
- createdonbehalfbyyominame - Type: String
- importsequencenumber - Type: Integer
- modifiedby - Type: Lookup
- modifiedbyname - Type: String
- modifiedbyyominame - Type: String
- modifiedon - Type: DateTime
- modifiedonbehalfby - Type: Lookup
- modifiedonbehalfbyname - Type: String
- modifiedonbehalfbyyominame - Type: String
- overriddencreatedon - Type: DateTime
- ownerid - Type: Owner
- owneridname - Type: String
- owneridtype - Type: EntityName
- owneridyominame - Type: String
- owningbusinessunit - Type: Lookup
- owningbusinessunitname - Type: String
- owningteam - Type: Lookup
- owninguser - Type: Lookup
- statecode - Type: State
- statecodename - Type: Virtual
- statuscode - Type: Status
- statuscodename - Type: Virtual
- timezoneruleversionnumber - Type: Integer
- utcconversiontimezonecode - Type: Integer
- versionnumber - Type: BigInt