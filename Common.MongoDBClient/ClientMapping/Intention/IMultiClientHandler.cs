namespace Common.MongoDBClient.ClientMapping.Intention
{
    #region using directives

    using MongoDB.Driver;

    #endregion using directives

    public interface IMultiClientHandler : IClientHandler
    {
        void SyncClientStatus();

        MongoClient GetPriorityReadableClient();

        MongoClient GetPriorityWritableClient();
    }
}