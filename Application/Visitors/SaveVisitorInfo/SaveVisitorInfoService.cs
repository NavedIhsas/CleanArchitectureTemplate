using Application.Interfaces.Contexts;
using Domain.Visitors;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;

namespace Application.Visitors.SaveVisitorInfo;

public class SaveVisitorInfoService : ISaveVisitorInfoService
{
    private readonly IMongoDbContext<Visitor> _dbContext;
    private readonly IMongoCollection<Visitor> _mongoCollection;

    public SaveVisitorInfoService(IMongoDbContext<Visitor> dbContext)
    {
        _dbContext = dbContext;
        _mongoCollection = _dbContext.GetCollection();
    }

  
    public void Execute(RequestSaveVisitorInfoDto request)
    {
       _mongoCollection.InsertOne(new Visitor()
        {
            Browser = new VisitorVersion()
            {
                Family = request.Browser.Family,
                Version = request.Browser.Version,
            },
            CurrentLink = request.CurrentLink,
            Device = new Device()
            {
                Brand = request.Device.Brand,
                Family = request.Device.Family,
                IsSpider = request.Device.IsSpider,
                Model = request.Device.Model,
            },
            Ip = request.Ip,
            Method = request.Method,
            OperationSystem = new VisitorVersion()
            {
                Family = request.OperationSystem.Family,
                Version = request.OperationSystem.Version,
            },
            PhysicalPath = request.PhysicalPath,
            Protocol = request.Protocol,
            ReferrerLink = request.ReferrerLink,
           VisitorId = request.VisitorId,
           Time = request.Time,
       });
    }
}