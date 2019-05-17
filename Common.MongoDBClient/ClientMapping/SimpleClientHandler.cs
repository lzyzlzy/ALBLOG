namespace Common.MongoDBClient.ClientMapping
{
    #region using directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using Intention;
    using MongoDB.Driver;

    #endregion using directives

    public class SimpleClientHandler : IClientHandler
    {
        protected static FileSystemWatcher FileWatcher;
        protected string ConfigFilePath;

        // MongoDB Client Status Table
        protected List<Guid> ReadableClientMap;

        protected List<Guid> WritableClientMap;

        private bool isEnableConfigFileWatcher;

        public SimpleClientHandler(string configFilePath, bool enableConfigFilerWatcher = false)
        {
            this.ReadableClientMap = new List<Guid>();
            this.WritableClientMap = new List<Guid>();
            this.ConfigFilePath = configFilePath;
            this.IsEnableConfigFileWatcher = enableConfigFilerWatcher;
            this.LoadClientMapFromXmlFile();
        }

        public bool IsEnableConfigFileWatcher
        {
            get { return this.isEnableConfigFileWatcher; }
            set
            {
                this.isEnableConfigFileWatcher = value;
                if (FileWatcher != null)
                {
                    FileWatcher.EnableRaisingEvents = value;
                }
            }
        }

        public virtual IClientHandler LoadClientMap()
        {
            if (String.IsNullOrEmpty(this.ConfigFilePath) || !File.Exists(this.ConfigFilePath))
            {
                throw new FileNotFoundException("You must specified the client map configuration file.",
                    this.ConfigFilePath);
            }
            if (this.IsEnableConfigFileWatcher && FileWatcher == null)
            {
                this.BindFileWatcher();
            }
            this.LoadClientMapFromXmlFile();
            return this;
        }

        public IClientHandler ReloadClientMap()
        {
            MongoDBClientPool.CleanMongoDBClientPool();
            this.ClearClientMap();
            return this.LoadClientMap();
        }

        public MongoClient GetDefaultClient()
        {
            return MongoDBClientPool.GetDefaultMongoDBClient();
        }

        protected void BindFileWatcher()
        {
            var configFileLocation = Path.GetDirectoryName(this.ConfigFilePath);
            var configFileName = Path.GetFileName(this.ConfigFilePath);
            FileWatcher = new FileSystemWatcher
            {
                Path = configFileLocation,
                EnableRaisingEvents = this.IsEnableConfigFileWatcher,
                IncludeSubdirectories = false,
                Filter = configFileName
                //Filter = "*.config"
            };
            FileWatcher.Changed += this.ConfigFileChanged;
        }

        protected void ConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            // http://stackoverflow.com/questions/19368710/filesystemwatcher-calls-handler-three-times
            // Common file system operations might raise more than one event.
            // For example, when a file is moved from one directory to another,
            // several OnChanged and some OnCreated and OnDeleted events might be raised.
            // Moving a file is a complex operation that consists of multiple simple operations,
            // therefore raising multiple events. Likewise, some applications
            // (for example, antivirus software) might cause additional file system events
            // that are detected by FileSystemWatcher.
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                this.ReloadClientMap();
            }
        }

        protected virtual void ClearClientMap()
        {
            this.ReadableClientMap.Clear();
            this.WritableClientMap.Clear();
        }

        private void LoadClientMapFromXmlFile()
        {
            var xmlDocument = XDocument.Parse(File.ReadAllText(this.ConfigFilePath));
            var rootNode = xmlDocument.Root;
            if (rootNode == null)
            {
                throw new XmlException("Load configuration file root node failed.");
            }
            if (rootNode.Name.LocalName.EqualsIgnoreCase("mongoDBConfiguration"))
            {
                foreach (var mongodbNode in rootNode.Elements())
                {
                    if (mongodbNode.Name.LocalName.EqualsIgnoreCase("mongoDB"))
                    {
                        foreach (var mongoClientNode in mongodbNode.Elements())
                        {
                            if (mongoClientNode.Name.LocalName.EqualsIgnoreCase("mongoClient"))
                            {
                                var idAttribute = mongoClientNode.Attribute("id");
                                var instanceId = Guid.NewGuid(); ;
                                if (idAttribute != null && !string.IsNullOrEmpty(idAttribute.Value))
                                {
                                    instanceId = new Guid(idAttribute.Value);
                                }

                                var connectionString = mongoClientNode.Attribute("connectionString").Value;
                                var mongoSettings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

                                MongoDBClientPool.RegisterMongoDBClient(instanceId, mongoSettings);
                                
                                var modeAttribute = mongoClientNode.Attribute("mode");
                                if (modeAttribute == null)
                                {
                                    // If not specify the MongoDB mode type it will NOT be considered as full mode.
                                    continue;
                                }
                                switch (modeAttribute.Value.ToLower())
                                {
                                    case "read":
                                        this.ReadableClientMap.Add(instanceId);
                                        break;

                                    case "write":
                                        this.WritableClientMap.Add(instanceId);
                                        break;

                                    case "full":
                                        this.ReadableClientMap.Add(instanceId);
                                        this.WritableClientMap.Add(instanceId);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}