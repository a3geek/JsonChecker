namespace JsonChecker.Extensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string str, int defaultValue = default)
            => int.TryParse(str, out var v) ? v : defaultValue;

        public static byte ToByte(this string str, byte defaultValue = default)
            => byte.TryParse(str, out var v) ? v : defaultValue;

        public static float ToFloat(this string str, float defaultValue = default)
            => float.TryParse(str, out var v) ? v : defaultValue;

        public static string TrimStart(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString))
            {
                return target;
            }

            var result = target;
            while (result.StartsWith(trimString))
            {
                result = result[trimString.Length..];
            }

            return result;
        }

        public static string TrimEnd(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString))
            {
                return target;
            }

            var result = target;
            while (result.EndsWith(trimString))
            {
                result = result.Substring(0, result.Length - trimString.Length);
            }

            return result;
        }
    }
}
