//-----------------------------------------------------------------------
// <copyright file="WorldClock.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    internal sealed class WorldClock : IDisposable
    {
        private readonly HttpClient client;

        public WorldClock()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("http://worldclockapi.com/");
        }

        public async Task<DateTime> GetTimeAsync()
        {
            string json = await this.client.GetStringAsync("api/json/utc/now");

            // HACK: Real production code would use a proper JSON library
            foreach (string kv in json.Split(','))
            {
                string[] keyValue = kv.Split(':');
                if (keyValue[0] == "\"currentFileTime\"")
                {
                    return DateTime.FromFileTimeUtc(long.Parse(keyValue[1]));
                }
            }

            throw new FormatException("Invalid data received: " + json);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }
}
