using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Headers;
using OpenTracker.Data.Models;

namespace OpenTracker.Data.Services
{
    public class Scryfall
    {
        private readonly HttpClient _httpClient;

        public Scryfall(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Card>> SearchForCardAsync(string query)
        {

            //TODO: Check to see if requested card already exists in "cached" database and the time since last updated is greater than a week

            var baseURL = $"https://api.scryfall.com/cards/search?q=name%3A{query}";

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("OpenTracker/1.0");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");

            var response = await _httpClient.GetAsync(baseURL);

            if (response.IsSuccessStatusCode)
            {
                List<Card> ReturnedCards = new List<Card>();
                var jsonString = await response.Content.ReadAsStringAsync();

                using JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

                if (jsonDocument.RootElement.TryGetProperty("data", out JsonElement dataArray))
                {
                    foreach (JsonElement card in dataArray.EnumerateArray())
                    {
                        ReturnedCards.Add(new Card
                        {
                            Name = card.GetProperty("name").GetString(),
                            OracleId = card.GetProperty("oracle_id").GetString(),
                            ImageURL = card.GetProperty("image_uris").GetProperty("small").GetString(),
                            CardURL = card.GetProperty("scryfall_uri").GetString(),
                            Price = Double.TryParse(card.GetProperty("prices").GetProperty("usd").GetString(), out double value) ? value : 0.00,
                            LastUpdated = DateTime.Now,
                            DateCreated = DateTime.Now
                        });
                    }
                }


                // Return the list of cards
                return ReturnedCards;
            }
            else
            {
                // Handle error here
                throw new HttpRequestException("Failed to fetch data from Scryfall API");
            }
        }

    }
}
