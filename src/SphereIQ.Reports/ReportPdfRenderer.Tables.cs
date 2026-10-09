using System.Collections.Generic;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace SphereIQ.Reports
{
    public static partial class ReportPdfRenderer
    {
        private static void RenderTable(PdfDocument pdf, ReportPage page)
        {
            string[] cols = page.Columns ?? new string[0];
            List<string[]> rows = page.Rows ?? new List<string[]>();
            int colCount = System.Math.Max(cols.Length, 1);
            PdfPage pdfPage = null;
            XGraphics gfx = null;
            XFont font = new XFont(FontMap.UiFamilyMac, 8);
            XFont headFont = new XFont(FontMap.UiFamilyMac, 9);
            double y = 0;
            try
            {
                for (int r = -1; r < rows.Count; r++)
                {
                    if (pdfPage == null)
                    {
                        pdfPage = pdf.AddPage();
                        pdfPage.Size = PdfSharp.PageSize.A4;
                        if (gfx != null)
                        {
                            gfx.Dispose();
                        }
                        gfx = XGraphics.FromPdfPage(pdfPage);
                        y = Margin;
                        gfx.DrawString(page.PageName ?? "Table", headFont, XBrushes.Black, Margin, y);
                        y += 20;
                        double x = Margin;
                        double w = (pdfPage.Width.Point - 2 * Margin) / colCount;
                        foreach (string c in cols)
                        {
                            gfx.DrawString(c ?? "", headFont, XBrushes.Black, x, y);
                            x += w;
                        }
                        y += LineGap;
                    }
                    if (r >= 0)
                    {
                        double x = Margin;
                        double w = (pdfPage.Width.Point - 2 * Margin) / colCount;
                        foreach (string cell in rows[r])
                        {
                            gfx.DrawString(cell ?? "", font, XBrushes.Black, x, y);
                            x += w;
                        }
                        y += LineGap;
                    }
                    if (y > pdfPage.Height.Point - Margin)
                    {
                        gfx.Dispose();
                        gfx = null;
                        pdfPage = null;
                    }
                }
            }
            finally
            {
                if (gfx != null)
                {
                    gfx.Dispose();
                }
            }
        }

        private static void RenderKeyValues(PdfDocument pdf, string heading, List<string[]> lines)
        {
            PdfPage pdfPage = pdf.AddPage();
            pdfPage.Size = PdfSharp.PageSize.A4;
            using (XGraphics gfx = XGraphics.FromPdfPage(pdfPage))
            {
                XFont headFont = new XFont(FontMap.UiFamilyMac, 14);
                XFont font = new XFont(FontMap.UiFamilyMac, 10);
                double y = Margin;
                gfx.DrawString(heading ?? "Report", headFont, XBrushes.Black, Margin, y);
                y += 24;
                if (lines != null)
                {
                    foreach (string[] kv in lines)
                    {
                        string text = kv.Length > 1 ? (kv[0] + ": " + kv[1]) : kv[0];
                        gfx.DrawString(text, font, XBrushes.Black, Margin, y);
                        y += LineGap + 2;
                        if (y > pdfPage.Height.Point - Margin)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
