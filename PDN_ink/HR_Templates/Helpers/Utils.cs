using MessagePack;
using System.Net;
using System.Text;

namespace HR_Templates.Helpers
{
    public static class Utils
    {
        public static string FromBase64(this string b64)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(b64));
            }
            catch (Exception)
            {
                return "InvalidString";
            }
        }

        public static string ToBase64(this string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }


        private static ISession Session => AppHttpContext.Current.Session;
        public static T GetVar<T>(string varName)
        {
            if (Session.TryGetValue(varName, out byte[] binaryData))
            {
                return MessagePackSerializer.Deserialize<T>(binaryData);
            }
            return default;
        }

        public static void SetVar(string varName, object value)
        {
            Session.TryGetValue(varName, out var arrBytes);

            if (value == null)
            {
                if (arrBytes != null)
                {
                    Session.Remove(varName);
                }
                return;
            }

            var binaryData = MessagePackSerializer.Serialize(value);
            Session.Set(varName, binaryData);
        }

        public static Uri GetUri(HttpRequest request, bool full)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host
            };

            if (request.Host.Port.HasValue)
                uriBuilder.Port = request.Host.Port.Value;

            if (!full) return uriBuilder.Uri;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();

            return uriBuilder.Uri;
        }

        public static bool IsInternal(string testIp)
        {
            try
            {
                if (string.IsNullOrEmpty(testIp)) return false;

                if (testIp == "::1") return true;

                var ip = IPAddress.Parse(testIp).GetAddressBytes();

                switch (ip[0])
                {
                    case 10:
                    case 127:
                        return true;
                    case 172:
                        return ip[1] >= 16 && ip[1] < 32;
                    case 192:
                        return ip[1] == 168;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string RemoveSpecialCharacters(this string strvalue)
        {
            string strword = strvalue;
            if (!string.IsNullOrEmpty(strword))
            {
                strword = strword.Replace("Ñ", "N");
                strword = strword.Replace("Á", "A");
                strword = strword.Replace("É", "E");
                strword = strword.Replace("Í", "I");
                strword = strword.Replace("Ó", "O");
                strword = strword.Replace("Ú", "U");
                strword = strword.Replace("Ü", "U");
                strword = strword.Replace("á", "A");
                strword = strword.Replace("é", "E");
                strword = strword.Replace("í", "I");
                strword = strword.Replace("ó", "O");
                strword = strword.Replace("ú", "U");
                strword = strword.Replace("ü", "U");
                strword = strword.Replace("´", " ");
                strword = strword.Replace("`", " ");
                strword = strword.Replace("¥", "N");
                strword = strword.Replace("°", "O");
                strword = strword.Replace("à", "a");
                strword = strword.Replace("è", "e");
                strword = strword.Replace("ì", "i");
                strword = strword.Replace("ò", "o");
                strword = strword.Replace("ù", "u");
                strword = strword.Replace("À", "A");
                strword = strword.Replace("È", "E");
                strword = strword.Replace("Ì", "I");
                strword = strword.Replace("Ò", "O");
                strword = strword.Replace("Ù", "U");
                strword = strword.Replace("&", "");
                strword = strword.Replace("?", "");
                strword = strword.Replace(",", "");
            }
            return strword;
        }


    }
}