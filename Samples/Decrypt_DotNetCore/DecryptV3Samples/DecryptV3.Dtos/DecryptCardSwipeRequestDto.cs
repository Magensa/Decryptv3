using System.Collections.Generic;

namespace DecryptV3.Dtos
{
    public class DecryptCardSwipeRequestDto
    {
        public string BillingLabel { get; set; }
        public string CustomerTransactionID { get; set; }
        public string CustomerCode { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string DeviceSN { get; set; }
        public string KSN { get; set; }
        public string KeyType { get; set; }
        public string MagnePrint { get; set; }
        public string MagnePrintStatus { get; set; }
        public string Track1 { get; set; }
        public string Track2 { get; set; }
        public string Track3 { get; set; }
        public List<KeyValuePair<string, string>> AdditionalRequestData { get; set; }
    }
}
