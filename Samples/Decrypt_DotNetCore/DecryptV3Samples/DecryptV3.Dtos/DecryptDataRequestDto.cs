using System.Collections.Generic;

namespace DecryptV3.Dtos
{
    public class DecryptDataRequestDto
    {
        public string BillingLabel { get; set; }
        public string CustomerTransactionID { get; set; }
        public string EncryptedData { get; set; }
        public string KSN { get; set; }
        public string KeyType { get; set; }
        public string CustomerCode { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public List<KeyValuePair<string, string>> AdditionalRequestData { get; set; }
    }
}
