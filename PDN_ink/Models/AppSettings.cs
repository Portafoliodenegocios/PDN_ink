namespace Pdnink_Coremvc.Models
{
    public class AppSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SigningSecret { get; set; }
        public string EncryptingSecret { get; set; }
        public string RecaptchaSecret { get; set; }
        public string SecUrl { get; set; }
        public string TokenKey { get; set; }
        public string SecurityServiceSecret { get; set; }
        public string SecurityServiceEncryptSecret { get; set; }
        public string SecurityServiceIssuer { get; set; }
        public string AppGuid { get; set; }
        public string Sin { get; set; }
        public string RecaptchaV3SiteKey { get; set; }

    }
}
