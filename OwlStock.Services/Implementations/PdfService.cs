using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OwlStock.Services.Interfaces;
using PdfSharp;
using PdfSharp.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace OwlStock.Services.Implementations
{
    public class PdfService : IPdfService
    {
        private ILogger<PdfService> _logger;

        public PdfService(ILogger<PdfService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Generates byte array for PDF generation out of HTML code
        /// </summary>
        /// <param name="html">The HTML code as string</param>
        /// <returns>Byte array</returns>
        public byte[]? GeneratePdfFromHtml(string html)
        {
            if (html.IsNullOrEmpty())
            {
                _logger.LogError("{html} is null in {Method}, {Class}, {DateTime}", nameof(html), nameof(GeneratePdfFromHtml), nameof(PdfService), DateTime.Now);
                return null;
            }

            PdfGenerateConfig config = new()
            {
                PageSize = PageSize.A4,
                PageOrientation = PageOrientation.Portrait,
                MarginTop = 20,
                MarginBottom = 20,
                MarginLeft = 20,
                MarginRight = 20
            };

            html = """
<!DOCTYPE html>
<html>
<body>
    <h1>Hello World</h1>
    <p>This is a test.</p>
</body>
</html>
""";

            using PdfDocument document = PdfGenerator.GeneratePdf(html, config);

            using var stream = new MemoryStream();
            document.Save(stream, false);

            return stream.ToArray();
        }
    }
}
