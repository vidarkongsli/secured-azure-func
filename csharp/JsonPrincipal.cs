using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace secured_azure_func
{
    public class JsonPrincipal
    {
        [JsonProperty("auth_typ")]
        public string AuthenticationType { get; set; }

        public ICollection<Claim> Claims { get; set; }

        [JsonProperty("name_typ")]
        public string NameClaimType { get; set; }

        [JsonProperty("role_typ")]
        public string RoleClaimType { get; set; }

        public string Name => Claims.Single(c => c.Type == NameClaimType).Value;

        public static JsonPrincipal CreateFrom(string s)
        {
            var principalDataBytes = Convert.FromBase64String(s);
            var principalData = Encoding.UTF8.GetString(principalDataBytes);
            return JsonConvert.DeserializeObject<JsonPrincipal>(principalData);
        }

        public class Claim
        {
            [JsonProperty("typ")]
            public string Type { get; set; }

            [JsonProperty("val")]
            public string Value { get; set; }
        }
    }
}