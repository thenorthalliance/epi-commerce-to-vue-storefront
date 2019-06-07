using System;
using System.Web.UI.WebControls;
using EPiServer.Vsf.Core.Payments;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Web.Console.Interfaces;

namespace EPiServer.Reference.Commerce.Manager.Apps.Order.Payments.Plugins.PayPal
{
    public partial class ConfigurePayment : System.Web.UI.UserControl, IGatewayControl
    {
        private PaymentMethodDto _paymentMethodDto;

        /// <summary>
        /// Gets or sets the validation group.
        /// </summary>
        /// <value>The validation group.</value>
        public string ValidationGroup { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurePayment"/> class.
        /// </summary>
        public ConfigurePayment()
        {
            ValidationGroup = string.Empty;
            _paymentMethodDto = null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// Loads the PaymentMethodDto object.
        /// </summary>
        /// <param name="dto">The PaymentMethodDto object.</param>
        public void LoadObject(object dto)
        {
            _paymentMethodDto = dto as PaymentMethodDto;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="dto">The dto.</param>
        public void SaveChanges(object dto)
        {
            if (!Visible)
            {
                return;
            }

            _paymentMethodDto = dto as PaymentMethodDto;
            if (_paymentMethodDto != null && _paymentMethodDto.PaymentMethodParameter != null)
            {
                var paymentMethodId = Guid.Empty;
                if (_paymentMethodDto.PaymentMethod.Count > 0)
                {
                    paymentMethodId = _paymentMethodDto.PaymentMethod[0].PaymentMethodId;
                }

                UpdateOrCreateParameter(PayPalConfiguration.ClientIdParameter, ClientId, paymentMethodId);
                UpdateOrCreateParameter(PayPalConfiguration.SecretParameter, Secret, paymentMethodId);

                //UpdateOrCreateParameter(PayPalConfiguration.UserParameter, APIUser, paymentMethodId);
                //UpdateOrCreateParameter(PayPalConfiguration.BusinessEmailParameter, BusinessEmail, paymentMethodId);
                //UpdateOrCreateParameter(PayPalConfiguration.PALParameter, PAL, paymentMethodId);
                //UpdateOrCreateParameter(PayPalConfiguration.PasswordParameter, Password, paymentMethodId);
                //UpdateOrCreateParameter(PayPalConfiguration.APISignatureParameter, Signature, paymentMethodId);
                //UpdateOrCreateParameter(PayPalConfiguration.ExpChkoutURLParameter, ExpChkoutURL, paymentMethodId);
                UpdateOrCreateParameter(PayPalConfiguration.SandBoxParameter, CheckBoxTest, paymentMethodId);
                //UpdateOrCreateParameter(PayPalConfiguration.AllowChangeAddressParameter, CheckBoxAllowChangeAddress, paymentMethodId);
                //UpdateOrCreateParameter(PayPalConfiguration.SkipConfirmPageParameter, CheckBoxSkipConfirmPage, paymentMethodId);
                //UpdateOrCreateParameter(PayPalConfiguration.AllowGuestParameter, CheckBoxGuestCheckout, paymentMethodId);
                UpdateOrCreateParameter(PayPalConfiguration.PaymentActionParameter, DropDownListPaymentAction, paymentMethodId);
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            if (_paymentMethodDto != null && _paymentMethodDto.PaymentMethodParameter != null)
            {
                BindParamterData(PayPalConfiguration.ClientIdParameter, ClientId);
                BindParamterData(PayPalConfiguration.SecretParameter, Secret);
                //BindParamterData(PayPalConfiguration.UserParameter, APIUser);
                //BindParamterData(PayPalConfiguration.PALParameter, PAL);
                //BindParamterData(PayPalConfiguration.PasswordParameter, Password);
                //BindParamterData(PayPalConfiguration.APISignatureParameter, Signature);
                BindParamterData(PayPalConfiguration.SandBoxParameter, CheckBoxTest);
                //BindParamterData(PayPalConfiguration.ExpChkoutURLParameter, ExpChkoutURL);
                //BindParamterData(PayPalConfiguration.BusinessEmailParameter, BusinessEmail);
                //BindParamterData(PayPalConfiguration.AllowChangeAddressParameter, CheckBoxAllowChangeAddress);
                //BindParamterData(PayPalConfiguration.SkipConfirmPageParameter, CheckBoxSkipConfirmPage);
                //BindParamterData(PayPalConfiguration.AllowGuestParameter, CheckBoxGuestCheckout);
                BindParamterData(PayPalConfiguration.PaymentActionParameter, DropDownListPaymentAction);
            }
            else
            {
                Visible = false;
            }
        }

        private void UpdateOrCreateParameter(string parameterName, TextBox parameterControl, Guid paymentMethodId)
        {
            var parameter = GetParameterByName(parameterName);
            if (parameter != null)
            {
                parameter.Value = parameterControl.Text;
            }
            else
            {
                var row = _paymentMethodDto.PaymentMethodParameter.NewPaymentMethodParameterRow();
                row.PaymentMethodId = paymentMethodId;
                row.Parameter = parameterName;
                row.Value = parameterControl.Text;
                _paymentMethodDto.PaymentMethodParameter.Rows.Add(row);
            }
        }

        private void UpdateOrCreateParameter(string parameterName, CheckBox parameterControl, Guid paymentMethodId)
        {
            var parameter = GetParameterByName(parameterName);
            var value = parameterControl.Checked ? "1" : "0";
            if (parameter != null)
            {
                parameter.Value = value;
            }
            else
            {
                var row = _paymentMethodDto.PaymentMethodParameter.NewPaymentMethodParameterRow();
                row.PaymentMethodId = paymentMethodId;
                row.Parameter = parameterName;
                row.Value = value;
                _paymentMethodDto.PaymentMethodParameter.Rows.Add(row);
            }
        }

        private void UpdateOrCreateParameter(string parameterName, DropDownList parameterControl, Guid paymentMethodId)
        {
            var parameter = GetParameterByName(parameterName);
            var value = parameterControl.SelectedValue;
            if (parameter != null)
            {
                parameter.Value = value;
            }
            else
            {
                var row = _paymentMethodDto.PaymentMethodParameter.NewPaymentMethodParameterRow();
                row.PaymentMethodId = paymentMethodId;
                row.Parameter = parameterName;
                row.Value = value;
                _paymentMethodDto.PaymentMethodParameter.Rows.Add(row);
            }
        }

        private void BindParamterData(string parameterName, TextBox parameterControl)
        {
            var parameterByName = GetParameterByName(parameterName);
            if (parameterByName != null)
            {
                parameterControl.Text = parameterByName.Value;
            }
        }

        private void BindParamterData(string parameterName, CheckBox parameterControl)
        {
            var parameterByName = GetParameterByName(parameterName);
            if (parameterByName != null)
            {
                parameterControl.Checked = parameterByName.Value == "1";
            }
        }

        private void BindParamterData(string parameterName, DropDownList parameterControl)
        {
            var parameterByName = GetParameterByName(parameterName);
            if (parameterByName != null)
            {
                parameterControl.SelectedValue = parameterByName.Value;
            }
        }

        private PaymentMethodDto.PaymentMethodParameterRow GetParameterByName(string name)
        {
            return PayPalConfiguration.GetParameterByName(_paymentMethodDto, name);
        }
    }
}