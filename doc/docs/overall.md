# EPiServer Commerce - VueStorefront integration demo

This is a demo implementation showing how to use EpiServer Commerce as a backend platform for [Vue Storefront](https://github.com/DivanteLtd/vue-storefront). 

This demo is based on the [Episerver Commerce Quicksilver sample site](https://github.com/episerver/Quicksilver)


The main goal of this project is to provide a set of algorithms and data structures that will help with the integration of EPiServer Commerce and VueStoreFront. Provided solution consists of two parts: ApiBridge and DataExporter.
1. ApiBridge - implements web api required by VSF
2. DataExporter - reading, converting and exporting data models from EpiServer to Elasticsearch (where it can be accessed by VSF)

Although the example implementation in based on Quicksilver, both ApiBridge and DataExporter have ben developed with customization in mind. 

## Project setup
1. Setup Quicksilver demo - you can find an instruction in a [Quicksilver directory](https://github.com/makingwaves/epi-commerce-to-vue-storefront/tree/master/Quicksilver)
2. [Install VueStorefront](https://docs.vuestorefront.io/guide/installation/windows.html)

### Changes introduced to Quicksilver demo
1. <b>EPiServer.Reference.Commerce.VsfIntegration</b> project has been added. This project contains examples of custom model mappers (e.g. FashionProductMapper) and service adapters (e.g. QuickSilverUserAdapter). 
2. <b>EPiServer.Reference.Commerce.Site->Features->Product->Models</b> has been moved to 
<b>EPiServer.Reference.Commerce.Shared->Models->Products</b>

## How to run this demo
### 1. Configuration 
#### 1. vue-storefront local.config - changes
An working example can be found [HERE] <--- TODO <br>
List of customized properties with example values:
```
{
    "elasticsearch": {
      "index": "epi-catalog",
      ...
    },
    "cart": {
      "setCustomProductOptions": false,
      "setConfigurableProductOptions": false,
      "create_endpoint": "http://localhost:50244/vsbridge/Cart/create?token={{token}}",
      "updateitem_endpoint": "http://localhost:50244/vsbridge/Cart/update?token={{token}}&cartId={{cartId}}",
      "deleteitem_endpoint": "http://localhost:50244/vsbridge/Cart/delete?token={{token}}&cartId={{cartId}}",
      "pull_endpoint": "http://localhost:50244/vsbridge/Cart/pull?token={{token}}&cartId={{cartId}}",
      "totals_endpoint": "http://localhost:50244/vsbridge/Cart/totals?token={{token}}&cartId={{cartId}}",
      "paymentmethods_endpoint": "http://localhost:50244/vsbridge/Cart/payment-methods?token={{token}}&cartId={{cartId}}",
      "shippingmethods_endpoint": "http://localhost:50244/vsbridge/Cart/shipping-methods?token={{token}}&cartId={{cartId}}",
      "shippinginfo_endpoint": "http://localhost:50244/vsbridge/Cart/shipping-information?token={{token}}&cartId={{cartId}}",
      "collecttotals_endpoint": "http://localhost:50244/vsbridge/Cart/collect-totals?token={{token}}&cartId={{cartId}}",
      "deletecoupon_endpoint": "http://localhost:50244/vsbridge/Cart/delete-coupon?token={{token}}&cartId={{cartId}}",
      "applycoupon_endpoint": "http://localhost:50244/vsbridge/Cart/apply-coupon?token={{token}}&cartId={{cartId}}&coupon={{coupon}}",
      ...
    },
    "users": {
      "endpoint": "http://localhost:50244/vsbridge/user",
      "history_endpoint": "http://localhost:50244/vsbridge/user/order-history?token={{token}}",
      "resetPassword_endpoint": "http://localhost:50244/vsbridge/user/reset-password",
      "changePassword_endpoint": "http://localhost:50244/vsbridge/user/change-password?token={{token}}",
      "login_endpoint": "http://localhost:50244/vsbridge/user/login",
      "create_endpoint": "http://localhost:50244/vsbridge/user/create",
      "me_endpoint": "http://localhost:50244/vsbridge/user/me?token={{token}}",
      "refresh_endpoint": "http://localhost:50244/vsbridge/user/refresh",
      ...
    },
    "stock": {
      "endpoint": "http://localhost:50244/vsbridge/stock",
      ...
    },
    "images": {
      "useExactUrlsNoProxy": true,
      "baseUrl": "http://localhost:50244/vsbridge/image/",
      ...
    },
    ...
}
```

#### 2. vue-storefront-api local.config - changes
An working example file can be found [HERE] <--- TODO <br>
List of customized properties with example values:
```
{
  "elasticsearch": {
    "indices": [ "epi-catalog" ],
    ...
  },
  ...
}
```

#### 3. EPiServer.Reference.Commerce.Site - web.config
This repository contains properly configured project. <br>
List of customized properties with example values:
```
<configuration>
    <configSections>
        ...
        <section name="vsf.export" type="EPiServer.Vsf.DataExport.Configuration.VsfExporterConfiguration, EPiServer.Vsf.DataExport"/>
        <section name="vsf.apiBridge" type="EPiServer.Vsf.ApiBridge.VsfApiBridgeConfiguration, EPiServer.Vsf.ApiBridge"/>
    </configSections>
    ...
    <vsf.export 
        elasticServerUrls="http://127.0.0.1:9200" 
        indexAliasName="epi-catalog" 
        bulkIndexBatchSize="100" />

    <vsf.apiBridge 
        auth.signingKey="alamakotaalamakotaalamakotaalamakota"
        auth.issuer="quicksilver_issuer"
        auth.audience="quicksilver_audience"
        auth.accessTokenExpiration="60" />
```

### 2. Data Export
Quicksilver to VSF export procedure.<br>
1. Before exporting make sure that:<br>
1.1 VSF (vsf and vsf-api) is up and running<br>
1.2 Quicksilver example is up and running
2. Navigate to EPiServer CMS Admin panel ->  Scheduled Jobs -> Export to Vue Storefront
3. Start Manually

After the job has finished, refresh VSF application - all product should be visible.

## Solution structure
Quicksilver example default projects:<br>
<b>EPiServer.Reference.Commerce.Site</b><br>
<b>EPiServer.Reference.Commerce.Shared</b><br>
<b>EPiServer.Reference.Commerce.Manager</b><br>

Actual Demo projects:<br>
<b>EPiServer.Vsf.Core</b> - set of core models and interfaces<br>
<b>EPiServer.Vsf.ApiBridge</b> - implementation of VSF web api bridge<br>
<b>EPiServer.Vsf.DataExport</b> - implementation of exporting and model mapping algorithms <br>
<b>EPiServer.Reference.Commerce.VsfIntegration</b> - quick silver customization. This project contains examples of custom model mappers (e.g. FashionProductMapper) and service adapters (e.g. QuickSilverUserAdapter).<br>

![Project dependency diagram](project_dependency.png)

## Todo
## Known bugs

