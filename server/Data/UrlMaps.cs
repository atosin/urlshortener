using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Data
{
    [BsonIgnoreExtraElements]
    public class UrlMaps
    {
        public string Url { get; set; }
        public string Hash { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastAccessedDate { get; set; }
    }
}
