namespace Common.MongoDBClient
{
    #region using directives

    using ClientMapping.Intention;
    using Internal;
    using Model;
    using MongoDB.Driver;
    using System;

    #endregion using directives

    public class MultiDataCollectionSelector
    {
        protected readonly IMultiClientHandler ClientMappingHandler;

        public MultiDataCollectionSelector(IMultiClientHandler clientMappingHandler)
        {
            this.ClientMappingHandler = clientMappingHandler;
        }

        public virtual IMongoCollection<T> GetReadableDataCollection<T>(string databaseName)
        {
            var readableClient = this.ClientMappingHandler.GetPriorityReadableClient();
            var collectionName = typeof(T).GetAttributeValue<CollectionAttribute, string>(t => t.Name);
            return MongoDBCollection.GetCollection<T>(readableClient, databaseName, collectionName);
        }

        public virtual IMongoCollection<T> GetWritableDataCollection<T>(string databaseName)
        {
            var writableClient = this.ClientMappingHandler.GetPriorityWritableClient();
            var collectionName = typeof(T).GetAttributeValue<CollectionAttribute, string>(t => t.Name);
            return MongoDBCollection.GetCollection<T>(writableClient, databaseName, collectionName);
        }

        public virtual IMongoCollection<T> GetReadableDataCollection<T>(string databaseName, string collectionName)
        {
            var readableClient = this.ClientMappingHandler.GetPriorityReadableClient();
            return MongoDBCollection.GetCollection<T>(readableClient, databaseName, collectionName);
        }

        public virtual IMongoCollection<T> GetWritableDataCollection<T>(string databaseName, string collectionName)
        {
            var writableClient = this.ClientMappingHandler.GetPriorityWritableClient();
            return MongoDBCollection.GetCollection<T>(writableClient, databaseName, collectionName);
        }

        public virtual IMongoCollection<T> GetDataCollection<T>(MongoClient client, string databaseName,
            string collectionName)
        {
            return MongoDBCollection.GetCollection<T>(client, databaseName, collectionName);
        }
    }
}