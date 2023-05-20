namespace IdentityManager.Models
{
    public class EnableAuthenticatorViewModel
    {
        public string Code { get; set; }
        public string Token { get; set; }
        public string QrCodeUrl { get; set; }
    }
}
