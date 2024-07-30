namespace StockApp.Core.Application.Dtos.Token
{
    public class JwtResponse
    {
        public bool HasError { get; set; }
        public string? ErrorDescription { get; set; }
    }
}
