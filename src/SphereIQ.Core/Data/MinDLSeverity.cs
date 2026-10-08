using System;

namespace EzyPlanData
{
    /// <summary>
    /// Severity of the minimum-dogleg state, replacing the original
    /// <c>System.Drawing.Color</c> (<c>Color.Orange/Red/Black</c>) carried on
    /// <c>DrillHoleStatus.MinimumDLStateColor</c>. The UI layer maps this to
    /// platform text/foreground colors; reports map it to PDF brush colors.
    /// </summary>
    public enum MinDLSeverity
    {
        Ok = 0,
        Warning = 1,
        Severe = 2
    }

    /// <summary>
    /// Small immutable ARGB color value (no System.Drawing dependency) so the
    /// severity tint can round-trip into reports and charts on any platform.
    /// </summary>
    public readonly struct SeverityColor : IEquatable<SeverityColor>
    {
        public readonly byte A;
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public SeverityColor(byte r, byte g, byte b, byte a = 255)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public static SeverityColor FromSeverity(MinDLSeverity severity)
        {
            switch (severity)
            {
                case MinDLSeverity.Warning: return new SeverityColor(255, 165, 0);
                case MinDLSeverity.Severe: return new SeverityColor(255, 0, 0);
                default: return new SeverityColor(0, 0, 0);
            }
        }

        public uint ToArgb()
        {
            return (uint)((A << 24) | (R << 16) | (G << 8) | B);
        }

        public bool Equals(SeverityColor other)
        {
            return A == other.A && R == other.R && G == other.G && B == other.B;
        }

        public override bool Equals(object obj)
        {
            return obj is SeverityColor other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)ToArgb();
        }

        public override string ToString()
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", A, R, G, B);
        }
    }
}
