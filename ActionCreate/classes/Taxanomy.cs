using Newtonsoft.Json;

namespace ActionCreate.classes
{
    public class Taxanomy
    {
        [JsonProperty("nodeId")]
        public string NodeId { get; set; } = "0";

        [JsonProperty("nodeName")]
        public string NodeName { get; set; } = "Root";

        [JsonProperty("parentId")]
        public string ParentId { get; set; } = "None";

        [JsonProperty("paretName")]
        public string ParetName { get; set; } = "None";
    }
}

/*
    "taxanomy": [
            {
                "nodeId": 0,
                "nodeName": "Root",
                "parentId": None,
                "paretName": None,
            },
        ],
 */
