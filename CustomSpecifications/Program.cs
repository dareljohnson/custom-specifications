using CustomSpecifications.Examples.Simple;
using CustomSpecifications.Examples.WMS;

namespace CustomSpecifications;

/// <summary>
/// Demo program showcasing the CustomSpecifications library.
/// Demonstrates simple to advanced usage of the Specification Pattern.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("?????????????????????????????????????????????????????????????????");
        Console.WriteLine("?     CustomSpecifications Library - Demonstration Program     ?");
        Console.WriteLine("?                  Specification Pattern in C#                  ?");
        Console.WriteLine("?????????????????????????????????????????????????????????????????");
        Console.WriteLine();

        bool running = true;

        while (running)
        {
            DisplayMenu();
            var choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    RunSimpleExamples();
                    break;
                case "2":
                    RunAdvancedWMSExamples();
                    break;
                case "3":
                    RunSpecificSimpleExample();
                    break;
                case "4":
                    RunSpecificWMSExample();
                    break;
                case "5":
                    ShowPatternExplanation();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("\nThank you for exploring CustomSpecifications!");
                    break;
                default:
                    Console.WriteLine("\n? Invalid choice. Please try again.\n");
                    break;
            }

            if (running && choice != "5")
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("???????????????????????????????????????????????????????????????");
        Console.WriteLine("                         MAIN MENU");
        Console.WriteLine("???????????????????????????????????????????????????????????????");
        Console.WriteLine();
        Console.WriteLine("  [1] Run All Simple Examples");
        Console.WriteLine("      ? User validation, email checks, ranges, passwords");
        Console.WriteLine();
        Console.WriteLine("  [2] Run All Advanced WMS Examples");
        Console.WriteLine("      ? 3PL Warehouse Management System scenarios");
        Console.WriteLine();
        Console.WriteLine("  [3] Run Specific Simple Example");
        Console.WriteLine("      ? Choose individual simple examples");
        Console.WriteLine();
        Console.WriteLine("  [4] Run Specific WMS Example");
        Console.WriteLine("      ? Choose individual warehouse examples");
        Console.WriteLine();
        Console.WriteLine("  [5] Specification Pattern Explanation");
        Console.WriteLine("      ? Learn about the pattern");
        Console.WriteLine();
        Console.WriteLine("  [0] Exit");
        Console.WriteLine();
        Console.WriteLine("???????????????????????????????????????????????????????????????");
        Console.Write("\nEnter your choice: ");
    }

    private static void RunSimpleExamples()
    {
        Console.WriteLine("?????????????????????????????????????????????????????????????????");
        Console.WriteLine("?                      SIMPLE EXAMPLES                          ?");
        Console.WriteLine("?????????????????????????????????????????????????????????????????");
        Console.WriteLine();

        SimpleExamples.RunAll();
    }

    private static void RunAdvancedWMSExamples()
    {
        Console.WriteLine("?????????????????????????????????????????????????????????????????");
        Console.WriteLine("?          ADVANCED WAREHOUSE MANAGEMENT EXAMPLES               ?");
        Console.WriteLine("?              3PL Logistics for Tire Rack,                     ?");
        Console.WriteLine("?              Fenty Beauty, and Newegg                         ?");
        Console.WriteLine("?????????????????????????????????????????????????????????????????");
        Console.WriteLine();

        AdvancedWMSExamples.RunAll();
    }

    private static void RunSpecificSimpleExample()
    {
        Console.WriteLine("???????????????????????????????????????????????????????????????");
        Console.WriteLine("                   SIMPLE EXAMPLES MENU");
        Console.WriteLine("???????????????????????????????????????????????????????????????");
        Console.WriteLine();
        Console.WriteLine("  [1] User Validation (age and status)");
        Console.WriteLine("  [2] Email Validation (multiple rules)");
        Console.WriteLine("  [3] Number Range Validation");
        Console.WriteLine("  [4] String Content Validation (passwords)");
        Console.WriteLine("  [5] NOT Operator Examples");
        Console.WriteLine("  [0] Back to Main Menu");
        Console.WriteLine();
        Console.Write("Enter your choice: ");

        var choice = Console.ReadLine();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                SimpleExamples.Example1_SimpleUserValidation();
                break;
            case "2":
                SimpleExamples.Example2_EmailValidation();
                break;
            case "3":
                SimpleExamples.Example3_NumberRanges();
                break;
            case "4":
                SimpleExamples.Example4_StringContent();
                break;
            case "5":
                SimpleExamples.Example5_NotOperator();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("? Invalid choice.\n");
                break;
        }
    }

    private static void RunSpecificWMSExample()
    {
        Console.WriteLine("???????????????????????????????????????????????????????????????");
        Console.WriteLine("           WAREHOUSE MANAGEMENT EXAMPLES MENU");
        Console.WriteLine("???????????????????????????????????????????????????????????????");
        Console.WriteLine();
        Console.WriteLine("  [1] Low Stock Alerts for Premium Clients");
        Console.WriteLine("  [2] Expedited Order Processing");
        Console.WriteLine("  [3] Special Handling Location Assignment");
        Console.WriteLine("  [4] Expiring Inventory Management");
        Console.WriteLine("  [5] Order Batching Logic");
        Console.WriteLine("  [6] SLA Compliance Monitoring");
        Console.WriteLine("  [7] Cycle Count Prioritization");
        Console.WriteLine("  [8] International Shipment Compliance");
        Console.WriteLine("  [0] Back to Main Menu");
        Console.WriteLine();
        Console.Write("Enter your choice: ");

        var choice = Console.ReadLine();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                AdvancedWMSExamples.Example1_LowStockPremiumClients();
                break;
            case "2":
                AdvancedWMSExamples.Example2_ExpeditedOrderProcessing();
                break;
            case "3":
                AdvancedWMSExamples.Example3_SpecialHandlingLocationAssignment();
                break;
            case "4":
                AdvancedWMSExamples.Example4_ExpiringInventoryManagement();
                break;
            case "5":
                AdvancedWMSExamples.Example5_OrderBatchingLogic();
                break;
            case "6":
                AdvancedWMSExamples.Example6_SLAComplianceMonitoring();
                break;
            case "7":
                AdvancedWMSExamples.Example7_CycleCountPrioritization();
                break;
            case "8":
                AdvancedWMSExamples.Example8_InternationalShipmentCompliance();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("? Invalid choice.\n");
                break;
        }
    }

    private static void ShowPatternExplanation()
    {
        Console.WriteLine("?????????????????????????????????????????????????????????????????");
        Console.WriteLine("?              THE SPECIFICATION PATTERN                        ?");
        Console.WriteLine("?????????????????????????????????????????????????????????????????");
        Console.WriteLine();
        Console.WriteLine("WHAT IS IT?");
        Console.WriteLine("???????????");
        Console.WriteLine("The Specification Pattern is a software design pattern that");
        Console.WriteLine("encapsulates business rules that can be recombined by chaining");
        Console.WriteLine("them together using Boolean logic (AND, OR, NOT).");
        Console.WriteLine();
        Console.WriteLine("KEY BENEFITS:");
        Console.WriteLine("?????????????");
        Console.WriteLine("? Reusable: Define rules once, use everywhere");
        Console.WriteLine("? Composable: Combine simple rules into complex ones");
        Console.WriteLine("? Testable: Each rule can be unit tested independently");
        Console.WriteLine("? Readable: Business logic reads like natural language");
        Console.WriteLine("? Maintainable: Changes to rules are isolated");
        Console.WriteLine();
        Console.WriteLine("BASIC STRUCTURE:");
        Console.WriteLine("????????????????");
        Console.WriteLine("  ISpecification<T>");
        Console.WriteLine("  ??? IsSatisfiedBy(T candidate) ? bool");
        Console.WriteLine("  ??? And(ISpecification<T> other)");
        Console.WriteLine("  ??? Or(ISpecification<T> other)");
        Console.WriteLine("  ??? Not()");
        Console.WriteLine("  ??? AndNot(ISpecification<T> other)");
        Console.WriteLine("  ??? OrNot(ISpecification<T> other)");
        Console.WriteLine();
        Console.WriteLine("SIMPLE EXAMPLE:");
        Console.WriteLine("???????????????");
        Console.WriteLine("  // Define specifications");
        Console.WriteLine("  var isAdult = new IsAdultSpecification();");
        Console.WriteLine("  var isActive = new IsActiveSpecification();");
        Console.WriteLine();
        Console.WriteLine("  // Combine them");
        Console.WriteLine("  var eligibleUsers = users.Where(isAdult.And(isActive));");
        Console.WriteLine();
        Console.WriteLine("COMPLEX EXAMPLE:");
        Console.WriteLine("????????????????");
        Console.WriteLine("  // Business Rule: Send to collection if:");
        Console.WriteLine("  // - Invoice is overdue AND");
        Console.WriteLine("  // - Notice has been sent AND");
        Console.WriteLine("  // - NOT already in collection");
        Console.WriteLine();
        Console.WriteLine("  var overdue = new OverdueSpecification();");
        Console.WriteLine("  var noticeSent = new NoticeSentSpecification();");
        Console.WriteLine("  var inCollection = new InCollectionSpecification();");
        Console.WriteLine();
        Console.WriteLine("  var sendToCollection = overdue");
        Console.WriteLine("      .And(noticeSent)");
        Console.WriteLine("      .And(inCollection.Not());");
        Console.WriteLine();
        Console.WriteLine("  foreach (var invoice in invoices)");
        Console.WriteLine("  {");
        Console.WriteLine("      if (sendToCollection.IsSatisfiedBy(invoice))");
        Console.WriteLine("          invoice.SendToCollection();");
        Console.WriteLine("  }");
        Console.WriteLine();
        Console.WriteLine("USE CASES:");
        Console.WriteLine("??????????");
        Console.WriteLine("• Filtering collections based on business rules");
        Console.WriteLine("• Validating objects against complex criteria");
        Console.WriteLine("• Building dynamic queries for databases");
        Console.WriteLine("• Implementing authorization rules");
        Console.WriteLine("• Creating business rule engines");
        Console.WriteLine();
        Console.WriteLine("WHEN TO USE:");
        Console.WriteLine("????????????");
        Console.WriteLine("? Multiple business rules need to be combined");
        Console.WriteLine("? Rules change frequently or independently");
        Console.WriteLine("? Same rules apply across different contexts");
        Console.WriteLine("? Complex validation logic needs to be testable");
        Console.WriteLine();
        Console.WriteLine("REFERENCES:");
        Console.WriteLine("???????????");
        Console.WriteLine("• Domain-Driven Design by Eric Evans");
        Console.WriteLine("• Martin Fowler's Specifications paper");
        Console.WriteLine("• Wikipedia: Specification Pattern");
        Console.WriteLine();
        Console.WriteLine("???????????????????????????????????????????????????????????????");
        Console.WriteLine();
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
        Console.Clear();
    }
}
