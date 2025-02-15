using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Headers;
using OpenTracker.Data.Models;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



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

        public async Task<List<Card>> SearchForCardAsync(string query, string optional = "", string set = "")
        {
            
            //TODO: Check to see if requested card already exists in "cached" database and the time since last updated is greater than a week
            Card CardData;
            List<Card> ReturnedCards = new List<Card>();

            using var dbContext = await _context.CreateDbContextAsync();


            //This section can be moved to the addcardtosession function as the query name will rarely be the exact card name
            //CardData = await dbContext.Cards.Where(c => c.Name == query).FirstOrDefaultAsync();

            //if (CardData != null)
            //{
            //    //TODO: Using ORACLEID, pull the cards data and update the price and last updated value
            //    ReturnedCards.Add(CardData);
            //    return ReturnedCards;
            //}


            //To find all artworks of a specific card change the q=name to q=@@name
            //to find all versions of a speicifc card change the q=name to q=++name
            //To restrict search to only a specific set, change the query to q=set%3A{setcode}+name%3A{query}
            //To search for a specific card in a set using the card number, changea the query to  q=set%3A{setcode}+cn%3A{number}
            var baseURL = $"https://api.scryfall.com/cards/search?q={optional}{(set == "" ? "" : $"set%3A{set}+")}{query}";


            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("OpenTracker/1.0");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");

            var response = await _httpClient.GetAsync(baseURL);

            //Handle Errors and responses properly

            if (response.IsSuccessStatusCode)
            {

                var jsonString = await response.Content.ReadAsStringAsync();

                using JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

                if (jsonDocument.RootElement.TryGetProperty("data", out JsonElement dataArray))
                {
                    foreach (JsonElement card in dataArray.EnumerateArray())
                    {
                        String ImgURL = "";
                        if (!card.TryGetProperty("image_uris", out JsonElement imgurl))
                        {
                            ImgURL = card.GetProperty("card_faces")[0].GetProperty("image_uris").GetProperty("small").ToString();

                        }
                        else
                        {
                            ImgURL = card.GetProperty("image_uris").GetProperty("small").GetString();
                        }
                        Card NewCard = new Card
                        {
                            Name = card.GetProperty("name").GetString(),
                            OracleId = card.GetProperty("oracle_id").GetString(),
                            SetName = card.GetProperty("set").GetString(),
                            SetNumber = int.TryParse(card.GetProperty("collector_number").ToString(), out int setNum) ? setNum : 000,
                            ImageURL = ImgURL,
                            CardURL = card.GetProperty("scryfall_uri").GetString(),
                            Price = Decimal.TryParse(card.GetProperty("prices").GetProperty("usd").GetString(), out Decimal value) ? value : 0,
                            LastUpdated = DateTime.Now,
                            DateCreated = DateTime.Now
                        };


                        Card ExistingCard;

                        ExistingCard = await dbContext.Cards.Where(c => c.OracleId == NewCard.OracleId && c.SetNumber == NewCard.SetNumber).FirstOrDefaultAsync();

                        if (ExistingCard is not null)
                        {
                            if (ExistingCard.LastUpdated < DateTime.Now.AddDays(-14))
                            {
                                ExistingCard.Price = NewCard.Price;
                                ExistingCard.LastUpdated = DateTime.Now;
                                dbContext.Update(ExistingCard);
                            }
                            ReturnedCards.Add(ExistingCard);
                        }
                        else
                        {
                            dbContext.Cards.Add(NewCard);
                            ReturnedCards.Add(NewCard);
                        }

                        await dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    //TODO: If query fails, present any cards with similar name in DB
                    ReturnedCards = await dbContext.Cards.Where(c => c.Name.Contains(query)).ToListAsync();
                }
                // Return the list of cards
                return ReturnedCards;
            }
            else
            {
                // Handle error here
                //throw new HttpRequestException("Failed to fetch data from Scryfall API");

                ReturnedCards = await dbContext.Cards.Where(c => c.Name.Contains(query)).ToListAsync();

                return ReturnedCards;
            }
        }

    }
}
