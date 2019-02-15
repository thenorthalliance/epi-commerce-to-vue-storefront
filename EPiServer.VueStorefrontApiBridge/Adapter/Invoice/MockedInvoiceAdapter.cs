using EPiServer.VueStorefrontApiBridge.Model.Invoice;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.Adapter.Invoice
{
    public class MockedInvoiceAdapter : IInvoiceAdapter
    {
        public InvoiceInformation GetInvoiceInformation(CustomerContact customerContact, CustomerAddress address)
        {
            return new InvoiceInformation
            {
                VatId = "VAT123",
                Company = "ACME"
            };
        }
    }
}