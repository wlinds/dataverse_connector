using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

class Program
{
    static void Main()
    {
        try
        {
            // Read & parse JSON
            string json = File.ReadAllText("appsettings.json");
            JObject settings = JObject.Parse(json);
            JObject environmentVariables = (JObject)settings["EnvironmentVariables"];

            string url = (string)environmentVariables["Url"];
            string userName = (string)environmentVariables["UserName"];
            string password = (string)environmentVariables["Password"];
            string appId = (string)environmentVariables["AppId"];
            string redirectUri = (string)environmentVariables["RedirectUri"];

            // Connection string
            string connectionString = $@"
                AuthType = OAuth;
                Url = {url};
                UserName = {userName};
                Password = {password};
                AppId = {appId};
                RedirectUri = {redirectUri};
                LoginPrompt=Auto;
                RequireNewInstance=True";

            // Connect to Dataverse
            ServiceClient service = new ServiceClient(connectionString);

            // Always use logical names for tables and columns
            var entityMetadata = RetrieveEntityMetadata(service, "cr226_table1");

            if (entityMetadata != null)
            {
                Console.WriteLine("Columns in cr226_table1:");

                foreach (var attributeMetadata in entityMetadata.Attributes)
                {
                    Console.WriteLine($"{attributeMetadata.LogicalName} - Type: {attributeMetadata.AttributeType}");
                }

                // Count rows
                int rowCount = CountRows(service, "cr226_table1");
                Console.WriteLine($"Number of rows in cr226_table1: {rowCount}");
            }
            else
            {
                Console.WriteLine("Table metadata not found.");
            }

            Console.WriteLine("Press <Enter> key to exit.");
            Console.ReadLine();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Error: appsettings.json file not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static EntityMetadata RetrieveEntityMetadata(ServiceClient service, string entityName)
    {
        // Retrieve entity metadata
        var metadataRequest = new Microsoft.Xrm.Sdk.Messages.RetrieveEntityRequest()
        {
            EntityFilters = Microsoft.Xrm.Sdk.Metadata.EntityFilters.Entity | Microsoft.Xrm.Sdk.Metadata.EntityFilters.Attributes,
            LogicalName = entityName
        };

        var metadataResponse = (Microsoft.Xrm.Sdk.Messages.RetrieveEntityResponse)service.Execute(metadataRequest);

        return metadataResponse.EntityMetadata;
    }

    static int CountRows(IOrganizationService service, string entityName)
{
    // Execute FetchXML to count rows
    string fetchXml = $@"
        <fetch aggregate='true'>
            <entity name='{entityName}'>
                <attribute name='{entityName}id' aggregate='count' alias='RowCount' />
            </entity>
        </fetch>";

    EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));

    if (result.Entities.Count > 0 && result.Entities[0].Contains("RowCount"))
    {
        int rowCount;
        if (result.Entities[0].TryGetAttributeValue<int>("RowCount", out rowCount))
        {
            return rowCount;
        }
    }

    return 0;
}
}
