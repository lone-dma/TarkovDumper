using System.Text.Json.Serialization;

namespace TarkovDumper.UI
{
    public sealed class DumperConfig
    {
        [JsonPropertyName("eft")]
        [JsonInclude]
        public EFTConfig EFT { get; private set; } = new();
        [JsonPropertyName("arena")]
        [JsonInclude]
        public ArenaConfig Arena { get; private set; } = new();
    }

    public sealed class EFTConfig
    {
        [JsonPropertyName("assemblyPath")]
        public string AssemblyPath { get; set; }
        [JsonPropertyName("dumpPath")]
        public string DumpPath { get; set; }
        [JsonPropertyName("outputPath")]
        public string OutputPath { get; set; }
    }

    public sealed class ArenaConfig
    {
        [JsonPropertyName("assemblyPath")]
        public string AssemblyPath { get; set; }
        [JsonPropertyName("dumpPath")]
        public string DumpPath { get; set; }
        [JsonPropertyName("outputPath")]
        public string OutputPath { get; set; }
    }
}
