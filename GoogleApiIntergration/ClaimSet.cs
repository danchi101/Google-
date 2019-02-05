using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleApiIntergration
{
    public class ClaimSet
    {
        /// <summary>
        /// email address of the service account
        /// </summary>
        [JsonProperty("iss")]
        public string Email { get; set; }

        /// <summary>
        /// A space-delimited list of the permissions that the application requests.
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// A descriptor of the intended target of the assertion. When making an access token request this value is always 
        /// https://www.googleapis.com/oauth2/v4/token.
        /// </summary>
        [JsonProperty("aud")]
        public string Audience { get; set; }

        /// <summary>
        /// The expiration time of the assertion, specified as seconds since 00:00:00 UTC, January 1, 1970. 
        /// This value has a maximum of 1 hour after the issued time.
        /// </summary>
        [JsonProperty("exp")]
        public int Expiry { get; set; }

        /// <summary>
        /// The time the assertion was issued, specified as seconds since 00:00:00 UTC, January 1, 1970.
        /// </summary>
        [JsonProperty("iat")]
        public int IssuedAt { get; set; }
    }
}
