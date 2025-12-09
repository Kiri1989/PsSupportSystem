using System.Collections.Generic;
using System.Linq;
using PsSupportSystem.App.Domain;

namespace PsSupportSystem.App.Infrastructure;

public class ReportRepository : JsonFileRepositoryBase<SelfReport>
{
    public ReportRepository(string filePath) : base(filePath)
    {
    }

    public IEnumerable<SelfReport> GetAll() => Items;

    public void SaveReport(SelfReport report)
    {
        if (report.Id == 0)
        {
            // Simple ID generation
            report.Id = Items.Any() ? Items.Max(r => r.Id) + 1 : 1;
            Items.Add(report);
        }
        else
        {
            var existing = Items.FirstOrDefault(r => r.Id == report.Id);
            if (existing != null)
            {
                var index = Items.IndexOf(existing);
                Items[index] = report;
            }
            else
            {
                Items.Add(report);
            }
        }

        Save();
    }
}
