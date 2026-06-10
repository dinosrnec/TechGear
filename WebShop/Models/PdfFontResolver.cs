using PdfSharp.Fonts;
using System.Reflection;

namespace WebShop
{
    public class PdfFontResolver : IFontResolver
    {
        public static readonly PdfFontResolver Instance = new();

        private static readonly Dictionary<string, string> FontMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Helvetica",      "arial.ttf"   },
            { "Helvetica#bold", "arialbd.ttf" },
        };

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            string key = isBold ? $"{familyName}#bold" : familyName;

            if (FontMap.ContainsKey(key))
                return new FontResolverInfo(key);

            return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
        }

        public byte[]? GetFont(string faceName)
        {
            if (FontMap.TryGetValue(faceName, out var fileName))
            {
                string winFonts = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
                string fullPath = Path.Combine(winFonts, fileName);

                if (File.Exists(fullPath))
                    return File.ReadAllBytes(fullPath);

                string[] linuxPaths =
                {
                    $"/usr/share/fonts/truetype/msttcorefonts/{fileName}",
                    $"/usr/share/fonts/truetype/liberation/{ToLiberationName(fileName)}",
                    $"/usr/share/fonts/{fileName}",
                };

                foreach (var path in linuxPaths)
                    if (File.Exists(path))
                        return File.ReadAllBytes(path);
            }

            return null;
        }

        private static string ToLiberationName(string arialFile) => arialFile switch
        {
            "arial.ttf" => "LiberationSans-Regular.ttf",
            "arialbd.ttf" => "LiberationSans-Bold.ttf",
            _ => arialFile
        };
    }
}
