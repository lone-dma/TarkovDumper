using dnlib.DotNet;
using ICSharpCode.Decompiler.CSharp;
using Spectre.Console;
using TarkovDumper.Helpers;

namespace TarkovDumper.Processors
{
    public abstract class AbstractProcessor
    {
        protected readonly ModuleDefMD _module;
        protected readonly Decompiler _decompiler_Basic;
        protected readonly Decompiler _decompiler_Async;
        protected readonly DnlibHelper _dnlibHelper;
        protected readonly DumpParser _dumpParser;

        public string LastStepName { get; protected set; } = "N/A";

        public AbstractProcessor(string assemblyPath, string dumpPath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(assemblyPath, nameof(assemblyPath));
            ArgumentException.ThrowIfNullOrWhiteSpace(dumpPath, nameof(dumpPath));
            if (!File.Exists(assemblyPath))
                throw new FileNotFoundException("Assembly path is invalid.", assemblyPath);
            if (!File.Exists(dumpPath))
                throw new FileNotFoundException("Dump path is invalid.", dumpPath);
            try
            {
                _module = ModuleDefMD.Load(assemblyPath);
                _module.EnableTypeDefFindCache = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"[bold yellow]Error loading assembly ~[/] [red]{ex.Message}[/]");
            }

            try
            {
                CSharpDecompiler CSharpDecompiler_basic = new(assemblyPath, new()
                {
                    AnonymousMethods = false,
                    ThrowOnAssemblyResolveErrors = false,
                    AsyncAwait = false,
                });
                _decompiler_Basic = new(CSharpDecompiler_basic);

                CSharpDecompiler CSharpDecompiler_async = new(assemblyPath, new()
                {
                    AnonymousMethods = false,
                    ThrowOnAssemblyResolveErrors = false,
                });
                _decompiler_Async = new(CSharpDecompiler_async);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"[bold yellow]Error loading decompiler ~[/] [red]{ex.Message}[/]");
            }

            try { _dnlibHelper = new(_module); }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"[bold yellow]Error loading dnlib helper ~[/] [red]{ex.Message}[/]");
            }

            try { _dumpParser = new(dumpPath); }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"[bold yellow]Error loading dump parser ~[/] [red]{ex.Message}[/]");
            }
        }

        /// <summary>
        /// Run this Processor Job.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Run(StatusContext ctx)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"Processing {this.GetType()} entries...");
        }
    }
}