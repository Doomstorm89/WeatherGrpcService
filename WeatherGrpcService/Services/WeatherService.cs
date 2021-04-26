using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherGrpcService
{
    public class WeatherService : Weather.WeatherBase
    {
        private Random r;
        private readonly ILogger<WeatherService> _logger;
        public WeatherService(ILogger<WeatherService> logger)
        {
            _logger = logger;
        }

        public override Task<WeatherReply> GetWeather(WeatherRequest request, ServerCallContext context)
        {
            
            string json = new WebClient().DownloadString(
                $"https://api.openweathermap.org//data/2.5/weather?" +
                $"q={request.Name}" +
                $"&units=metric" +
                $"&lang=ru" +
                $"&appid=2e339f540b2fd338f75e7208a1ba8023");

            var weatherDesc = JObject.Parse(json)["weather"].ToArray();
            string desc="";
            
            foreach (var item in weatherDesc)
            {
                desc = item["description"].ToString();
            }

            return Task.FromResult(new WeatherReply
            {
                Message = $"Â " + request.Name + $" {desc}.\n"
            });
        }
    }
}
