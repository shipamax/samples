﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

/*
 * On problems with PostAsJsonAsync try
 * Type the following command into Tools > Library Package Manager > Package Manager Console:
    Install-Package Microsoft.AspNet.WebApi.Client -Version 4.0.20710
    see: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
 */

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

    public class Message
    {
        public string date { get; set; }
        public string sender { get; set; }
        public string subject { get; set; }
        public string receiver { get; set; }
        public string unqid { get; set; }
        public string customId { get; set; }
        public Tonnage[] tonnage { get; set; }
        public EmailVesselSpec[] emailVesselSpec { get; set; }
    }

    public class Messages
    {
        public Message[] results { get; set; }
    }

    public class Tonnage
    {
        public string openDateStart { get; set; }
        public string openDateEnd { get; set; }
        public string openPortName { get; set; }
        public int? openPortId { get; set; }
        public string openRegionName { get; set; }
        public int? openRegionId { get; set; }
        public string openCountryName { get; set; }
        public string openCountryId { get; set; }
        public string vesselName { get; set; }
        public int? vesselId { get; set; }
        public Boolean? isBallast { get; set; }
        public string status { get; set; }
        public string lastCargoes { get; set; }
        public int? sourceId { get; set; }
        public string attachment { get; set; }
        public Boolean? isEta { get; set; }
        public Boolean? spot { get; set; }
        public string color { get; set; }
        public string colorNote { get; set; }
    }

    public class EmailVesselSpec
    {
        public int? vesselId { get; set; }
        public string vesselName { get; set; }
        public int? dwt { get; set; }
        public int? dwtSummer { get; set; }
        public int? dwtWinter { get; set; }
        public int? dwtTropical { get; set; }
        public int? dwtTropicalFresh { get; set; }
        public int? dwtFresh { get; set; }
        public int? buildYear { get; set; }
        public SpeedConsumptions[] speedAndConsumption { get; set; }
    }

    public class SpeedConsumptions
    {
        public Double? speed { get; set; }
        public int? type { get; set; }
        public Consumption[] consumption { get; set; }
    }

    public class Consumption
    {
        public float? quantity { get; set; }
        public string fuel { get; set; }
    }

    public class QueryParam
    {
        public string since { get; set; }
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
                "api/users/login", credentials);
            Console.WriteLine($"Username: {credentials.email}");
            AccessDetails accessDetails = null;
            if (response.IsSuccessStatusCode)
            {
                accessDetails = await response.Content.ReadAsAsync<AccessDetails>();
            }
            return accessDetails;
        }

        static async Task GetMessagesAsync(AccessDetails accessDetails)
        {
            Messages messages = null;
            String sinceDate = DateTime.Now.ToString("yyyy-MM-dd");
            Console.WriteLine($"Getting parsing results since {sinceDate}");
            QueryParam queryParam = new QueryParam
            {
                since = sinceDate,
                access_token = accessDetails.id
            };
            // don't pass access token via URI
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Messages/query", queryParam);
            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<Messages>();
                Console.WriteLine($"Number of messages found: {messages.results.Length}");
                foreach (Message message in messages.results)
                {
                    Console.WriteLine("--- Message ---");
                    Console.WriteLine($"Unique ID: {message.unqid}");
                    Console.WriteLine($"Custom ID (only for messages pushed via API): {message.customId}");
                    Console.WriteLine($"E-mail date: {message.date}");
                    Console.WriteLine($"E-mail sender: {message.sender}");
                    Console.WriteLine($"E-mail subject: {message.subject}");
                    Console.WriteLine($"E-mail receiver: {message.receiver}");
                    foreach (Tonnage tonnage in message.tonnage)
                    {
                        Console.WriteLine("- Ship position -");
                        Console.WriteLine($"vesselId: {tonnage.vesselId}");
                        Console.WriteLine($"vesselName: {tonnage.vesselName}");
                        Console.WriteLine($"openDateStart: {tonnage.openDateStart}");
                        Console.WriteLine($"openDateEnd: {tonnage.openDateEnd}");
                        Console.WriteLine($"openPortName: {tonnage.openPortName}");
                        Console.WriteLine($"openPortId: {tonnage.openPortId}");
                        Console.WriteLine($"openRegionName: {tonnage.openRegionName}");
                        Console.WriteLine($"openRegionId: {tonnage.openRegionId}");
                        Console.WriteLine($"openCountryName: {tonnage.openCountryName}");
                        Console.WriteLine($"openCountryId: {tonnage.openCountryId}");
                        Console.WriteLine($"isBallast: {tonnage.isBallast}");
                        Console.WriteLine($"status: {tonnage.status}");
                        Console.WriteLine($"lastCargoes: {tonnage.lastCargoes}");
                        Console.WriteLine($"sourceId: {tonnage.sourceId}");
                        Console.WriteLine($"attachment: {tonnage.attachment}");
                        Console.WriteLine($"isEta: {tonnage.isEta}");
                        Console.WriteLine($"spot: {tonnage.attachment}");
                        Console.WriteLine($"color: {tonnage.color}");
                        Console.WriteLine($"colorNote: {tonnage.colorNote}");
                    }
                    foreach (EmailVesselSpec emailVesselSpec in message.emailVesselSpec)
                    {
                        Console.WriteLine("- Ship specifications -");
                        Console.WriteLine($"vesselId: {emailVesselSpec.vesselId}");
                        Console.WriteLine($"vesselName: {emailVesselSpec.vesselName}");
                        Console.WriteLine($"dwt: {emailVesselSpec.dwt}");
                        Console.WriteLine($"dwtSummer: {emailVesselSpec.dwtSummer}");
                        Console.WriteLine($"dwtWinter: {emailVesselSpec.dwtWinter}");
                        Console.WriteLine($"dwtTropical: {emailVesselSpec.dwtTropical}");
                        Console.WriteLine($"dwtTropicalFresh: {emailVesselSpec.dwtTropicalFresh}");
                        Console.WriteLine($"dwtFresh: {emailVesselSpec.dwtFresh}");
                        Console.WriteLine($"buildYear: {emailVesselSpec.buildYear}");
                        foreach (SpeedConsumptions speedAndConsumption in emailVesselSpec.speedAndConsumption)
                        {
                            Console.WriteLine("- Speed and Consumption -");
                            Console.WriteLine($"speed: {speedAndConsumption.speed}");
                            Console.WriteLine($"type: {speedAndConsumption.type}");
                            foreach (Consumption consumption in speedAndConsumption.consumption)
                            {
                                Console.WriteLine("- Consumption -");
                                Console.WriteLine($"speed: {consumption.quantity}");
                                Console.WriteLine($"type: {consumption.fuel}");
                            }
                        }
                    }
                }
            } else
            {
                Console.WriteLine(response.StatusCode);
            }
            return;
        }	
		
        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://jupiter.shipamax.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Credentials credentials = new Credentials
                {
                    email = "username",
                    password = "password"
                };

                AccessDetails accessDetails = await LoginAsync(credentials);
                Console.WriteLine($"Logged in and received access token: {accessDetails.id}");
                await GetMessagesAsync(accessDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

    }
}
