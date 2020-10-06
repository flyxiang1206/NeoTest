using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Neo4jClientTest
{
    public class Sql
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
