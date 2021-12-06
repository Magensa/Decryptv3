using System.Collections.Generic;
using System.Text.Json;

namespace DecryptV3.Dtos
{
    public class GenerateMacResponseDto
    {
        public string CustomerTransactionId { get; set; }
        public bool IsReplay { get; set; }
        public string MagTranId { get; set; }
        public string MACedString { get; set; }
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
