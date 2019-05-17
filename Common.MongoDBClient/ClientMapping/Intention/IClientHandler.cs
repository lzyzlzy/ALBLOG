namespace Common.MongoDBClient.ClientMapping.Intention
{
    #region using directives

    using MongoDB.Driver;

    #endregion using directives

    public interface IClientHandler
    {
        IClientHandler ReloadClientMap();

        IClientHandler LoadClientMap();

        MongoClient GetDefaultClient();
    }
}