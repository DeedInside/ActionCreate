using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCreate.classes
{
    public class DataSet
    {
        public DataSet(Dictionary<string, data> database, List<Taxanomy> taxanomy)
        {
            Database = database;
            Taxanomy = taxanomy;
        }

        [JsonProperty("database")]
        public Dictionary<string, data> Database { get; set; }

        [JsonProperty("taxanomy")]
        public List<Taxanomy> Taxanomy { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; } = "v1.0";
    }
}

/*
    "database": database,
        "taxanomy": [
            {
                "nodeId": 0,
                "nodeName": "Root",
                "parentId": None,
                "paretName": None,
            },
            {
                "nodeId": 1,
                "nodeName": label_name,
                "parentId": 0,
                "paretName": "Root",
            },
        ],
        "version": "v1.0",
 */
