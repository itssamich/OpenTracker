using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Internal;
using System.Formats.Asn1;


namespace OpenTracker.Data.Services
{
    public class CSVDocument
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;

        private readonly IJSRuntime _jsRuntime;

        public CSVDocument(IJSRuntime jsRuntime, IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context;
            _jsRuntime = jsRuntime;
        }
        public async Task GenerateAndDownloadCsvAsync(Guid SessionId)
        {
            using var dbContext = await _context.CreateDbContextAsync();
            var CardList = await dbContext.CardSessions.Where(s => s.SessionId == SessionId).Include(c => c.Card).ToListAsync();
            var SessionName = await dbContext.Sessions.Where(s => s.SessionId == SessionId).Select(s => s.SessionName).FirstOrDefaultAsync();


            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Quantity,Name,"); // Header row

            foreach (var card in CardList)
            {
                csvBuilder.AppendLine($"{card.CardQuantity}, \"\", \"{card.Card.Name}\",{card.Card.SetName}, 'NM', 'EN', 'Normal' ,\"\", \"\", {card.Card.SetNumber}, \"\", \"\", \"\""); // Adjust fields
            }

            byte[] fileBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            string fileName = $"export_{SessionName}.csv";

            await DownloadFileAsync(fileName, fileBytes);
        }
        private async Task DownloadFileAsync(string fileName, byte[] fileBytes)
        {
            using var streamRef = new DotNetStreamReference(new MemoryStream(fileBytes));
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
    }
}
