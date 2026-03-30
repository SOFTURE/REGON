namespace REGON.Client.Models;

public class AdvanceReportData
{
    public string Krs { get; init; }
    public bool IsSuspended { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public string WebsiteUrl { get; init; }
}