using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class MPesaTransactionModel
    {
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public string ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public string USER_ID { get; set; }
        public List<CallbackMetadata> CallbackMetadata { get; set; }
    }

    public class MPesaProcessModel
    {
        public string BusinessShortCode { get; set; }
        public string Password { get; set; }
        public string Timestamp { get; set; }
        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public string PartyA { get; set; }
        public string PartyB { get; set; }
        public string PhoneNumber { get; set; }
        public string CallBackURL { get; set; }
        public string AccountReference { get; set; }
        public string TransactionDesc { get; set; }

    }
    public class GenerateTokenModel
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }

    public class CallbackMetadata
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class MPesaValidation
    {
        public long CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_MOBILE { get; set; }
        public string MPESA_AMOUNT { get; set; }
        public string MPESA_CALLBACK_REQUEST { get; set; }
        public string MPESA_PROCESS_RESPONSE { get; set; }
        public long USER_ID { get; set; }
    }
}
