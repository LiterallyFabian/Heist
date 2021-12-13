using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace Heist
{
    public static class Program
    {
        private static readonly string[] Messages =
        {
            "Right clicked and saved @.", "Downloaded @!", "Took a screenshot of @.", "@ right clicked successfully.", "Screenshot taken of @.", "@ has been added to the database.", "Poor @ got right clicked.", "Saved @.", "Downloaded @."
        };
        private static readonly Random Rnd = new();
        private const string Api = "https://api.opensea.io/api/v1/assets";
        private static string _directory = "./";
        public static void Main(string[] args)
        {
            Console.Write("Enter offset (0): ");
            string? offset = Console.ReadLine();
            if(string.IsNullOrEmpty(offset)) offset = "0";
            
            Console.Write("Enter limit (50):");
            string? limit = Console.ReadLine();
            if(string.IsNullOrEmpty(limit)) limit = "50";
            
            Console.Write("Enter collection (boredapeyachtclub):");
            string? collection = Console.ReadLine();
            if(string.IsNullOrEmpty(collection)) collection = "boredapeyachtclub";
            _directory = "./" + collection;
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);

            
            string apiUrl = Api + "?offset=" + offset + "&limit=" + limit + "&collection=" + collection + "&format=json";
            
            // get the data from the api
            Console.WriteLine("Getting data from " + apiUrl + "...");
            using HttpClient client = new();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContent responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;
                AssetSet? set = JsonConvert.DeserializeObject<AssetSet>(responseString);
                if (set == null) return;
                
                
                foreach(Asset asset in set.Assets)
                {
                    SaveAsset(asset);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response);
            }
        }

        private static void SaveAsset(Asset a)
        {
            // download image and save it to directory folder
            using HttpClient client = new();
            HttpResponseMessage response = client.GetAsync(a.ImageOriginalUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContent responseContent = response.Content;
                byte[] image = responseContent.ReadAsByteArrayAsync().Result;
                string fileName = a.TokenId + ".png";
                File.WriteAllBytes(_directory + "/" + fileName, image);
                
                //write random message
                int messageIndex = Rnd.Next(0, Messages.Length);
                string message = Messages[messageIndex];
                message = message.Replace("@", a.TokenId.ToString());
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("Error: " + response);
            }
        }
        

        /*
         // old shit to prevent their rate limits,  not finished and probably won't be
         
        public static string GetPayload(int cursor, int count = 32)
        {
            string payload = Resources.ResourceManager.GetString("api_payload") ?? string.Empty;
            
            payload = payload.Replace("{cursor}", ToBase64("arrayconnection:"+cursor));
            payload = payload.Replace("{count}", count.ToString());

            return payload;
        }

        public static string ToBase64(string s)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(bytes);
        }
        */
    }
}