namespace Common.MongoDBClient
{
    #region using directives

    using ClientMapping.Intention;
    using Internal;
    using Model;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion using directives

    public class DataCollectionSelector
    {
        protected readonly IClientHandler ClientHandler;

        public DataCollectionSelector(IClientHandler clientMappingHandler)
        {
            this.ClientHandler = clientMappingHandler;
        }

        public static IEnumerable<BsonDocument> Enumerate(IAsyncCursor<BsonDocument> docs)
        {
            while (docs.MoveNext())
            {
                foreach (var item in docs.Current)
                {
                    yield return item;
                }
            }
        }

        public virtual IEnumerable<string> GetDataCollectionList(string databaseName)
        {
            var client = this.ClientHandler.GetDefaultClient();
            var collections =
                Enumerate(client.GetDatabase(databaseName).ListCollections()).Select(
                    c => c.GetValue("name").AsString);
            return collections;
        }

        public virtual IMongoCollection<T> GetDataCollection<T>(string databaseName)
        {
            var client = this.ClientHandler.GetDefaultClient();
            var collectionName = typeof(T).GetAttributeValue<CollectionAttribute, string>(t => t.Name);
            if (collectionName == null)
            {
                throw new Exception("You must specified the [Collection] attribute for the domain model.");
            }
            return MongoDBCollection.GetCollection<T>(client, databaseName, collectionName);
        }

        public virtual IMongoCollection<T> GetDataCollection<T>(string databaseName, string collectionName)
        {
            var client = this.ClientHandler.GetDefaultClient();
            return MongoDBCollection.GetCollection<T>(client, databaseName, collectionName);
        }

        public virtual IMongoCollection<T> GetDataCollection<T>(MongoClient client, string databaseName,
            string collectionName)
        {
            return MongoDBCollection.GetCollection<T>(client, databaseName, collectionName);
        }
    }
}