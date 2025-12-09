using PsSupportSystem.App.Domain;

namespace PsSupportSystem.App.Application;

public class RiskEvaluator
{    
    public EngagementStatus Evaluate(SelfReport report)
    {
        // "AtRisk" if wellbeing very low OR progress is "behind"
        if (report.WellbeingRating <= 2 ||
            report.ProgressStatus.Equals("behind", StringComparison.OrdinalIgnoreCase))
        {
            return EngagementStatus.AtRisk;
        }

        // "Concern" if workload is high or wellbeing is mediocre
        if (report.WorkloadRating >= 4 || report.WellbeingRating == 3)
        {
            return EngagementStatus.Concern;
        }

        // otherwise OK
        return EngagementStatus.OK;
    }
}
