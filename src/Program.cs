using Spectre.Console;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using TarkovDumper.Processors;
using TarkovDumper.UI;

namespace TarkovDumper
{
    class Program
    {
        private const string CONFIG_PATH = "dumper_config.json";

        /// <summary>
        /// Dumper Configuration File.
        /// </summary>
        internal static DumperConfig Config { get; }

        static Program()
        {
            Console.OutputEncoding = Encoding.Unicode;
            if (!File.Exists(CONFIG_PATH))
            {
                Config = new();
                File.WriteAllText(CONFIG_PATH, JsonSerializer.Serialize(Config, new JsonSerializerOptions { WriteIndented = true }));
            }
            else
            {
                var configText = File.ReadAllText(CONFIG_PATH);
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
                    case MenuSelection.All:
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
            var title = new FigletText("Tarkov Dumper")
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
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            AnsiConsole.Status().Start($"Starting {typeof(T)}...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
                ctx.SpinnerStyle(Style.Parse("green"));

                AbstractProcessor processor = null;
                try
                {
                    processor = Activator.CreateInstance<T>();
                    processor.Run(ctx);
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine();

                    if (processor is not null)
                    {
                        var step = (processor is EftProcessor ep) ? ep.LastStepName : processor.LastStepName;
                        AnsiConsole.MarkupLine($"[bold yellow]Exception thrown while processing step -> {step}[/]");
                    }

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
                    GC.Collect();
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                    Thread.CurrentThread.Priority = ThreadPriority.Normal;
                    Pause();
                }
            });
        }

        private static void Pause()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
