namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class VsfResponse<T>
    {
        public VsfResponse(int code, T result)
        {
            this.code = code;
            this.result = result;
        }

        public int code { get; set; }
        public T result { get; set; }
    }

    public class VsfSuccessResponse<T> : VsfResponse<T>
    {
        public VsfSuccessResponse(T result) : base(200, result)
        {}
    }

    public class VsfErrorResponse : VsfResponse<string>
    {
        public VsfErrorResponse(string errorMsg) : base(500, errorMsg)
        {}
    }
    public class LoginResponse : VsfSuccessResponse<string>
    {
        public class LoginMetadata
        {
            public string refreshToken { get; set; }
        }

        public LoginResponse(string token, string refreshToken) : base(token)
        {
            meta = new LoginMetadata
            {
                refreshToken = refreshToken
            };
        }

        public LoginMetadata meta { get; set; }
    }

    public class RefreshTokenResponse : VsfSuccessResponse<string>
    {
        public RefreshTokenResponse(string result) : base(result)
        {}
    }
}