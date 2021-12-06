using System.Collections.Generic;
using System.Text.Json;

namespace DecryptV3.Dtos
{
    public class DecryptCardSwipeResponseDto
    {
        public bool IsReplay { get; set; }
        public string MagTranId { get; set; }
        public string CardID { get; set; }
        public DecryptedCardSwipe DecryptedCardSwipe { get; set; }
        public decimal MagnePrintScore { get; set; }
        public KeyValuePair<string, string>[] AdditionalResponseData { get; set; }


        public override string ToString()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            return json;
        }
    }
}
