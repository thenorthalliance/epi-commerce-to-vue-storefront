using EPiServer.VueStorefrontApiBridge.User.Invoice.Model;
using Mediachase.Commerce.Customers;


namespace EPiServer.VueStorefrontApiBridge.User.Invoice
{
    public interface IInvoiceAdapter
    {
        InvoiceInformation GetInvoiceInformation(CustomerContact customerContact, CustomerAddress address);
    }

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