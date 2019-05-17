namespace Common.MongoDBClient.ClientMapping
{
    #region using directives

    using Intention;
    using MongoDB.Driver;
    using System.Linq;

    #endregion using directives

    public class SimpleMultiClientHandler : SimpleClientHandler, IMultiClientHandler
    {
        public SimpleMultiClientHandler(string configFilePath, bool enableConfigFilerWatcher = false)
            : base(configFilePath, enableConfigFilerWatcher)
        {
            this.ConfigFilePath = configFilePath;
            this.IsEnableConfigFileWatcher = enableConfigFilerWatcher;
        }

        public virtual MongoClient GetPriorityReadableClient()
        {
            this.SyncClientStatus();
            if (!this.ReadableClientMap.Any())
            {
                throw new MongoException("There without any available MongoDB client for read.");
            }
            var readableClientId = this.ReadableClientMap.First();
            return MongoDBClientPool.GetMongoDBClient(readableClientId);
        }

        public virtual MongoClient GetPriorityWritableClient()
        {
            this.SyncClientStatus();
            if (!this.WritableClientMap.Any())
            {
                throw new MongoException("There without any available MongoDB client for write.");
            }
            var writableClientId = this.WritableClientMap.First();
            return MongoDBClientPool.GetMongoDBClient(writableClientId);
        }

        public virtual void SyncClientStatus()
        {
            // Disrupt the order
            this.ReadableClientMap = this.ReadableClientMap.Shuffle().ToList();
            this.WritableClientMap = this.WritableClientMap.Shuffle().ToList();
        }

        public new virtual IMultiClientHandler LoadClientMap()
        {
            return base.LoadClientMap() as IMultiClientHandler;
        }

        public new virtual IMultiClientHandler ReloadClientMap()
        {
            return base.ReloadClientMap() as IMultiClientHandler;
        }
    }
}