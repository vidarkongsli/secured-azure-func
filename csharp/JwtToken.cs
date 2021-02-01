using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace secured_azure_func
{
    public class JwtToken
    {
        [JsonProperty("aud")]
        public string Audience { get; set; }
        
        public string Email { get; set; }

        [JsonProperty("exp")]
        public int Expiry { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("idp")]
        public string IdentityProvider { get; set; }

        [JsonProperty("unique_name")]
        public string UniqueName { get; set; }

        [JsonProperty("oid")]
        public Guid ObjectId { get; set; }

        [JsonProperty("iss")]
        public string Issuer { get; set; }

        [JsonProperty("ver")]
        public string Version { get; set; }

        public static JwtToken CreateFrom(string jwtString)
        {
            var t = jwtString.Split('.')
                .Skip(1).First()
                .Replace('_', '/').Replace('-', '+');
            switch (t.Length % 4)
            {
                case 2: t += "=="; break;
                case 3: t += "="; break;
            }
            var bytes = Convert.FromBase64String(t);
            var originalText = Encoding.ASCII.GetString(bytes);
            return JsonConvert.DeserializeObject<JwtToken>(originalText);
        }
    }
}