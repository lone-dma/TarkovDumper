using Spectre.Console;
using System.Diagnostics;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Text.Json;
using TarkovDumper;
using TarkovDumper.Processors;
using TarkovDumper.UI;

[assembly: AssemblyVersion("2.0.1.0")]
[assembly: AssemblyTitle(Program.Name)]
[assembly: AssemblyProduct(Program.Name)]
[assembly: AssemblyCopyright("©2025 Lone")]
[assembly: System.Runtime.Versioning.SupportedOSPlatform("Windows")]

namespace TarkovDumper
{
    class Program
    {
        internal const string Name = "Tarkov Dumper";
        private static readonly DirectoryInfo _configFolder = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TarkovDumper"));
        private static readonly FileInfo _configFile = new(Path.Combine(_configFolder.FullName, "dumper_config.json"));

        /// <summary>
        /// Dumper Configuration File.
        /// </summary>
        internal static DumperConfig Config { get; }

        static Program()
        {
            Console.OutputEncoding = Encoding.Unicode;
            _configFolder.Create(); // Create config folder if it doesn't exist
            if (!_configFile.Exists)
            {
                Config = new();
                File.WriteAllText(_configFile.FullName, JsonSerializer.Serialize(Config, new JsonSerializerOptions { WriteIndented = true }));
            }
            else
            {
                var configText = File.ReadAllText(_configFile.FullName);
                Config = JsonSerializer.Deserialize<DumperConfig>(configText) ?? new();
            }
        }

        static void Main()
        {
            string version = Assembly.GetExecutingAssembly()!.GetName()!.Version!.ToString();
            while (true) // Prompt loop
            {
                AnsiConsole.Clear();
                RenderHeader(version);
                var selection = ShowMenu();

                switch (selection)
                {
                    case MenuSelection.EFT:
                        ExecuteProcessor<EftProcessor>();
                        break;
                    case MenuSelection.Arena:
                        ExecuteProcessor<ArenaProcessor>();
                        break;
                    case MenuSelection.Both:
                        ExecuteProcessor<EftProcessor>();
                        ExecuteProcessor<ArenaProcessor>();
                        break;
                    case MenuSelection.Exit:
                        return;
                }
            }
        }
        private static void RenderHeader(string version)
        {
            var title = new FigletText(Program.Name)
                .Centered()
                .Color(Color.Aqua);
            AnsiConsole.Write(title);

            AnsiConsole.MarkupLine($"[gray]Version {version}[/]");
            AnsiConsole.MarkupLine($"[gray]https://lone-dma.org/[/]");
            AnsiConsole.Write(new Rule().RuleStyle("grey").Centered());
            AnsiConsole.WriteLine();
        }

        private static MenuSelection ShowMenu()
        {
            AnsiConsole.MarkupLine("[bold]Select job to run:[/]");
            var prompt = new SelectionPrompt<MenuSelection>()
                .Title("Choose a [green]task[/]:")
                .PageSize(Enum.GetValues<MenuSelection>().Length)
                .AddChoices(Enum.GetValues<MenuSelection>());
            return AnsiConsole.Prompt(prompt);
        }

        private static void ExecuteProcessor<T>()
            where T : AbstractProcessor
        {
            AnsiConsole.Status().Start($"Starting {typeof(T)}...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
                ctx.SpinnerStyle(Style.Parse("green"));

                AbstractProcessor processor = null;
                ToggleHighPerformance();
                try
                {
                    processor = Activator.CreateInstance<T>();
                    processor.Run(ctx);
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine();

                    AnsiConsole.MarkupLine($"[bold yellow]Exception thrown while processing step -> {processor?.LastStepName ?? "Unknown"}[/]");
                    AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                    if (ex.StackTrace != null)
                    {
                        AnsiConsole.MarkupLine("[bold yellow]==========================Begin Stack Trace==========================[/]");
                        AnsiConsole.WriteLine(ex.StackTrace);
                        AnsiConsole.MarkupLine("[bold yellow]===========================End Stack Trace===========================[/]");
                    }
                }
                finally
                {
                    ToggleHighPerformance();
                    ctx.Status("Press any key to continue...");
                    Console.ReadKey(true);
                }
            });
        }

        private static void ToggleHighPerformance()
        {
            var currentPriority = Thread.CurrentThread.Priority;
            if (currentPriority == ThreadPriority.Highest) // Job Finished -> Reset to normal
            {
                GCSettings.LatencyMode = GCLatencyMode.Interactive;
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                Thread.CurrentThread.Priority = ThreadPriority.Normal;
                // Force a full garbage collection and compact the LOH
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect(
                    generation: GC.MaxGeneration,
                    mode: GCCollectionMode.Aggressive,
                    blocking: true,
                    compacting: true);
            }
            else // Job Starting -> Set high performance
            {
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
            }
        }
    }
}
