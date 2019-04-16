using EPiServer.Core;

namespace EPiServer.Vsf.DataExport.Utils.Epi
{
    public interface IVsfPriceService
    {
        decimal GetDefaultPrice(ContentReference reference);
    }
}