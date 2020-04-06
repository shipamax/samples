using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleApp1
{
    public class Credentials
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class AccessDetails
    {
        public string id { get; set; }
    }

    public class DocumentContainers
    {
        public DocumentContainer[] results { get; set; }
    }

    public class DocumentContainer
    {
        public string unqId { get; set; }
    }

    public class CustomQueryParam
    {
        public string[] customIds { get; set; }
        public string access_token { get; set; }
    }
	
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task<AccessDetails> LoginAsync(Credentials credentials)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/v1/users/login", credentials);
            Console.WriteLine($"Username: {credentials.email}");
            AccessDetails accessDetails = null;
            if (response.IsSuccessStatusCode)
            {
                accessDetails = await response.Content.ReadAsAsync<AccessDetails>();
            }
            return accessDetails;
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://developer.shipamax-api.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Credentials credentials = new Credentials
                {
                    email = "[YOUR-USERNAME]",
                    password = "[YOUR-PASSWORD]"
                };

                AccessDetails accessDetails = await LoginAsync(credentials);
                Console.WriteLine($"Logged in and received access token: {accessDetails.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

    }
}
