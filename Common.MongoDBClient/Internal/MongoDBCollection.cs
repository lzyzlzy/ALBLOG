﻿namespace Common.MongoDBClient.Internal
{
    #region using directives

    using System;
    using MongoDB.Driver;

    #endregion using directives

    internal class MongoDBCollection
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        public static IMongoCollection<T> GetCollection<T>(MongoClient client, string databaseName,
            string collectionName)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Mongo client object was not specified.");
            }
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName), "You must specify a valid MongoDB database name.");
            }
            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentNullException(nameof(collectionName), "You must specify a valid MongoDB collection name.");
            }
            var database = client.GetDatabase(databaseName);
            return database.GetCollection<T>(collectionName);
        }
    }
}