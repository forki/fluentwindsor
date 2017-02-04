namespace Example.Console
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string value, string compareTo)
        {
            if (value == null && compareTo != null)
                return false;

            if (value != null && compareTo == null)
                return false;

            if (value == null && compareTo == null)
                return true;

            return value.ToLower() == compareTo.ToLower();
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}