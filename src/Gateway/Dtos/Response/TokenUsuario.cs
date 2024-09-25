namespace Gateways.Dtos.Response
{
    public record TokenUsuario
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public DateTimeOffset Expiry { get; set; }
    }
}
