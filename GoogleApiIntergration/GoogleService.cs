using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GoogleApiIntergration
{
    public class GoogleService
    {
        private readonly HttpClient client = new HttpClient();
        public const string PurchaseUrl = "https://www.googleapis.com/androidpublisher/v3/applications/packageName/purchases/subscriptions/subscriptionId/tokens/token";
        private readonly GoogleOptions options;
        public async Task<PurchaseSubsription> GetPurchaseDetailsAsync(string packageName, string subscriptionId, string purchaseToken)
        {
            var token = await GetAccessTokenFromJSONKeyAsync("path to json file", "scopes to add");

            var url = $"{options.BaseUrl}/{packageName}/purchases/subscription/{subscriptionId}/tokens/{purchaseToken}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);


            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //send response
            var response = await client.SendAsync(request);

            //check result
            var body = await response.Content.ReadAsStringAsync();

            //capture failed event
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed with Code {response.StatusCode} and Body= {body}");
            }
            return JsonConvert.DeserializeObject<PurchaseSubsription>(body);
        }

        /// <summary>  
        /// Get Access Token From JSON Key Async  
        /// </summary>  
        /// <param name="jsonKeyFilePath">Path to your JSON Key file</param>  
        /// <param name="scopes">Scopes required in access token</param>  
        /// <returns>Access token as string Task</returns>  
        public static async Task<string> GetAccessTokenFromJSONKeyAsync(string jsonKeyFilePath, params string[] scopes)
        {
            using (var stream = new FileStream(jsonKeyFilePath, FileMode.Open, FileAccess.Read))
            {
                return await GoogleCredential
                    .FromStream(stream) // Loads key file  
                    .CreateScoped(scopes) // Gathers scopes requested  
                    .UnderlyingCredential // Gets the credentials  
                    .GetAccessTokenForRequestAsync(); // Gets the Access Token  
            }
        }
    }

    public class PurchaseSubsription
    {
        /// <summary>
        /// This kind represents a subscriptionPurchase object in the androidpublisher service.
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// Time at which the subscription was granted, in milliseconds since the Epoch.	
        /// </summary>
        public long StartTimeMilis { get; set; }

        /// <summary>
        /// Time at which the subscription will expire, in milliseconds since the Epoch.	
        /// </summary>
        public long ExpiryTimeMilis { get; set; }

        /// <summary>
        /// Whether the subscription will automatically be renewed when it reaches its current expiry time.	
        /// </summary>
        public bool AutoRenewing { get; set; }

        /// <summary>
        /// ISO 4217 currency code for the subscription price. For example, 
        /// if the price is specified in British pounds sterling, price_currency_code is "GBP".
        /// </summary>
        public string PriceCurrencyCode { get; set; }

        /// <summary>
        /// Price of the subscription, not including tax. Price is expressed in micro-units,
        /// where 1,000,000 micro-units represents one unit of the currency.
        /// For example, if the subscription price is €1.99, price_amount_micros is 1990000.
        /// </summary>
        public long PriceAmountMicros { get; set; }

        /// <summary>
        /// 	ISO 3166-1 alpha-2 billing country/region code of the user at the time the subscription was granted.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// A developer-specified string that contains supplemental information about an order.
        /// </summary>
        public string DeveloperPayload { get; set; }

        /// <summary>
        /// The payment state of the subscription. Possible values are:
        ///1.Payment pending
        ///2.Payment received
        ///3.Free trial
        /// </summary>
        public int PaymentState { get; set; }

        /// <summary>
        /// The reason why a subscription was canceled or is not auto-renewing. Possible values are:
        ///0.User canceled the subscription
        ///1.Subscription was canceled by the system, for example because of a billing problem
        ///2.Subscription was replaced with a new subscription
        ///3.Subscription was canceled by the developer
        /// </summary>
        public int CancelReason { get; set; }

        /// <summary>
        /// The time at which the subscription was canceled by the user,
        /// in milliseconds since the epoch. Only present if cancelReason is 0.
        /// </summary>
        public long UserCancellationTimeMillis { get; set; }

        /// <summary>
        /// Information provided by the user when they complete the subscription cancellation flow (cancellation reason survey).
        /// </summary>
        public CancelSurveyResult CancelSurveyResult { get; set; }

        /// <summary>
        /// The order id of the latest recurring order associated with the purchase of the subscription.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// The purchase token of the originating purchase if this subscription is one of the following:
        ///Re-signup of a canceled but non-lapsed subscription
        ///Upgrade/downgrade from a previous subscription
        /// </summary>
        public string LinkedPurchaseToken { get; set; }

        /// <summary>
        /// The type of purchase of the subscription. This field is only set if this purchase was not made using the standard in-app billing flow. Possible values are:
        ///0.Test(i.e.purchased from a license testing account)
        /// </summary>
        public int purchaseType { get; set; }

        /// <summary>
        /// 	The profile name of the user when the subscription was purchased.
        /// 	Only present for purchases made with 'Subscribe with Google'.
        /// </summary>
        public string ProfileName { get; set; }

        /// <summary>
        /// The email address of the user when the subscription was purchased. Only present for purchases made with 'Subscribe with Google'.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 	The given name of the user when the subscription was purchased. 
        /// 	Only present for purchases made with 'Subscribe with Google'.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// The profile id of the user when the subscription was purchased. 
        /// Only present for purchases made with 'Subscribe with Google'.
        /// </summary>
        public string ProfileId { get; set; }
    }
}
