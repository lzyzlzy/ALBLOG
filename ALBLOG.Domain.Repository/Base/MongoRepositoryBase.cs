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
using System.Threading.Tasks;

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

        public async Task<long> GetCountAsync()
        {
            return await this.GetCollection().CountAsync(i => true);
        }

        public async Task<long> GetCountAsync(Expression<Func<T, bool>> filter)
        {
            return await this.GetCollection().CountAsync(filter);
        }

        public async Task<IEnumerable<T>> GetManyByPage(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sortExpression, int offset, int limit)
        {
            return await Task.Run<IEnumerable<T>>(() =>
         {
             return this.GetCollection()
                              .Find(filter)
                              .SortByDescending(sortExpression)
                              .Skip(offset)
                              .Limit(limit)
                              .ToList();
         });
        }

        public IEnumerable<T> GetAll()
        {
            return this.GetCollection().Find(_ => true).ToList();
        }


        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            return this.GetCollection().Find(filter).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var data = await this.GetCollection().FindAsync(_ => true);
            return await data.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            var data = await this.GetCollection().FindAsync(filter);
            return await data.ToListAsync();
        }

        public T GetOne(Expression<Func<T, bool>> filter)
        {
            return this.GetCollection().Find(filter).FirstOrDefault();
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> filter)
        {
            return await this.GetCollection().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetOneAndUpdateAsync(Expression<Func<T, bool>> filter, Func<T, T> UpdateMethod)
        {
            var collection = this.GetCollection();
            var element = await (await collection.FindAsync(filter)).FirstOrDefaultAsync();
            var replacedElement = UpdateMethod(element);
            await collection.ReplaceOneAsync(filter, replacedElement);
            return replacedElement;
        }

        public void AddMany(IEnumerable<T> models)
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

        public async Task AddManyAsnyc(IEnumerable<T> models)
        {
            foreach (var model in models)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = ObjectId.GenerateNewId().ToString();
                }
            }
            await this.GetCollection().InsertManyAsync(models);
        }

        public void Add(T model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = ObjectId.GenerateNewId().ToString();
            }
            this.GetCollection().InsertOne(model);
        }

        public async Task AddAsync(T model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = ObjectId.GenerateNewId().ToString();
            }
            await this.GetCollection().InsertOneAsync(model);
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

        public async Task UpdateAsync(T model)
        {
            await this.GetCollection().ReplaceOneAsync(doc => doc.Id == model.Id, model);
        }

        public long DeleteOne(Expression<Func<T, bool>> filter)
        {
            return this.GetCollection().DeleteOne(filter).DeletedCount;
        }

        public async Task<long> DeleteOneAsync(Expression<Func<T, bool>> filter)
        {
            var result = await this.GetCollection().DeleteOneAsync(filter);
            return result.DeletedCount;
        }

        public long DeleteMany(Expression<Func<T, bool>> filter)
        {
            return this.GetCollection().DeleteMany(filter).DeletedCount;
        }

        public async Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            var result = await this.GetCollection().DeleteOneAsync(filter);
            return result.DeletedCount;
        }

        public void DropCollection(string collectionName)
        {
            this.GetCollection().Database.DropCollection(collectionName);
        }
    }
}