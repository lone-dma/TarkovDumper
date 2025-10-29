using System.Text.Json.Serialization;

namespace TarkovDumper.UI
{
    public sealed class DumperConfig
    {
        [JsonPropertyName("eft")]
        [JsonInclude]
        public ProcessorConfig EFT { get; private set; } = new();
        [JsonPropertyName("arena")]
        [JsonInclude]
        public ProcessorConfig Arena { get; private set; } = new();
    }

    public sealed class ProcessorConfig
    {
        [JsonPropertyName("assemblyPath")]
        public string AssemblyPath { get; set; }
        [JsonPropertyName("dumpPath")]
        public string DumpPath { get; set; }
        [JsonPropertyName("outputPath")]
        public string OutputPath { get; set; }
    }
}
