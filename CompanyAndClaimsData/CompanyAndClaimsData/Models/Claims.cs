using Newtonsoft.Json;

namespace CompanyAndClaimsData.Models
{
    public class Claims
    {
        public int CompanyId { get; set; }

        public string UCR {  get; set; }

        public DateTime ClaimDate { get; set; }

        public DateTime LossDate { get; set; }

        [JsonProperty("Assured Name")]
        public string AssuredName { get; set; }

        [JsonProperty("Incurred Loss")]
        public string IncurredLoss { get; set; }

        public bool closed { get; set; }
    }
}
