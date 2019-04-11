using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.Reference.Commerce.Shared.Services;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Model;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;
using Microsoft.AspNet.Identity;

namespace EPiServer.Reference.Commerce.VsfIntegration
{
    
    public class MyUserAdapter : IUserAdapter<UserModel>
    {
        private readonly ApplicationUserManager<SiteUser> _appUserManager;
        private readonly MailService _mailService;
        private readonly UrlHelper _urlHelper;

        public MyUserAdapter(ApplicationUserManager<SiteUser> appUserManager, MailService mailService, UrlHelper urlHelper)
        {
            _appUserManager = appUserManager;
            _mailService = mailService;
            _urlHelper = urlHelper;
        }

        public async Task<UserModel> GetUserByCredentials(string userLogin, string userPassword)
        {
            var user = await _appUserManager.FindAsync(userLogin, userPassword);
            if (user == null || !user.IsApproved)
                return null;

            return CvtUser(user);
        }

        public async Task<UserModel> GetUserById(string userId)
        {
            var user = await _appUserManager.FindByIdAsync(userId);
            return CvtUser(user);
        }

        public async Task<UserModel> CreateUser(UserCreateModel newUser)
        {
            var appUser = new SiteUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = newUser.Customer.Email,
                Email = newUser.Customer.Email,
                EmailConfirmed = false,
                IsLockedOut = false,
                IsApproved = true,
                CreationDate = DateTime.UtcNow
            };

            var result = await _appUserManager.CreateAsync(appUser, newUser.Password);
            
            if (!result.Succeeded)
            {
                LogDebugErrors("CreateUser failed", result.Errors);
                return null;
            }

            var newContact = CustomerContact.CreateInstance();
            newContact.PrimaryKeyId = new PrimaryKeyId(new Guid(appUser.Id));
            newContact.UserId = "String:" + newUser.Customer.Email; //See UserService.cs:124        
            newContact.AcceptMarketingEmail = false;
            newContact.FirstName = newUser.Customer.FirstName;
            newContact.LastName = newUser.Customer.LastName;
            newContact.Email = newUser.Customer.Email;
            newContact.SaveChanges();

            var user = await _appUserManager.FindByIdAsync(appUser.Id);
            return CvtUser(user);
        }

        public async Task<bool> UpdateUser(string userId, UserModel updatedUser)
        {
            var user = await _appUserManager.FindByIdAsync(userId);
            user.UserName = updatedUser.Email;
            user.Email = updatedUser.Email;

            var identity = await _appUserManager.UpdateAsync(user);

            if (!identity.Succeeded)
            {
                LogDebugErrors("UpdateUser failed", identity.Errors);
                return false;
            }

            return UpdateCustomerContact(userId, updatedUser);
        }

        public async Task<bool> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var result = await _appUserManager.ChangePasswordAsync(userId, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                LogDebugErrors($"Change password for user '{userId}' failed", result.Errors);
                return false;
            }

