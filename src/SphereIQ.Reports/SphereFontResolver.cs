using System;
using System.Collections.Generic;
using System.IO;
using PdfSharp.Fonts;

namespace SphereIQ.Reports
{
    /// <summary>
    /// PDFsharp 6 font resolver: maps logical families to real font files.
    /// macOS: Helvetica/Helvetica Neue from /System/Library/Fonts.
    /// Windows (dev/CI + legacy-converter runs): Arial from %WINDIR%/Fonts.
    /// No GDI+, no System.Drawing — pure file lookup.
    /// </summary>
    public sealed class SphereFontResolver : IFontResolver
    {
        // Map every logical family to a single-face TTF (no .ttc collections:
        // PDFsharp 6 needs one face per resolver key). Windows dev/CI uses
        // Arial; macOS bundles Helvetica.ttc only, so prefer the per-face
        // Geneva/HelveticaNeue discrete files when present, else Arial.
        private static readonly string[] Candidates = new string[]
        {
            "HelveticaNeue.ttc",
            "arial.ttf",
            "Helvetica.ttf",
            "Geneva.ttf"
        };

        private static readonly string[] SearchDirs = BuildSearchDirs();

        private static string[] BuildSearchDirs()
        {
            List<string> dirs = new List<string>();
            dirs.Add(AppContext.BaseDirectory);
            dirs.Add(Path.Combine(AppContext.BaseDirectory, "fonts"));
            string windir = Environment.GetEnvironmentVariable("WINDIR");
            if (!string.IsNullOrEmpty(windir))
            {
                dirs.Add(Path.Combine(windir, "Fonts"));
            }
            dirs.Add("/System/Library/Fonts");
            dirs.Add("/Library/Fonts");
            string home = Environment.GetEnvironmentVariable("HOME");
            if (!string.IsNullOrEmpty(home))
            {
                dirs.Add(Path.Combine(home, "Library", "Fonts"));
            }
            return dirs.ToArray();
        }

        public string DefaultFontName => "Arial";

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo("Arial");
        }

        public byte[] GetFont(string faceName)
        {
            foreach (string dir in SearchDirs)
            {
                foreach (string file in Candidates)
                {
                    try
                    {
                        // Skip .ttc collections: PDFsharp 6 cannot select a face from them.
                        if (file.EndsWith(".ttc", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                        string full = Path.Combine(dir, file);
                        if (File.Exists(full))
                        {
                            return File.ReadAllBytes(full);
                        }
                    }
                    catch (IOException)
                    {
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                }
            }
            throw new InvalidOperationException(
                "No usable single-face font found. searched: " + string.Join(", ", SearchDirs));
        }
    }
}
