using Application.Interfaces.Contexts;
using Domain.Visitors;
using MongoDB.Driver;

namespace Application.VisitorOnline
{
    public interface IVisitorOnlineService
    {
        void ConnectUser(string clientId);
        void DisconnectUser(string clientId);
        int GetCount();
    }

    public class VisitorOnlineService : IVisitorOnlineService
    {
        private readonly IMongoDbContext<OnlineVisitors> dbContext;
        private readonly IMongoCollection<OnlineVisitors> _mongoCollection;

        public VisitorOnlineService(IMongoDbContext<OnlineVisitors> dbContext)
        {
            this.dbContext = dbContext;
            _mongoCollection = dbContext.GetCollection();
        }

        public void ConnectUser(string clientId)
        {
            var exist = _mongoCollection.AsQueryable().FirstOrDefault(x => x.ClientId == clientId);
            if (exist != null)
            {
                _mongoCollection.InsertOne(new OnlineVisitors()
                {
                    Time = DateTime.Now,
                    ClientId = clientId
                });
            }
        }

        public void DisconnectUser(string clientId)
        {
            _mongoCollection.FindOneAndDelete(x => x.ClientId == clientId);
        }

        public int GetCount()
        {
           return  _mongoCollection.AsQueryable().Count();
           
        }
    }
}
