using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.People.v1;
using Google.Apis.People.v1.Data;
using Google.Apis.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoogleApiIntergration
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var header = new Header
            //{
            //    Algorithmn = "SHA256",
            //    Type = "JWT"
            //};

            //var headerEncoded = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(header));

            //var claimSet = new ClaimSet
            //{
            //    Audience = "https://www.googleapis.com/oauth2/v4/token",
            //    Email = "761326798069-r5mljlln1rd4lrbhg75efgigp36m78j5@developer.gserviceaccount.com",
            //    Expiry = 1328554385,
            //    IssuedAt = 1328550785
            //};

            //var claimSetEncoded = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(claimSet));
            //var signature = new Signature().Encode(header, claimSet);

            //var base64Encoded = $"{headerEncoded}.{claimSetEncoded}.{signature}";

            //var certificate = new X509Certificate2(@"deployment_cert.der");

            //ServiceAccountCredential credential = new ServiceAccountCredential(
            //   new ServiceAccountCredential.Initializer("email")
            //   {

            //   }..FromCertificate(certificate));

            //var credential = ServiceAccountCredential.FromServiceAccountData(/*File.OpenRead(@"C:\Users\danchi\Desktop\logos.json")*/)
            var scope = DriveService.Scope.Drive;
            var credential = GoogleCredential.FromStream(File.OpenRead(@"C:\Users\danchi\Desktop\logos.json"))
                                   .CreateScoped("https://www.googleapis.com/auth/contacts.readonly")
                                   .UnderlyingCredential as ServiceAccountCredential;
            // Create the service.
            var service = new PeopleService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "logos",
            });

            PeopleResource.ConnectionsResource.ListRequest peopleRequest =
   service.People.Connections.List("people/me");
            //peopleRequest.PersonFields = "names,emailAddresses";
            ListConnectionsResponse connectionsResponse = peopleRequest.Execute();
            IList<Person> connections = connectionsResponse.Connections;
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
}
