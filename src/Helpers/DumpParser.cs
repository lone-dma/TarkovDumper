using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace TarkovSdkGen
{
    public sealed partial class DumpParser
    {
        [GeneratedRegex("^(0x|0X)?[a-fA-F0-9]+$")]
        private static partial Regex OffsetRegex();

        private readonly string[] _dump;

        public DumpParser(string dumpPath)
        {
            _dump = File.ReadAllLines(dumpPath);
        }

        public readonly struct Result<T>(bool success, T value = default)
        {
            public readonly bool Success = success;
            public readonly T Value = value;
        }

        public readonly struct OffsetData(string offsetName, string typeName, uint offset)
        {
            public readonly string OffsetName = offsetName;
            public readonly string TypeName = typeName;
            public readonly uint Offset = offset;

            public override string ToString() => $"0x{Offset:X}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<OffsetData> FindOffsetByName(string offsetGroupName, string offsetName) => FindOffset(offsetGroupName, offsetName: offsetName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<OffsetData> FindOffsetByTypeName(string offsetGroupName, string typeName) => FindOffset(offsetGroupName, typeName: typeName);

        public enum SearchType
        {
            TypeName,
            OffsetName
        }

        public readonly struct EntitySearchListEntry(string name, SearchType searchType)
        {
            public readonly string Name = name;
            public readonly SearchType SearchType = searchType;
        }

        /// <summary>
        /// Finds an offset group (class / struct) that contains all of the given entities.
        /// Supports partial group name search ("GameWorld" will match "EFT.GameWorld").
        /// </summary>
        public string FindOffsetGroupWithEntities(List<EntitySearchListEntry> entities)
        {
            const StringComparison ct = StringComparison.OrdinalIgnoreCase;

            for (int i = 0; i < _dump.Length; i++)
            {
                string cleaned = CleanLine(_dump[i]);

                if (IsOffsetGroup(cleaned))
                {
                    ReadOnlySpan<string> foundOffsetGroup = GetOffsetGroupExtents(i + 1);
                    if (foundOffsetGroup == null)
                        continue;

                    int foundCount = 0;

                    foreach (EntitySearchListEntry entity in entities)
                    {
                        foreach (string line in foundOffsetGroup)
                        {
                            var lineData = ExtractLineData(CleanLine(line));
                            if (!lineData.Success)
                                continue;

                            if (entity.SearchType == SearchType.TypeName)
                            {
                                if (lineData.Value.TypeName.Equals(entity.Name, ct))
                                {
                                    foundCount++;
                                    break;
                                }
                            }
                            else // OffsetName
                            {
                                string alias = GetBackingFieldPropertyName(lineData.Value.OffsetName);
                                if (lineData.Value.OffsetName.Equals(entity.Name, ct) ||
                                    (alias != null && alias.Equals(entity.Name, ct)))
                                {
                                    foundCount++;
                                    break;
                                }
                            }
                        }
                    }

                    if (foundCount == entities.Count)
                    {
                        string groupName = ExtractGroupName(cleaned);
                        return groupName;
                    }
                }
            }

            return null;
        }

        #region Private Methods

        private Result<OffsetData> FindOffset(string offsetGroupName, string offsetName = null, string typeName = null)
        {
            const StringComparison ct = StringComparison.OrdinalIgnoreCase;

            if (offsetGroupName == null)
                return new(false);

            if (offsetName == null && typeName == null)
                return new(false);

            // Resolve group by flexible name matching
            ReadOnlySpan<string> foundClass = FindOffsetGroupByFlexibleName(offsetGroupName);
            if (foundClass == null)
                return new(false);

            foreach (string line in foundClass)
            {
                string cleaned = CleanLine(line);

                var lineData = ExtractLineData(cleaned);
                if (!lineData.Success)
                    continue;

                bool nameMatch = false;
                if (offsetName != null)
                {
                    string alias = GetBackingFieldPropertyName(lineData.Value.OffsetName);
                    nameMatch = lineData.Value.OffsetName.Equals(offsetName, ct) ||
                                (alias != null && alias.Equals(offsetName, ct));
                }

                bool typeMatch = typeName != null && lineData.Value.TypeName.Equals(typeName, ct);

                if (!nameMatch && !typeMatch)
                    continue;

                uint offset = uint.Parse(lineData.Value.Offset, System.Globalization.NumberStyles.HexNumber);
                return new(true, new(lineData.Value.OffsetName, lineData.Value.TypeName, offset));
            }

            return new(false);
        }

        private static string GetBackingFieldPropertyName(string offsetName)
        {
            if (string.IsNullOrEmpty(offsetName))
                return null;
            if (!offsetName.StartsWith('<') || !offsetName.EndsWith("k__BackingField", StringComparison.Ordinal))
                return null;
            int closingIndex = offsetName.IndexOf('>');
            if (closingIndex <= 1)
                return null;
            return offsetName.Substring(1, closingIndex - 1);
        }

        /// <summary>
        /// Flexible group name resolution. Accepts fully qualified name or short (last segment).
        /// </summary>
        private ReadOnlySpan<string> FindOffsetGroupByFlexibleName(string name)
        {
            const StringComparison ct = StringComparison.OrdinalIgnoreCase;
            if (string.IsNullOrWhiteSpace(name))
                return null;

            for (int i = 0; i < _dump.Length; i++)
            {
                string cleaned = CleanLine(_dump[i]);
                if (!IsOffsetGroup(cleaned))
                    continue;

                string groupName = ExtractGroupName(cleaned);
                if (groupName == null)
                    continue;

                // Direct match (fully qualified)
                if (groupName.Equals(name, ct))
                    return GetOffsetGroupExtents(i + 1);

                // Short name match (last segment after '.')
                int lastDot = groupName.LastIndexOf('.');
                if (lastDot >= 0)
                {
                    string shortName = groupName.Substring(lastDot + 1);
                    if (shortName.Equals(name, ct))
                        return GetOffsetGroupExtents(i + 1);
                }
            }
            return null;
        }

        private static string ExtractGroupName(string cleaned)
        {
            try
            {
                // New format: [Class] EFT.GameWorld : UnityEngine.MonoBehaviour
                // Old format: [Class] -.\uE07F : System.Object
                if (!cleaned.StartsWith("[", StringComparison.Ordinal))
                    return null;

                int closingBracket = cleaned.IndexOf(']');
                if (closingBracket < 0)
                    return null;

                // Skip ']' and get remainder
                string remainder = cleaned.Substring(closingBracket + 1).Trim();
                
                // Find the class name (before ':')
                int colonIndex = remainder.IndexOf(':');
                string namePart = colonIndex >= 0 ? remainder.Substring(0, colonIndex).Trim() : remainder;

                // Remove optional "-." prefix if present
                if (namePart.StartsWith("-.", StringComparison.Ordinal))
                    namePart = namePart.Substring(2).Trim();

                return namePart.Length == 0 ? null : namePart;
            }
            catch
            {
                return null;
            }
        }

        private ReadOnlySpan<string> GetOffsetGroupExtents(int startIndex)
        {
            int endIndex = -1;
            int lastLineWithOffset = -1;

            for (int i = startIndex; i < _dump.Length; i++)
            {
                string cleaned = CleanLine(_dump[i], true);

                // Check if we hit another class/struct header
                if (IsOffsetGroup(cleaned))
                {
                    endIndex = lastLineWithOffset > -1 ? lastLineWithOffset : i;
                    break;
                }

                var lineData = ExtractLineData(cleaned);
                if (lineData.Success && IsHexString(lineData.Value.Offset))
                {
                    lastLineWithOffset = i + 1;
                    continue;
                }

                // Empty line or non-offset line
                if (string.IsNullOrWhiteSpace(cleaned))
                    continue;
            }

            if (endIndex > -1)
                return _dump.AsSpan(startIndex, endIndex - startIndex);

            // Reached end of file; include up to last offset if present
            if (lastLineWithOffset > -1)
                return _dump.AsSpan(startIndex, lastLineWithOffset - startIndex);

            return null;
        }

        private static string CleanLine(string line, bool minimal = false)
        {
            const StringComparison ct = StringComparison.OrdinalIgnoreCase;
            line = line.Trim();
            if (!minimal)
                line = line.Replace("[C]", "", ct).Replace("[S]", "", ct);
            return line;
        }

        private readonly struct LineData(string offset, string offsetName, string typeName)
        {
            public readonly string Offset = offset;
            public readonly string OffsetName = offsetName;
            public readonly string TypeName = typeName;
        }

        private static Result<LineData> ExtractLineData(string cleaned)
        {
            try
            {
                // New format: "    [00] Position : object"
                // Pattern: [hex_offset] field_name : type_name
                
                // Skip lines that don't have field data
                if (!cleaned.Contains('[') || !cleaned.Contains(']'))
                    return new(false);

                int open = cleaned.IndexOf('[');
                int close = cleaned.IndexOf(']');
                if (open < 0 || close < open)
                    return new(false);

                // Extract hex offset (e.g., "00", "10", "A8")
                string offset = cleaned.Substring(open + 1, close - open - 1).Trim();
                
                // Validate it's a hex string
                if (!IsHexString(offset))
                    return new(false);

                // Get everything after ']'
                string after = cleaned.Substring(close + 1).TrimStart();
                
                // Find the colon separator
                int colonIdx = after.IndexOf(':');
                if (colonIdx < 0)
                    return new(false);

                // Extract field name and type name
                string offsetName = after.Substring(0, colonIdx).Trim();
                string typeName = after.Substring(colonIdx + 1).Trim();

                return new(true, new(offset, offsetName, typeName));
            }
            catch
            {
                return new(false);
            }
        }

        private static bool IsOffsetGroup(string cleaned)
        {
            const StringComparison ct = StringComparison.OrdinalIgnoreCase;
            // Check for both [Class] and [Struct] (and also [Interface])
            return cleaned.StartsWith("[class]", ct) || 
                   cleaned.StartsWith("[struct]", ct) ||
                   cleaned.StartsWith("[interface]", ct);
        }

        private static bool IsHexString(string data) => OffsetRegex().IsMatch(data);

        #endregion
    }
}
