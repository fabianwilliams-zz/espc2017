using System;
using Newtonsoft.Json;

namespace minileadgen.core.Models
{
    public class Prospect
    {
        public Prospect()
        {
        }
        [JsonProperty(PropertyName = "pid")]
        string pId { get; set; }
        [JsonProperty(PropertyName = "pfullname")]
        string pFullName { get; set; }
        string pAddressName { get; set; }
        string pAddressCity { get; set; }
        string pAddressRegion { get; set; }
        string pPriority { get; set; }
        string pPhoneNumber { get; set; }
        string pEmail { get; set; }
        bool pConvered { get; set; }
        DateTime pDateAdded { get; set; }
        string pNotes { get; set; }
    }
}
