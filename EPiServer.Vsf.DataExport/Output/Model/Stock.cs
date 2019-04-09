using Nest;

namespace EPiServer.Vsf.DataExport.Output.Model
{
    public class Stock
    {
        [PropertyName("is_in_stock")]
        public bool IsInStock { get; set; }

        [PropertyName("qty")]
        public int Quantity { get; set; }
    }
}