            return true;
        }

        public async Task<bool> SendResetPasswordEmail(string userEmail)
        {
            //TEST RESETPASWORD EMAIL SENDING
            var user = await _appUserManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                LogDebugErrors($"SendResetPasswordEmail: User '{userEmail}' not found");
                return false;
            }

            var token = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
            var passwordResetUrl = _urlHelper.Action("ResetPassword", "ResetPassword", new { userId = user.Id, code = HttpUtility.UrlEncode(token), language = "en" }, "http");

            var body = $@"Reset password: <a href='{passwordResetUrl}'> Reset </a>";

            await _appUserManager.SendEmailAsync(user.Id, "Vuestorefront password reset.", body);
            await _mailService.SendAsync(new IdentityMessage
            {
                Destination = user.Email,
                Body = body,
                Subject = "Vuestorefront password reset"
            });

            return true;
        }

        private UserModel CvtUser(SiteUser user)
        {
            var userContact = CustomerContext.Current.GetContactById(Guid.Parse(user.Id));
            var userAddresses = userContact?.ContactAddresses?.Select(addr => MapAddress(userContact, addr)).ToList();

            return new UserModel
            {
                Id = user.Id,
                LastName = userContact?.LastName ?? string.Empty,
                FirstName = userContact?.FirstName ?? string.Empty,
                Email = userContact?.Email ?? string.Empty,
                CreatedAt = user.CreationDate,
                UpdatedAt = userContact?.Modified ?? user.CreationDate,
                Addresses = userAddresses,
                DefaultShippingId = userAddresses?.FirstOrDefault(a => a.DefaultShipping)?.Id,
                DefaultBillingId = userAddresses?.FirstOrDefault(a => a.DefaultBilling)?.Id
            };
        }

        protected UserAddressModel MapAddress(CustomerContact contact, CustomerAddress address)
        {
            var isBillingAddress = (address.AddressType & CustomerAddressTypeEnum.Billing) != 0;
            var isDefaultBillingAddress = contact.PreferredBillingAddress?.AddressId == address.AddressId;
            var isDefaultShipping = contact.PreferredShippingAddress?.AddressId == address.AddressId;
            var invoiceInformation = isBillingAddress ? CreateMockedInvoice(contact, address) : null;

            return new UserAddressModel(address, invoiceInformation, isDefaultBillingAddress, isDefaultShipping);
        }

        private InvoiceInformation CreateMockedInvoice(CustomerContact customerContact, CustomerAddress address)
        {
            return new InvoiceInformation
            {
                VatId = "VAT123",
                Company = "ACME"
            };
        }

        protected void LogDebugErrors(string message, IEnumerable<string> errors = null)
        {
            var errorMessage = errors != null ?
                $"{message}:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}" :
                message;

            LogManager.GetLogger(GetType()).Debug(errorMessage);
        }

        public bool UpdateCustomerContact(string userId, UserModel updatedUser)
        {
            var currentContact = CustomerContext.Current.GetContactById(new Guid(userId));
            currentContact.FullName = string.Concat(updatedUser.FirstName, " ", updatedUser.LastName);
            currentContact.FirstName = updatedUser.FirstName;
            currentContact.LastName = updatedUser.LastName;
            currentContact.Email = updatedUser.Email;
            currentContact.UserId = "String:" + updatedUser.Email; //See UserService.cs:124

            return UpdateContactAddresses(userId, currentContact, updatedUser.Addresses);
        }

        public bool UpdateContactAddresses(string userId, CustomerContact currentContact, IEnumerable<UserAddressModel> userAddresses)
        {
            var updatedAddresses = userAddresses?.Where(x => x.DefaultShipping || x.DefaultBilling);
            if (updatedAddresses == null)
            {
                currentContact.SaveChanges();
                return true;
            }

            foreach (var updatedAddress in updatedAddresses)
            {
                var currentAddress = currentContact.ContactAddresses
                    .FirstOrDefault(x => x.AddressId.ToString() == updatedAddress.Id);
                CustomerAddress address;
                if (currentAddress == null)
                {
                    address = CreateCustomerAddress(updatedAddress, new Guid(userId));
                    currentContact.AddContactAddress(address);
                }
                else
                {
                    UpdateContactAddressData(currentAddress, updatedAddress);
                    address = currentAddress;
                    currentContact.UpdateContactAddress(address);
                }

                if (updatedAddress.DefaultBilling)
                {
                    currentContact.PreferredBillingAddress = address;

                }

                if (updatedAddress.DefaultShipping)
                {
                    currentContact.PreferredShippingAddress = address;
                }
            }

            currentContact.SaveChanges();
            return true;
        }

        public CustomerAddress CreateCustomerAddress(UserAddressModel addressModel, Guid userGuid)
        {
            var customerAddress = CustomerAddress.CreateInstance();
            UpdateContactAddressData(customerAddress, addressModel);
            customerAddress.ContactId = new PrimaryKeyId(userGuid);
            customerAddress.AddressId = new PrimaryKeyId(Guid.NewGuid());

            return customerAddress;
        }

        public void UpdateContactAddressData(CustomerAddress customerAddress, UserAddressModel updatedAddressModel)
        {
            customerAddress.City = updatedAddressModel.City;
            customerAddress.RegionName = updatedAddressModel.Region?.Region;
            customerAddress.FirstName = updatedAddressModel.Firstname;
            customerAddress.LastName = updatedAddressModel.Lastname;
            customerAddress.Line1 = updatedAddressModel.Street?[0];
            customerAddress.Line2 = updatedAddressModel.Street?[1];
            customerAddress.CountryCode = updatedAddressModel.CountryId;
            customerAddress.PostalCode = updatedAddressModel.Postcode;
            customerAddress.DaytimePhoneNumber = updatedAddressModel.Telephone;

            if (updatedAddressModel.DefaultBilling)
            {
                customerAddress.AddressType |= CustomerAddressTypeEnum.Billing;
            }

            if (updatedAddressModel.DefaultShipping)
            {
                customerAddress.AddressType |= CustomerAddressTypeEnum.Shipping;
            }
        }
    }
}