using System;
using System.Collections.Generic;
using AppreciationCards.Models;

namespace AppreciationProject.DataAccess
{
    public class MessagesRepository
    {
        private readonly DynamoDB dynamoDb;

        public MessagesRepository(DynamoDB dynamoDb)
        {
            this.dynamoDb = dynamoDb;
        }

        public void SaveAppreciation(DBEntities.Messages messages)
        {
            dynamoDb.Save(messages);
        }

        public void MarkReadAppreciation(string toName, double dateTime)
        {
            dynamoDb.MarkReadAsync(toName, dateTime);
        }

        public void DelAppreciation(string ToName, long DateTime)
        {
            dynamoDb.Delete(ToName,DateTime);

        }
        public List<Messages> LoadAppreciation() 
        {
           return dynamoDb.Get();
        }
        public List<Messages> LoadUnreadAppreciation() 
        {
           return dynamoDb.GetUnread();
        }

        public List<Messages> LoadAppreciationWithinPeriod(DateTime? fromDate, DateTime? toDate)
        {
            return dynamoDb.GetSpecificPeriod(fromDate, toDate);
        }
    }
}