using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public abstract class VsfResponse<T>
    {
        public VsfResponse(int code, T result)
        {
            this.Code = code;
            this.Result = result;
        }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("result")]
        public T Result { get; set; }
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
            [JsonProperty("refreshToken")]
            public string RefreshToken { get; set; }
        }

        public LoginResponse(string token, string refreshToken) : base(token)
        {
            Meta = new LoginMetadata
            {
                RefreshToken = refreshToken
            };
        }

        [JsonProperty("meta")]
        public LoginMetadata Meta { get; set; }
    }

    public class RefreshTokenResponse : VsfSuccessResponse<string>
    {
        public RefreshTokenResponse(string result) : base(result)
        {}
    }
}