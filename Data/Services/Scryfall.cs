using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Headers;
using OpenTracker.Data.Models;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;



namespace OpenTracker.Data.Services
{
    public class Scryfall
    {
        private readonly HttpClient _httpClient;

        private readonly IDbContextFactory<ApplicationDbContext> _context;

        public Scryfall(HttpClient httpClient, IDbContextFactory<ApplicationDbContext> context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<List<Card>> SearchForCardAsync(string query)
        {
            
            //TODO: Check to see if requested card already exists in "cached" database and the time since last updated is greater than a week
            Card CardData;
            List<Card> ReturnedCards = new List<Card>();

            using var dbContext = await _context.CreateDbContextAsync();

            CardData = await dbContext.Cards.Where(c => c.Name == query).FirstOrDefaultAsync();

            if (CardData != null)
            {
                //TODO: Using ORACLEID, pull the cards data and update the price and last updated value
                ReturnedCards.Add(CardData);
                return ReturnedCards;
            }

            var baseURL = $"https://api.scryfall.com/cards/search?q=name%3A{query}";

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("OpenTracker/1.0");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");

            var response = await _httpClient.GetAsync(baseURL);

            if (response.IsSuccessStatusCode)
            {

                var jsonString = await response.Content.ReadAsStringAsync();

                using JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

                if (jsonDocument.RootElement.TryGetProperty("data", out JsonElement dataArray))
                {
                    foreach (JsonElement card in dataArray.EnumerateArray())
                    {
                        Card NewCard = new Card
                        {
                            Name = card.GetProperty("name").GetString(),
                            OracleId = card.GetProperty("oracle_id").GetString(),
                            SetName = card.GetProperty("set").GetString(),
                            SetNumber = int.TryParse(card.GetProperty("collector_number").ToString(), out int setNum) ? setNum : 000,
                            ImageURL = card.GetProperty("image_uris").GetProperty("small").GetString(),
                            CardURL = card.GetProperty("scryfall_uri").GetString(),
                            Price = Decimal.TryParse(card.GetProperty("prices").GetProperty("usd").GetString(), out Decimal value) ? value : 0,
                            LastUpdated = DateTime.Now,
                            DateCreated = DateTime.Now
                        };
                        ReturnedCards.Add(NewCard);
                        dbContext.Cards.Add(NewCard);
                    }
                }

                await dbContext.SaveChangesAsync();
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
