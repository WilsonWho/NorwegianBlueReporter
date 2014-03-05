using System.Text.RegularExpressions;

namespace LaserOptics.Utilities
{
    public static class StringExtensions
    {
        private static readonly Regex SoftHyphenLocations = new Regex("(.)", RegexOptions.Compiled);
        public static string InsertSoftHyphens(this string str)
        {
            var ret = SoftHyphenLocations.Replace(str, "$1\u00ad");
            ret = ret.TrimEnd(new[] {'\xad'}); //because PDF rendering seems to crash if there is a soft-hyphen at the end of a string
            return ret;
        }
    }
}
