using EPiServer.VueStorefrontApiBridge.Model.Invoice;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.Adapter.Invoice
{
    public interface IInvoiceAdapter
    {
        InvoiceInformation GetInvoiceInformation(CustomerContact customerContact, CustomerAddress address);
    }
}