using Application.Interfaces.Contexts;
using MongoDB.Driver;

namespace persistent.Context.MongoContext
{
    public class MongoDbContext<T>:IMongoDbContext<T>
    {
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<T> collection;

        public MongoDbContext()
        {
            var client=new MongoClient();
            database = client.GetDatabase("visitorDb");
            collection = database.GetCollection<T>(typeof(T).Name);
        }
        public IMongoCollection<T> GetCollection()
        {
            return collection;
        }
    }
}
