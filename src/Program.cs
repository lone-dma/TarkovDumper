using Spectre.Console;
using System.Diagnostics;

namespace TarkovDumper
{
    internal class Program
    {
        static void Main()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            AnsiConsole.Profile.Width = 420;
            AnsiConsole.Status().Start("Starting...", ctx => {
                ctx.Spinner(Spinner.Known.Dots);
                ctx.SpinnerStyle(Style.Parse("green"));

                EftProcessor processor = null;
                try
                {
                    string asm = Environment.GetEnvironmentVariable("TARKOV_DUMPER_ASSEMBLY", EnvironmentVariableTarget.Machine);
                    asm ??= Environment.GetEnvironmentVariable("TARKOV_DUMPER_ASSEMBLY", EnvironmentVariableTarget.User);
                    asm ??= Path.Combine(Environment.CurrentDirectory, "Assembly-CSharp.dll");
                    string dump = Environment.GetEnvironmentVariable("TARKOV_DUMPER_DUMP", EnvironmentVariableTarget.Machine);
                    dump ??= Environment.GetEnvironmentVariable("TARKOV_DUMPER_DUMP", EnvironmentVariableTarget.User);
                    dump ??= Path.Combine(Environment.CurrentDirectory, "dump.txt");
                    string output = Environment.GetEnvironmentVariable("TARKOV_DUMPER_OUTPUT", EnvironmentVariableTarget.Machine);
                    output ??= Environment.GetEnvironmentVariable("TARKOV_DUMPER_OUTPUT", EnvironmentVariableTarget.User);
                    output ??= Path.Combine(Environment.CurrentDirectory, "SDK.cs");
                    Console.WriteLine(
                        $"ASSEMBLY INPUT: {asm}\n" +
                        $"DUMP INPUT: {dump}\n" +
                        $"SDK OUTPUT: {output}");
                    processor = new EftProcessor(asm, dump, output);
                    processor.Run(ctx);
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine();

                    if (processor != null)
                        AnsiConsole.MarkupLine($"[bold yellow]Exception thrown while processing step -> {processor.LastStepName}[/]");

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
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                    Thread.CurrentThread.Priority = ThreadPriority.Normal;
                }
            });

            Pause();
        }

        private static void Pause()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
