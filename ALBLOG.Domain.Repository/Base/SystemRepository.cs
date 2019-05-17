namespace ALBLOG.Domain.Repository
{
    #region using directives

    using System;
    using System.Collections.Generic;
    using Common.MongoDBClient;
    using Common.MongoDBClient.ClientMapping;
    using Constant;

    #endregion using directives 

    public class SystemRepository
    {
        private readonly DataCollectionSelector dataSelector;
        private readonly string databaseName;

        public SystemRepository()
        {
            this.databaseName = GlobalConfig.MongoDbAuditDatabaseName;

            var simpleClientMappingHandler = new SimpleClientHandler(
                GlobalConfig.MongoDbConfigFilePath,
                false
                ).LoadClientMap();
            this.dataSelector = new DataCollectionSelector(simpleClientMappingHandler);
        }

        public IEnumerable<string> GetDataCollectionList(string targetDatabaseName = null)
        {
            if (targetDatabaseName.IsNullOrEmpty())
            {
                targetDatabaseName = this.databaseName;
            }
            return this.dataSelector.GetDataCollectionList(targetDatabaseName);
        }
    }
}