using System;

namespace Pdnink_Coremvc.Helpers
{
    public class Constants
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Constants(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public string Token
        {
            get => Session.GetString("Token");
            set => Session.SetString("Token", value ?? string.Empty);
        }

        public string TempToken
        {
            get => Session.GetString("TempToken");
            set => Session.SetString("TempToken", value ?? string.Empty);
        }

        public Guid? UserGuid
        {
            get
            {
                var value = Session.GetString("UserGuid");
                return string.IsNullOrEmpty(value) ? null : Guid.Parse(value);
            }
            set
            {
                if (value.HasValue)
                    Session.SetString("UserGuid", value.Value.ToString());
                else
                    Session.Remove("UserGuid");
            }
        }
    }
}

    //public static class Constants
    //{

    //    internal static string Token
    //    {
    //        get => Utils.GetVar<dynamic>("Token");
    //        set => Utils.SetVar("Token", value);
    //    }

    //    internal static string TempToken
    //    {
    //        get => Utils.GetVar<dynamic>("TempToken");
    //        set => Utils.SetVar("TempToken", value);
    //    }

    //    internal static System.Guid? UserGuid
    //    {
    //        get => Utils.GetVar<System.Guid?>("UserGuid");
    //        set => Utils.SetVar("UserGuid", value);
    //    }
    //}

