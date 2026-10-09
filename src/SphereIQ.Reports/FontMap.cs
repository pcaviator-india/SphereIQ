using System;
using System.Collections.Generic;

namespace SphereIQ.Reports
{
    /// <summary>
    /// Phase 2 font map: WinForms logical fonts → cross-platform families.
    /// WinForms sizes are in points; Avalonia uses DIPs (1/96"). At 96 DPI
    /// 1pt = 4/3 DIPs; this table is the single source of truth for Phase 3
    /// layout conversion so heading/body/grid text matches the original.
    /// </summary>
    public static class FontMap
    {
        public const string UiFamilyWindows = "Microsoft Sans Serif";
        public const string UiFamilyMac = "Helvetica Neue";
        public const string UiFamilyFallback = "Inter";

        public const double PointsToDips = 96.0 / 72.0;

        public static double ToDips(double points)
        {
            return points * PointsToDips;
        }

        public static string MapFamily(string windowsFamily)
        {
            if (string.Equals(windowsFamily, UiFamilyWindows, StringComparison.OrdinalIgnoreCase))
            {
                return UiFamilyMac;
            }
            return windowsFamily;
        }

        public static readonly Dictionary<string, double> Roles = new Dictionary<string, double>
        {
            { "ScreenTitle", 15.75 },
            { "Header", 15.75 },
            { "Button", 14.25 },
            { "Grid", 9.75 },
            { "Label", 9.75 },
            { "Small", 8.25 }
        };
    }
}
