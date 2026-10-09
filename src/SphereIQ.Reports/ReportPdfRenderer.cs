using System;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace SphereIQ.Reports
{
    /// <summary>
    /// Phase 2 PDF pipeline on PDFsharp 6 (cross-platform, no GDI+).
    /// Renders a <see cref="ReportDocument"/> to A4 PDF: cover, tables
    /// (with pagination), key-value detail pages, and chart placeholders
    /// (Phase 3 fills real OxyPlot snapshots via AppendChartPage data).
    /// </summary>
    public static partial class ReportPdfRenderer
    {
        private const double Margin = 50;
        private const double LineGap = 15;

        private static bool _resolverRegistered;

        private static void EnsureResolver()
        {
            if (!_resolverRegistered)
            {
                GlobalFontSettings.FontResolver = new SphereFontResolver();
                _resolverRegistered = true;
            }
        }

        public static void Save(ReportDocument doc, string filePath)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            using (PdfDocument pdf = Build(doc))
            {
                pdf.Save(filePath);
            }
        }

        public static void Save(ReportDocument doc, Stream stream)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            using (PdfDocument pdf = Build(doc))
            {
                pdf.Save(stream);
            }
        }

        private static PdfDocument Build(ReportDocument doc)
        {
            EnsureResolver();
            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = doc.ReportName ?? "SphereIQ Report";
            pdf.Info.Author = doc.ReportAuthor ?? "SphereIQ";
            pdf.Info.CreationDate = doc.DateTimeCreated;
            foreach (ReportPage page in doc.Pages)
            {
                RenderPage(pdf, page, doc);
            }
            if (pdf.PageCount == 0)
            {
                PdfPage blank = pdf.AddPage();
                blank.Size = PdfSharp.PageSize.A4;
            }
            return pdf;
        }

        private static void RenderPage(PdfDocument pdf, ReportPage page, ReportDocument doc)
        {
            switch (page.Kind)
            {
                case ReportPageKind.Table:
                    RenderTable(pdf, page);
                    break;
                case ReportPageKind.Cover:
                    RenderCover(pdf, page, doc);
                    break;
                default:
                    RenderKeyValues(pdf, page.PageName, DetailLines(page));
                    break;
            }
        }

        private static void RenderCover(PdfDocument pdf, ReportPage page, ReportDocument doc)
        {
            PdfPage pdfPage = pdf.AddPage();
            pdfPage.Size = PdfSharp.PageSize.A4;
            using (XGraphics gfx = XGraphics.FromPdfPage(pdfPage))
            {
                XFont title = new XFont(FontMap.UiFamilyMac, 24);
                XFont sub = new XFont(FontMap.UiFamilyMac, 12);
                double y = 120;
                gfx.DrawString(page.Title ?? doc.ReportName ?? "SphereIQ", title, XBrushes.Black, Margin, y);
                y += 30;
                gfx.DrawString(page.Subtitle ?? "", sub, XBrushes.Gray, Margin, y);
                y += 30;
                gfx.DrawString("Generated " + doc.DateTimeCreated.ToString("yyyy-MM-dd HH:mm"), sub, XBrushes.Gray, Margin, y);
            }
        }
    }
}
