namespace Common.MongoDBClient.Model
{
    #region using directives

    using System;

    #endregion using directives

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CollectionAttribute : Attribute
    {
        public CollectionAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}