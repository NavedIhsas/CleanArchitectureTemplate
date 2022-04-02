using Application.Visitors.SaveVisitorInfo;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UAParser;

namespace Web.Endpoint.Utilities.Filters
{
    public class SaveVisitorFilter : IPageFilter
    {
        private readonly ISaveVisitorInfoService _saveVisitorInfoService;
        public SaveVisitorFilter(ISaveVisitorInfoService saveVisitorInfoService)
        {
            _saveVisitorInfoService = saveVisitorInfoService;
        }


        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
           
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var ip = context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var actionName = context.ActionDescriptor.DisplayName;
            var pageName = context.ActionDescriptor.ModelTypeInfo?.Name;
            var userAgent = context.HttpContext.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);
            var referer = context.HttpContext.Request.Headers["Referer"].ToString();
            var currentUrl = context.HttpContext.Request.Path;
            var request = context.HttpContext.Request;
            var visitorId = context.HttpContext.Request.Cookies["VisitorId"];
            
            _saveVisitorInfoService.Execute(new RequestSaveVisitorInfoDto
            {
                Browser = new VisitorVersionDto
                {
                    Family = clientInfo.UA.Family,
                    Version = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}.{clientInfo.UA.Patch}"
                },
                CurrentLink = currentUrl,
                Device = new DeviceDto
                {
                    Brand = clientInfo.Device.Brand,
                    Family = clientInfo.Device.Family,
                    IsSpider = clientInfo.Device.IsSpider,
                    Model = clientInfo.Device.Model,
                },
                Ip = ip,
                Method = request.Method,
                OperationSystem = new VisitorVersionDto()
                {
                    Family = clientInfo.OS.Family,
                    Version = $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}.{clientInfo.OS.Patch}.{clientInfo.OS.PatchMinor}"
                },
                PhysicalPath = $"{pageName}/{actionName}",
                Protocol = request.Protocol,
                ReferrerLink = referer,
                VisitorId = visitorId,
                Time = DateTime.Now,
            });
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            
        }
    }
}
