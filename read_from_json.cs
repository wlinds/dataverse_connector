using System;
using System.IO;
using Newtonsoft.Json.Linq;

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

            // Assign variables
            string url = (string)environmentVariables["Url"];
            string userName = (string)environmentVariables["UserName"];
            string password = (string)environmentVariables["Password"];
            string appId = (string)environmentVariables["AppId"];
            string redirectUri = (string)environmentVariables["RedirectUri"];

            Console.WriteLine($"Url: {url}");
            Console.WriteLine($"UserName: {userName}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"AppId: {appId}");
            Console.WriteLine($"RedirectUri: {redirectUri}");
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
}