# Tarkov-SDKGEN

SDK Generator for EFT/Arena.

Relies on a [custom Il2CppDumper implementation](https://github.com/lone-dma/Il2CppDumper/tree/lone)

## How to use
1. Set the path of your `dump.txt` file from [UnispectEx](https://github.com/lone-dma/UnispectEx) in `%AppData%\Tarkov-SDKGEN\dumper_config.json`.
2. Set the path of your `Assembly-CSharp.dll` file in `%AppData%\Tarkov-SDKGEN\dumper_config.json`.
3. Set your output (SDK) path in `%AppData%\Tarkov-SDKGEN\dumper_config.json`.
4. Run Tarkov-SDKGEN. It will generate a `SDK.cs` output with offsets, etc.
