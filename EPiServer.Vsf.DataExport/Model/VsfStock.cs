using Nest;

namespace EPiServer.Vsf.DataExport.Model
{
    public class VsfStock
    {
        [PropertyName("is_in_stock")]
        public bool IsInStock { get; set; }

        [PropertyName("qty")]
        public int Quantity { get; set; }
    }
}