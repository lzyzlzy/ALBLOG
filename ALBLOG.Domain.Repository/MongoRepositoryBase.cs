#region using directives

using ALBLOG.Domain.Model;
using Common.MongoDBClient;
using Common.MongoDBClient.ClientMapping;
using ALBLOG.Constant;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion using directives
namespace ALBLOG.Domain.Repository
{

    public abstract class MongoRepositoryBase<T> where T : DomainModel
    {
        public const double EarthRadius = 6378.137; // KM
        private readonly DataCollectionSelector dataSelector;
        private readonly string databaseName;

        protected MongoRepositoryBase()
        {
            // 连接数据库
            this.databaseName = GlobalConfig.MongoDbDatabaseName;

            var simpleClientMappingHandler = new SimpleClientHandler(
                GlobalConfig.MongoDbConfigFilePath,
                false
                ).LoadClientMap();
            this.dataSelector = new DataCollectionSelector(simpleClientMappingHandler);
        }

        public IMongoCollection<T> GetCollection()
        {
            return this.dataSelector.GetDataCollection<T>(this.databaseName);
        }

        //redius unit: Meters
        public IEnumerable<T> GetNearBy(Location centerPoint, double redius /* Meter */)
        {
            // 获取表
            var collection = this.GetCollection();

            // 建立空间搜索 索引
            collection.Indexes.CreateOneAsync(Builders<T>.IndexKeys.Geo2DSphere(m => m.Location)).Wait();

            // 空间搜索算法
            var rangeInKm = redius / 1000; // KM
            var radians = rangeInKm / EarthRadius;

            var filter = Builders<T>.Filter.NearSphere(m => m.Location, centerPoint.Longitude, centerPoint.Latitude,
                radians);
            return collection.Find(filter).ToList();
        }

        public long GetCount()
        {
            return this.GetCollection().Count(_ => true);
        }

        public long GetCount(Expression<Func<T, bool>> filter)
        {
            return this.GetCollection().Count(filter);
        }

        public IEnumerable<T> GetAll()
        {
            return this.GetCollection().Find(_ => true).ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            return this.GetCollection().Find(filter).ToList();
        }

        public T GetOne(string id)
        {
            return this.GetCollection().Find(doc => doc.Id == id).FirstOrDefault();
        }

        public T GetOne(Expression<Func<T, bool>> filter)
        {
            return this.GetCollection().Find(filter).FirstOrDefault();
        }

        public void BulkAdd(IEnumerable<T> models)
        {
            foreach (var model in models)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = ObjectId.GenerateNewId().ToString();
                }
            }
            this.GetCollection().InsertMany(models);
        }

        public void Add(T model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = ObjectId.GenerateNewId().ToString();
            }
            this.GetCollection().InsertOne(model);
        }

        public void AddAsync(T model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = ObjectId.GenerateNewId().ToString();
            }
            this.GetCollection().InsertOneAsync(model);
        }

        public long Update(T model)
        {
            var executeResult = this.GetCollection().ReplaceOne(doc => doc.Id == model.Id, model);
            if (executeResult.IsModifiedCountAvailable)
            {
                return executeResult.ModifiedCount;
            }
            return -1;
        }

        public void UpdateAsync(T model)
        {
            this.GetCollection().ReplaceOneAsync(doc => doc.Id == model.Id, model);
        }

        public long DeleteOne(string id)
        {
            return this.GetCollection().DeleteOne(doc => doc.Id == id).DeletedCount;
        }

        public long DeleteMany(Expression<Func<T, bool>> filter)
        {
            return this.GetCollection().DeleteMany(filter).DeletedCount;
        }

        public void DeleteOneAsync(string id)
        {
            this.GetCollection().DeleteOneAsync(doc => doc.Id == id);
        }

        public void DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            this.GetCollection().DeleteOneAsync(filter);
        }

        public void DropCollection(string collectionName)
        {
            this.GetCollection().Database.DropCollection(collectionName);
        }
    }
}