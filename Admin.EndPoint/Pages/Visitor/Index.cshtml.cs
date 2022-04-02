using Application.Visitors.GetTodayReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.EndPoint.Pages.Visitor
{
    public class IndexModel : PageModel
    {
        private readonly IGetTodayReport _todayReport;
        public ResultToDayReportDto ResultTodayReport;
        public List<VisitorsDto> Visitors;
        public IndexModel(IGetTodayReport todayReport)
        {
            _todayReport = todayReport;
        }

        public void OnGet()
        {
            ResultTodayReport = _todayReport.Execute();
        }
    }
}
