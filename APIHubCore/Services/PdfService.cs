using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Services
{

        public class PdfService
        {
            private readonly IConverter _converter;

            public PdfService()
            {
                _converter = new SynchronizedConverter(new PdfTools());
            }

            public void ConvertHtmlToPdf(string htmlContent, string outputPath)
            {
                var document = new HtmlToPdfDocument
                {
                    GlobalSettings = new GlobalSettings
                    {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                        Out = outputPath
                    },
                    Objects = { new ObjectSettings { HtmlContent = htmlContent } }
                };

                _converter.Convert(document);
            }

        public void ConvertTo()
        {
            var htmlContent = @"
            <html>
                <head>
                    <style>
                        body { font-family: Arial, sans-serif; }
                        h1 { color: #3498db; }
                        p { font-size: 14px; }
                    </style>
                </head>
                <body>
                    <h1>Hello, World!</h1>
                    <p>This is a PDF generated from HTML content.</p>
                </body>
            </html>";

        
            var pdf = GeneratePdfFromHtml(htmlContent);
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.pdf");

            File.WriteAllBytes(outputPath, pdf);
            Console.WriteLine($"PDF generated and saved to {outputPath}");

        }

        public  byte[] GeneratePdfFromHtml(string html)
        {
            var converter = new SynchronizedConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                        FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Footer text" }
                    }
                }
            };

            return converter.Convert(doc);
        }
    }
}
