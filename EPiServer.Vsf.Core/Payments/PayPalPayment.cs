using System;
using System.Runtime.Serialization;
using Mediachase.Commerce.Orders;
using Mediachase.MetaDataPlus.Configurator;

namespace EPiServer.Vsf.Core.Payments
{
    /// <summary>
    /// Represents Payment class for PayPal.
    /// </summary>
    [Serializable]
    public class PayPalPayment : Mediachase.Commerce.Orders.Payment
    {
        private static MetaClass _metaClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayPalPayment"/> class.
        /// </summary>
        public PayPalPayment()
            : base(PayPalPaymentMetaClass)
        {
            PaymentType = PaymentType.Other;
            ImplementationClass = GetType().AssemblyQualifiedName; // need to have assembly name in order to retrieve the correct type in ClassInfo
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayPalPayment"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected PayPalPayment(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        {
            PaymentType = PaymentType.Other;
            ImplementationClass = GetType().AssemblyQualifiedName; // need to have assembly name in order to retrieve the correct type in ClassInfo
        }

        /// <summary>
        /// Gets the credit card payment meta class.
        /// </summary>
        /// <value>The credit card payment meta class.</value>
        public static MetaClass PayPalPaymentMetaClass => _metaClass ?? (_metaClass = MetaClass.Load(OrderContext.MetaDataContext, "PayPalPayment"));
        
        /// <summary>
        /// Represents the PayPal order number
        /// </summary>
        public string PayPalOrderNumber
        {
            get { return GetString(PayPalPaymentGateway.PayPalOrderNumberPropertyName); }
            set { this[PayPalPaymentGateway.PayPalOrderNumberPropertyName] = value; }
        }

        /// <summary>
        /// Represents the PayPal exp token
        /// </summary>
        public string PayPalExpToken
        {
            get { return GetString(PayPalPaymentGateway.PayPalExpTokenPropertyName); }
            set { this[PayPalPaymentGateway.PayPalExpTokenPropertyName] = value; }
        }    
    }
}