using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using ServiceStack.DataAnnotations;

namespace AppreciationProject.DBEntities
{
    public class Messages
    {
        [HashKey]
        public string To_name { get; set; }
        [DynamoDBRangeKey]
        public long Date { get; set; }

        public string Content { get; set; }

        public string From_name { get; set; }

        public string Value { get; set; }

        public string Unread { get; set; }
    }
}
