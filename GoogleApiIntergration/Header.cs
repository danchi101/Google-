using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleApiIntergration
{
    public class Header
    {
        /// <summary>
        /// the type of algorithmn used
        /// </summary>
        [JsonProperty("alg")]
        public string Algorithmn { get; set;}

        /// <summary>
        /// format of the assertion
        /// </summary>
        [JsonProperty("typ")]       
        public string Type{ get; set; }
    }
}
