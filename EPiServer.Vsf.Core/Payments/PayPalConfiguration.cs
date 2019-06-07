using System;
using System.Collections.Generic;
using System.Linq;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Managers;

namespace EPiServer.Vsf.Core.Payments
{
    /// <summary>
    /// Represents PayPal configuration data.
    /// </summary>
    public class PayPalConfiguration
    {
        private PaymentMethodDto _paymentMethodDto;
        private IDictionary<string, string> _settings;

        public const string PayPalSystemName = "PayPal";
        public const string PaymentActionParameter = "PayPalPaymentAction";
        public const string SandBoxParameter = "PayPalSandBox";
        public const string ClientIdParameter = "ClientId";
        public const string SecretParameter = "Secret";
        public const string PayPalOrderNumber = "PayPalOrderNumber";
        public const string PayPalExpToken = "PayPalExpToken";

        public Guid PaymentMethodId { get; protected set; }

        public string PaymentAction { get; protected set; }

        public string SandBox { get; protected set; }

        public string ClientId { get; protected set; }

        public string Secret { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="PayPalConfiguration"/>.
        /// </summary>
        public PayPalConfiguration() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PayPalConfiguration"/> with specific settings.
        /// </summary>
        /// <param name="settings">The specific settings.</param>
        public PayPalConfiguration(IDictionary<string, string> settings)
        {
            Initialize(settings);
        }

        /// <summary>
        /// Gets the PaymentMethodDto's parameter (setting in CommerceManager of PayPal) by name.
        /// </summary>
        /// <param name="paymentMethodDto">The payment method dto.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The parameter row.</returns>
        public static PaymentMethodDto.PaymentMethodParameterRow GetParameterByName(PaymentMethodDto paymentMethodDto, string parameterName)
        {
            var rowArray = (PaymentMethodDto.PaymentMethodParameterRow[])paymentMethodDto.PaymentMethodParameter.Select(string.Format("Parameter = '{0}'", parameterName));
            return rowArray.Length > 0 ? rowArray[0] : null;
        }

        /// <summary>
        /// Returns the PaymentMethodDto of PayPal.
        /// </summary>
        /// <returns>The PayPal payment method.</returns>
        public static PaymentMethodDto GetPayPalPaymentMethod()
        {
            return PaymentManager.GetPaymentMethodBySystemName(PayPalSystemName, SiteContext.Current.LanguageName);
        }

        protected virtual void Initialize(IDictionary<string, string> settings)
        {
            _paymentMethodDto = GetPayPalPaymentMethod();
            PaymentMethodId = GetPaymentMethodId();

            _settings = settings ?? GetSettings();
            GetParametersValues();
        }

        private IDictionary<string, string> GetSettings()
        {
            return _paymentMethodDto.PaymentMethod
                                    .FirstOrDefault()
                                   ?.GetPaymentMethodParameterRows()
                                   ?.ToDictionary(row => row.Parameter, row => row.Value);
        }

        private void GetParametersValues()
        {
            if (_settings != null)
            {
                PaymentAction = GetParameterValue(PaymentActionParameter);
                SandBox = GetParameterValue(SandBoxParameter);
                ClientId = GetParameterValue(ClientIdParameter);
                Secret = GetParameterValue(SecretParameter);
            }
        }

        private string GetParameterValue(string parameterName)
        {
            string parameterValue;
            return _settings.TryGetValue(parameterName, out parameterValue) ? parameterValue : string.Empty;
        }

        private Guid GetPaymentMethodId()
        {
            var paymentMethodRow = _paymentMethodDto.PaymentMethod.Rows[0] as PaymentMethodDto.PaymentMethodRow;
            return paymentMethodRow != null ? paymentMethodRow.PaymentMethodId : Guid.Empty;
        }
    }
}
