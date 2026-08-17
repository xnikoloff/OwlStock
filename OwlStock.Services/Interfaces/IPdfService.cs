namespace OwlStock.Services.Interfaces
{
    public interface IPdfService
    {
        byte[]? GeneratePdfFromHtml(string html);
    }
}
