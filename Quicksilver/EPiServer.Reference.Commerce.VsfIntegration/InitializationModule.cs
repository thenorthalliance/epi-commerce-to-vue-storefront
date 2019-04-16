using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Reference.Commerce.VsfIntegration.Adapter;
using EPiServer.Reference.Commerce.VsfIntegration.Mapping;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.ApiBridge.Authorization.Claims;
using EPiServer.Vsf.ApiBridge.Authorization.Model;
using EPiServer.Vsf.ApiBridge.Authorization.Token;
using EPiServer.Vsf.ApiBridge.Endpoints;
using EPiServer.Vsf.ApiBridge.Utils;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using EPiServer.Vsf.Core.Exporting;
using EPiServer.Vsf.DataExport.Exporting;
using EPiServer.Vsf.DataExport.Mapping;
using EPiServer.Vsf.DataExport.Utils.Epi;

namespace EPiServer.Reference.Commerce.VsfIntegration
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class InitializationModule : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {}

        public void Uninitialize(InitializationEngine context)
        {}

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<PriceService>();
            services.AddSingleton<InventoryService>();

            services.AddTransient<IExtractedContentHandler, ExtractedContentHandler<FasionVsfProduct>>();
            services.AddTransient<IContentExtractor, ContentExtractor>();
            services.AddTransient<IIndexingService, IndexingService<FasionVsfProduct>>();
            
            services.AddTransient<IProductMapper<FasionVsfProduct>, FasionProductMapper>();
            services.AddTransient<IAttributeMapper, AttributeMapper>();
            services.AddTransient<ICategoryMapper, CategoryMapper>();
            
            services.AddTransient<IUserAdapter<VsfUser>, QuickSilverUserAdapter>();
            services.AddTransient<ICartAdapter, QuickSilverCartAdapter>();
            services.AddTransient<IStockAdapter, QuickSilverStockAdapter>();

            services.AddTransient<IUserEndpoint, UserEndpoint<VsfUser>>();
            services.AddTransient<ICartEndpoint, CartEndpoint>();
            services.AddTransient<IStockEndpoint, StockEndpoint>();

            services.Add(typeof(IUserClaimsProvider<VsfUser>), typeof(UserClaimsProvider<VsfUser>), ServiceInstanceScope.Transient);
            
            if (!services.Contains(typeof(IUserTokenProvider)))
                services.Add(typeof(IUserTokenProvider), new JwtUserTokenProvider(new AuthTokenOptions
                {
                    Issuer = "test_issuer",
                    Audience = "http://localhost:50244",
                    SecurityKey = "alamakotaalamakotaalamakotaalamakota".ToSymmetricSecurityKey()
                }, new MemoryRefreshTokenRepository()));
        }
    }
}
