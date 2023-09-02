using System.Text.RegularExpressions;

namespace RentCar_AspNetCore7.Utilities
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            string str = phrase.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");  // Invalid chars
            str = Regex.Replace(str, @"\s+", "-").Trim();  // Convert whitespaces to hyphen
            str = Regex.Replace(str, @"-+", "-");  // Convert multiple hyphens to single hyphen
            return str;
        }
    }
}
