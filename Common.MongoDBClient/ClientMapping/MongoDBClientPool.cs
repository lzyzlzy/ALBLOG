namespace Common.MongoDBClient.ClientMapping
{
    #region using directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;

    #endregion using directives

    public class MongoDBClientPool
    {
        // MongoDB Client Instance Pool
        private static readonly Dictionary<Guid, MongoClient> mongoClientInstances;

        private static readonly object locker = new object();

        static MongoDBClientPool()
        {
            mongoClientInstances = new Dictionary<Guid, MongoClient>();
        }

        public static void CleanMongoDBClientPool()
        {
            mongoClientInstances.Clear();
        }

        public static MongoClient GetDefaultMongoDBClient()
        {
            return mongoClientInstances.FirstOrDefault().Value;
        }

        public static MongoClient GetMongoDBClient(Guid instanceId)
        {
            MongoClient client;
            mongoClientInstances.TryGetValue(instanceId, out client);
            return client;
        }

        public static void RegisterMongoDBClient(Guid instanceId, MongoClientSettings mongoClientSettings)
        {
            lock (locker)
            {
                if (mongoClientInstances.ContainsKey(instanceId))
                {
                    throw new Exception("The same name instance has been registered.");
                }
                var mongoClient = new MongoClient(mongoClientSettings);
                mongoClientInstances.Add(instanceId, mongoClient);
            }
        }

        public static void UnregisterMongoDBClient(Guid instanceId)
        {
            lock (locker)
            {
                if (mongoClientInstances.ContainsKey(instanceId) && !mongoClientInstances.Remove(instanceId))
                {
                    throw new Exception("Unregister MongoDB client instance failed.");
                }
            }
        }
    }
}