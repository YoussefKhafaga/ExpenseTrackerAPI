public class ExpenseQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string Filter { get; set; } // "pastweek", "pastmonth", "last3months", "custom"
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
