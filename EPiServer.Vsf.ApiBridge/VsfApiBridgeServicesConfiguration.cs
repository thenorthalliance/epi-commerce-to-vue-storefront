using System;
using EPiServer.Vsf.ApiBridge.Authorization.Claims;
using EPiServer.Vsf.ApiBridge.Endpoints;
using EPiServer.Vsf.Core.ApiBridge.Model;

namespace EPiServer.Vsf.ApiBridge
{
    public class VsfApiBridgeServicesConfiguration<TUser> where TUser : UserModel
    {
        public VsfApiBridgeServicesConfiguration(Type userAdapter, Type cartAdapter, Type stockAdapter)
        {
            UserAdapter = userAdapter;
            CartAdapter = cartAdapter;
            StockAdapter = stockAdapter;
        }

        public Type UserAdapter { get; set; }
        public Type CartAdapter { get; set; }
        public Type StockAdapter { get; set; }
        

        public Type UserEndpoint { get; set; } = typeof(UserEndpoint<TUser>);
        public Type CartEndpoint { get; set; } = typeof(CartEndpoint);
        public Type StockEndpoint { get; set; } = typeof(StockEndpoint);

        public Type UserClaimsProvider { get; set; } = typeof(UserClaimsProvider<TUser>);
    }
}