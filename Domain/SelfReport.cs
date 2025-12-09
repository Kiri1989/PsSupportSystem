namespace PsSupportSystem.App.Domain;

public class SelfReport
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public DateTime SubmittedAt { get; set; }

    public int WellbeingRating { get; set; }   // 1 (very bad) to 5 (very good)
    public int WorkloadRating { get; set; }    // 1 (very low) to 5 (too high)

    public string ProgressStatus { get; set; } = ""; // e.g. "on track" or "behind"
    public string Comment { get; set; } = "";

    public EngagementStatus DerivedStatus { get; set; } = EngagementStatus.NoData;
}
