namespace Application.Visitors.GetTodayReport;

public class ToDayDto
{
    public long PageViews { get; set; }
    public long Visitors { get; set; }
    public float PageViewsPerVisit { get; set; }
    public VisitCountDto Visitor { get; set; }
}