How to setup:

copy mw specific paypal folder (based on develo design: https://github.com/DivanteLtd/vue-storefront/blob/master/docs/guide/integrations/paypal-payments.md) to:

src/modules

copy default-mw folder theme to:

src/themes

https://github.com/DivanteLtd/vue-storefront/blob/master/docs/guide/core-themes/themes.md

add to your local.config:

"paypal": {
  "env": "sandbox",
  "endpoint": {
	"create": "http://localhost:50244/vsbridge/order/payments/paypal/create",
	"execute": "http://localhost:50244/vsbridge/order/payments/paypal/execute"
  },
  "methodId": "{paymentMethodId generated after installing and configuring in epi commerce manager}"
},

Add at the end of ./src/modules/index.ts
	
import { GoogleAnalytics } from './google-analytics';
import { Paypal } from './paypal';

export const registerModules: VueStorefrontModule[] = [
  GoogleAnalytics,
  Paypal //<- this one
]

Configure PayPal in EpiServer:

Setting up the PayPal payment provider in Commerce Manager

1. Go to Administration > Order System > Payments > English (United States). The last option is the language in which you want to make the PayPal payment available.

2. Click New to create new payment method.

For System keyword, select PayPal, the name of the folder created during deployment.

For Class Name, select  EPiServer.Vsf.Core.Payments.PayPalPaymentGateway.
For Payment Class, select  EPiServer.Vsf.Core.Payments.PayPal.PayPalPayment.

Screen sample
3. Click OK to save the PayPal payment method
4. Open the PayPal payment method for additional editing
5. Go to the Parameters tab and enter the following

ClientId - Enter PayPal ClientId from developer site
Secret - Enter PayPal Secret from developer site
Check sandbox environment
Payment action - Authorize

6. Open the Markets tab and add the expected markets for this payment.

7. In Commerce Manager, go to Administration > Order System > Meta Classes.

8. Click Import/Export, select Import MetaData.

9.  To populate the MetaData import screen. Click Add New File. Click Choose File and select the PayPal/PayPalPaymentMetaClass.xml file. Click Upload File. Click Save the File.

10. Select the PayPal/PayPalPaymentMetaClass.xml in the MetaData import screen. Click Start Import.

Enter the generated payment method id into local.json config

"paypal": {
  "env": "sandbox",
  "endpoint": {
	"create": "http://localhost:50244/vsbridge/order/payments/paypal/create",
	"execute": "http://localhost:50244/vsbridge/order/payments/paypal/execute"
  },
  "methodId": "{paymentMethodId here}"
},