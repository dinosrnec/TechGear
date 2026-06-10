using PdfSharp.Fonts;
using System.Reflection;

namespace WebShop
{
    /// <summary>
    /// Resolves fonts for PDFsharp 6.x on .NET, which does not bundle any fonts.
    /// Maps "Helvetica" requests to the system Arial font (metrically compatible).
    /// </summary>
    public class PdfFontResolver : IFontResolver
    {
        public static readonly PdfFontResolver Instance = new();

        // Map logical font face names to system font file names
        private static readonly Dictionary<string, string> FontMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Helvetica",      "arial.ttf"   },
            { "Helvetica#bold", "arialbd.ttf" },
        };

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Normalise the key the same way KosaricaController builds it
            string key = isBold ? $"{familyName}#bold" : familyName;

            if (FontMap.ContainsKey(key))
                return new FontResolverInfo(key);

            // Fallback: let PDFsharp try its own resolution
            return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
        }

        public byte[]? GetFont(string faceName)
        {
            if (FontMap.TryGetValue(faceName, out var fileName))
            {
                // Windows font directory
                string winFonts = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
                string fullPath = Path.Combine(winFonts, fileName);

                if (File.Exists(fullPath))
                    return File.ReadAllBytes(fullPath);

                // Linux / Docker fallback paths
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

        // Maps Arial filenames to Liberation Sans equivalents (common on Linux)
        private static string ToLiberationName(string arialFile) => arialFile switch
        {
            "arial.ttf" => "LiberationSans-Regular.ttf",
            "arialbd.ttf" => "LiberationSans-Bold.ttf",
            _ => arialFile
        };
    }
}