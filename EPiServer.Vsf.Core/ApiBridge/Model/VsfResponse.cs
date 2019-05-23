using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model
{
    public abstract class VsfResponse
    {}

    public abstract class VsfResponse<T> : VsfResponse
    {
        protected VsfResponse(int code, T result)
        {
            this.Code = code;
            this.Result = result;
        }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("result")]
        public T Result { get; set; }
    }

    public class VsfCustomResponse<T> : VsfResponse<T>
    {
        public VsfCustomResponse(int code, T result) : base(code, result){ }
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