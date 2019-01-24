using System;
using System.Collections.Generic;
using System.Globalization;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using ServiceStack.Aws;
using ServiceStack.Aws.DynamoDb;

namespace AppreciationProject.DataAccess
{
    public class DynamoDB
    {
        private readonly IConfiguration configuration;

        public DynamoDB(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async void MarkReadAsync(string toName, double dateTime)
        {
            using (AmazonDynamoDBClient raw = new AmazonDynamoDBClient(GetCredentials(), RegionEndpoint.APSoutheast2))
            {
                Table table = Table.LoadTable(raw, "Messages");

                GetItemOperationConfig config = new GetItemOperationConfig()
                {
                    AttributesToGet = new List<string>() { "To_name", "Date", "Content", "From_name", "Unread", "Value" },
                    ConsistentRead = true
                };
                Document doc =  await table.GetItemAsync(toName, dateTime, config);

                long date = doc["Date"].AsLong();
                string to_name = doc["To_name"].AsString();
                string from_name = doc["From_name"].AsString();
                string content = doc["Content"].AsString();
                string value = doc["Value"].AsString();
                bool unread = doc["Unread"].AsBoolean();

                await table.DeleteItemAsync(to_name, date);

                Table Updatetable = Table.LoadTable(raw, "Messages");
                var message = new Document();
                message["To_name"] = to_name; // Primary key.
                message["Date"] = date;
                message["Content"] = content;
                message["From_name"] = from_name;
                message["Unread"] = "false";
                message["Value"] = value;

                await Updatetable.PutItemAsync(message);

            }
        }

        public List<AppreciationCards.Models.Messages> GetUnread()
        {
            using (AmazonDynamoDBClient raw = new AmazonDynamoDBClient(GetCredentials(), RegionEndpoint.APSoutheast2))
            {
                var dynamo = new PocoDynamo(raw);
                var request = new ScanRequest
                {
                    TableName = "Messages",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                        {":val", new AttributeValue { S = "true" }}
                    },
                    FilterExpression = "Unread = :val",
                };
                return QueryDB(request,raw);
            }

        }

        public List<AppreciationCards.Models.Messages> GetSpecificPeriod(DateTime? dateFrom, DateTime? dateTo)
        {
            using (AmazonDynamoDBClient raw = new AmazonDynamoDBClient(GetCredentials(), RegionEndpoint.APSoutheast2))
            {
                PocoDynamo dynamo = new PocoDynamo(raw);
                ScanRequest request;

                if (dateFrom.HasValue && dateTo.HasValue)
                {
                    request = new ScanRequest
                    {
                        TableName = "Messages",
                        ExpressionAttributeNames = new Dictionary<string, string>
                        {
                            {"#date", "Date" }
                        },
                        ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                        {
                            {":from", new AttributeValue { N = ""
                                                                + dateFrom.Value.Year
                                                                + (dateFrom.Value.Month < 10 ? "0" + dateFrom.Value.Month : "" + dateFrom.Value.Month)
                                                                + (dateFrom.Value.Day < 10 ? "0" + dateFrom.Value.Day : "" + dateFrom.Value.Day)
                                                                + "000000"
                            }},
                            {":to", new AttributeValue { N = ""
                                                                + dateTo.Value.Year
                                                                + (dateTo.Value.Month < 10 ? "0" + dateTo.Value.Month : "" + dateTo.Value.Month)
                                                                + (dateTo.Value.Day < 10 ? "0" + dateTo.Value.Day : "" + dateTo.Value.Day)
                                                                + "235959"
                            }}
                        },
                        FilterExpression = "#date between :from and :to",
                    };
                }

                else if (dateFrom.HasValue)
                {
                    request = new ScanRequest
                    {
                        TableName = "Messages",
                        ExpressionAttributeNames = new Dictionary<string, string>
                        {
                            { "#date", "Date" }
                        },
                        ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                        {
                            {":from", new AttributeValue { N = ""
                                                                + dateFrom.Value.Year
                                                                + (dateFrom.Value.Month < 10 ? "0" + dateFrom.Value.Month : "" + dateFrom.Value.Month)
                                                                + (dateFrom.Value.Day < 10 ? "0" + dateFrom.Value.Day : "" + dateFrom.Value.Day)
                                                                + "000000"
                            } }
                        },
                       FilterExpression = "#date >= :from",
                    };
                }

                else if (dateTo.HasValue)
                {
                    request = new ScanRequest
                    {
                        TableName = "Messages",
                        ExpressionAttributeNames = new Dictionary<string, string>
                        {
                            {"#date", "Date" }
                        },
                        ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                        {
                            {":to", new AttributeValue { N = ""
                                                                + dateTo.Value.Year
                                                                + (dateTo.Value.Month < 10 ? "0" + dateTo.Value.Month : "" + dateTo.Value.Month)
                                                                + (dateTo.Value.Day < 10 ? "0" + dateTo.Value.Day : "" + dateTo.Value.Day)
                                                                + "235959"
                            }}
                        },
                        FilterExpression = "#date <= :to",
                    };
                }

                else
                {
                    request = new ScanRequest
                    {
                        TableName = "Messages",
                    };
                }

                return QueryDB(request, raw);
            }
        }

        public List<AppreciationCards.Models.Messages> Get()
        {
            using (AmazonDynamoDBClient raw = new AmazonDynamoDBClient(GetCredentials(), RegionEndpoint.APSoutheast2))
            {
                var dynamo = new PocoDynamo(raw);
                var request = new ScanRequest
                {
                    TableName = "Messages",
                };
                return QueryDB(request,raw);
            }
        }

        public List<AppreciationCards.Models.Messages> QueryDB( ScanRequest request, AmazonDynamoDBClient raw)
        {  

            List<AppreciationCards.Models.Messages> allMessages = new List<AppreciationCards.Models.Messages>();
            ScanResponse response = null;
            do
            {
               if (response != null)
                 request.ExclusiveStartKey = response.LastEvaluatedKey;

               response = raw.Scan(request);

               foreach (var item in response.Items)
               {

                string DatenTime = item["Date"].N;
                string format = "yyyyMMddHHmmss";
                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime result = DateTime.ParseExact(DatenTime, format, provider);

                var messages= new AppreciationCards.Models.Messages()
                 {
                   FromName = item["From_name"].S,
                   ToName =  item["To_name"].S,
                   Content = item["Content"].S,
                   Value = item["Value"].S,
                   Unread = item["Unread"].BOOL,
                   MessageDate = result
                };
                     allMessages.Add(messages);
               }
            } while (response.LastEvaluatedKey != null && response.LastEvaluatedKey.Count > 0);
            return allMessages;
            
        }

        public void Save<T>(T value) where T : class
        {
            using (AmazonDynamoDBClient raw = new AmazonDynamoDBClient(GetCredentials(), RegionEndpoint.APSoutheast2))
            {
                var dynamo = new PocoDynamo(raw);
                dynamo.RegisterTable<T>();
                dynamo.InitSchema();
                dynamo.PutItem(value);
            }
        }

        public void Delete(string toName, long dateTime) 
        {
            using (AmazonDynamoDBClient raw = new AmazonDynamoDBClient(GetCredentials(), RegionEndpoint.APSoutheast2))
            {
                Table table = Table.LoadTable(raw, "Messages");
                table.DeleteItemAsync(toName, dateTime);
                
            }
        }
        private AWSCredentials GetCredentials()
        {
            return new SessionAWSCredentials(
                configuration["AWS:AccessKeyId"],
                configuration["AWS:SecretAccessKey"],
                configuration["AWS:SessionToken"]);
        }
    }
}