using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Web;

namespace ALBLOG.Constant
{
    public class GlobalConfig
    {
        // Assign MongoDB Database name
        public static readonly string MongoDbDatabaseName = "ALBLOG";

        public static readonly string MongoDbAuditDatabaseName = "AveAdministrationAudit";
        public static readonly string MongoDbCognitiveDatabaseName = "AtheneCognitive";
        public static readonly string MongoDbCognitiveAuditDatabaseName = "AtheneCognitiveAudit";
        public static readonly string MongoDbConfigFilePath = string.Empty;

        public static readonly string RedisHost = "localhost";
        public static readonly string RedisRequestMessageQueueName = "RequestMessage";
        public static readonly string RedisResponseMessageDictionaryName = "ResponseMessage";
        public static readonly int CognitiveGetResponseRetryCount = 6;

        public static readonly List<string> ImgExtensions = new List<string> { "bmp", "jpg", "png", "gif", "PNG", "JPG", "GIF", "BMP" };
        static GlobalConfig()
        {
            MongoDbConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Config/mongodb.config";
        }


        public static string GetInstallPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (path.EndsWith(@"bin\", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(0, path.Length - 4);
            }
            if (path.EndsWith("\\"))
            {
                path = path.TrimEnd('\\');
            }
            return path;
        }
    }

}
