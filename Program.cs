using System;
using System.Collections.Generic;
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
            JObject settings = JObject.Parse(File.ReadAllText("appsettings.json"));
            var environmentVariables = settings["EnvironmentVariables"];

            if (environmentVariables != null)
            {
                var url = environmentVariables["Url"]?.ToString();
                var userName = environmentVariables["UserName"]?.ToString();
                var password = environmentVariables["Password"]?.ToString();
                var appId = environmentVariables["AppId"]?.ToString();
                var redirectUri = environmentVariables["RedirectUri"]?.ToString();

                var connectionString = $"AuthType=OAuth;Url={url};UserName={userName};Password={password};AppId={appId};RedirectUri={redirectUri};LoginPrompt=Auto;RequireNewInstance=True";

                // Connect to Dataverse
                using (var service = new ServiceClient(connectionString))
                {

                    var entityMetadata = RetrieveEntityMetadata(service, "cr226_aisuggestions");
                    if (entityMetadata != null)
                    {
                        Console.WriteLine("Columns in cr226_aisuggestions:");
                        foreach (var attributeMetadata in entityMetadata.Attributes)
                        {
                            Console.WriteLine($"{attributeMetadata.LogicalName} - Type: {attributeMetadata.AttributeType}");
                        }

                        var logicalNames = new List<string>
                        {
                            "cr226_aisuggestionid",
                            "cr226_aisuggestionsid",
                            "cr226_consultantid",
                            "cr226_date",
                            "cr226_enddate",
                            "cr226_isdiscarded",
                            "cr226_ishandled",
                            "cr226_isrepeat",
                            "cr226_name",
                            "cr226_projectid",
                            "cr226_startdate",
                            "cr226_title",
                        };

                        var currentRowValues = GetCurrentRowValues(service, entityMetadata.LogicalName);

                        // Print current row values for included attributes
                        Console.WriteLine("Current Row Values:");
                        foreach (var kvp in currentRowValues)
                        {
                            if (logicalNames.Contains(kvp.Key))
                            {
                                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                                
                            }
                        }

                        // Add new row
                        var newRowId = AddNewRow(service, entityMetadata.LogicalName);
                        Console.WriteLine($"New row added with ID: {newRowId}");
                    }
                    else
                    {
                        Console.WriteLine("Table metadata not found.");
                    }
                }
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

    static Dictionary<string, object> GetCurrentRowValues(ServiceClient service, string entityName)
    {
        // Query to retrieve current row values
        var query = new QueryExpression(entityName);
        query.ColumnSet.AllColumns = true;
        query.TopCount = 1;

        var result = service.RetrieveMultiple(query);
        var entity = result.Entities.FirstOrDefault();

        return entity?.Attributes.ToDictionary(entry => entry.Key, entry => entry.Value);
    }

    static Guid AddNewRow(ServiceClient service, string entityName)
    {
        // Create new entity object
        var newEntity = new Entity(entityName);

        // Set attribute values
        newEntity["cr226_aisuggestionid"] = "0";
        newEntity["cr226_name"] = "New suggestion";
        newEntity["cr226_startdate"] = DateTime.Now;
        newEntity["cr226_enddate"] = DateTime.Now;

        // Generate new unique identifier
        var newRowId = Guid.NewGuid();
        newEntity["cr226_aisuggestionsid"] = newRowId;

        // Create record
        var createdRecordId = service.Create(newEntity);

        return createdRecordId;
    }
}