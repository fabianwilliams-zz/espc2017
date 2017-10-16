using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace minileadgen.core.Models
{
    public class Lead
    {
        [JsonProperty(PropertyName = "pid")]
        public string pId { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }
        [JsonProperty(PropertyName = "name")]
        public Name Name { get; set; }
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        [JsonProperty(PropertyName = "contact")]
        public Contact Contact { get; set; }
        [JsonProperty(PropertyName = "interest")]
        public string Interest { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "isconverted")]
        public bool IsConverted { get; set; }
        [JsonProperty(PropertyName = "dateadded")]
        public DateTime DateAdded { get; set; }
    }

    public class Address
    {
        public string type { get; set; }
        public string streetNumber { get; set; }
        public string streetName { get; set; }
        public string aptNum { get; set; }
        public string city { get; set; }
        public string region { get; set; }
    }

    public class Name
    {
        public string firstname { get; set; }
        public string middle { get; set; }
        public string last { get; set; }
        public string suffix { get; set; }
    }

    public class Contact
    {
        public string phone { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }

}
