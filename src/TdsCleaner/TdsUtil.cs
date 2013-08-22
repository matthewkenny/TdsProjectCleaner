using System.Globalization;
using System.Text.RegularExpressions;

namespace TdsCleaner
{
    public static class TdsUtil
    {
        private static string charactersRequiringDoubleEncoding = @"$";

        private static string charactersRequiringEncoding = @"%()@';";

        public static string EncodePath(string path)
        {
            var encodedPath = Regex.Replace(path, "[" + charactersRequiringDoubleEncoding + "]",
                                            match => EncodeCharacter(match.Groups[0].Value));

            return Regex.Replace(path, "[" + charactersRequiringEncoding + "]",
                                 match => EncodeCharacter(match.Groups[0].Value));
        }

        public static string DecodePath(string encodedPath)
        {
            return Regex.Replace(encodedPath, "%([0-9a-fA-F]{2})", match => DecodeEntity(match.Groups[1].Value));
        }

        private static string DecodeEntity(string entity)
        {
            return ((char)int.Parse(entity, NumberStyles.AllowHexSpecifier)).ToString(CultureInfo.InvariantCulture);
        }

        private static string EncodeCharacter(string character)
        {
            return "%" + ((byte)character[0]).ToString("x2", CultureInfo.InvariantCulture);
        }
    }
}