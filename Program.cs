using PsSupportSystem.App.Application;
using PsSupportSystem.App.Domain;
using PsSupportSystem.App.Infrastructure;
using System.Linq;



namespace PsSupportSystem.App;

public class Program
{
    public static void Main()
    {
        // Create Data folder if it doesn't exist
        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);

        // Create repository pointing at Data/reports.json
        var reportRepoPath = Path.Combine(dataDir, "reports.json");
        var reportRepo = new ReportRepository(reportRepoPath);

        var riskEvaluator = new RiskEvaluator();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Personal Supervisor Support System (Prototype)");
            Console.WriteLine("==============================================");
            Console.WriteLine("1. Simulate student self-report");
            Console.WriteLine("2. List saved self-reports");
            Console.WriteLine("0. Exit");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            if (choice == "0")
                break;

            else if (choice == "2")
            {
                ListReports(reportRepo);
            }

            if (choice == "1")
            {
                RunSelfReportSimulation(riskEvaluator, reportRepo);

            }
        }
    }

    private static void ListReports(ReportRepository reportRepo)
    {
        Console.Clear();
        Console.WriteLine("Saved self-reports");
        Console.WriteLine("-------------------");

        var reports = reportRepo.GetAll().ToList();

        if (!reports.Any())
        {
            Console.WriteLine("No reports found.");
        }
        else
        {
            foreach (var r in reports)
            {
                Console.WriteLine($"Id: {r.Id}");
                Console.WriteLine($"  StudentId: {r.StudentId}");
                Console.WriteLine($"  SubmittedAt: {r.SubmittedAt}");
                Console.WriteLine($"  Wellbeing: {r.WellbeingRating}");
                Console.WriteLine($"  Workload: {r.WorkloadRating}");
                Console.WriteLine($"  Progress: {r.ProgressStatus}");
                Console.WriteLine($"  Status: {r.DerivedStatus}");
                Console.WriteLine($"  Comment: {r.Comment}");
                Console.WriteLine();
            }
        }

        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }


    private static void RunSelfReportSimulation(RiskEvaluator riskEvaluator, ReportRepository reportRepo)
    {
        Console.Clear();
        Console.WriteLine("Self-report simulation");
        Console.WriteLine("----------------------");

        int wellbeing = ReadInt("Wellbeing (1 = very bad, 5 = very good): ");
        int workload = ReadInt("Workload (1 = very low, 5 = too high): ");

        Console.Write("Progress (on track / behind): ");
        string progress = Console.ReadLine() ?? "";

        Console.Write("Optional comment: ");
        string comment = Console.ReadLine() ?? "";

        var report = new SelfReport
        {
            Id = 1,
            StudentId = 1,
            SubmittedAt = DateTime.Now,
            WellbeingRating = wellbeing,
            WorkloadRating = workload,
            ProgressStatus = progress,
            Comment = comment
        };

        report.DerivedStatus = riskEvaluator.Evaluate(report);
        reportRepo.SaveReport(report);


        Console.WriteLine();
        Console.WriteLine($"Calculated engagement status: {report.DerivedStatus}");
        Console.WriteLine("Report saved to JSON.");
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();

    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (int.TryParse(input, out int value))
                return value;

            Console.WriteLine("Please enter a valid number.");
        }
    }
}
