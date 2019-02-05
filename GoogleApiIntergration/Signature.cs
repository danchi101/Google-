using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace GoogleApiIntergration
{
    public class Signature
    {
        public string Encode(Header header, ClaimSet claimSet)
        {
            var serializedHeader = JsonConvert.SerializeObject(header);
            var serializedClaimSet = JsonConvert.SerializeObject(claimSet);

            var encodedstring=$"{ Base64UrlEncoder.Encode(serializedHeader)}.{Base64UrlEncoder.Encode(serializedClaimSet)}";

            var signature = "";
            using (RSACryptoServiceProvider rsa=new RSACryptoServiceProvider())
            {
                byte[] value = Encoding.UTF8.GetBytes(encodedstring);

                var signed = rsa.SignData(value, "SHA256");
                signature = Encoding.UTF8.GetString(signed);
            }
            return signature;
        }
    }
}
