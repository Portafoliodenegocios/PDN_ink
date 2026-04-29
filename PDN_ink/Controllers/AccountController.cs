using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pdnink_Coremvc.Helpers;
using Pdnink_Coremvc.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Security.Claims;
using System.Text;


namespace Pdnink_Coremvc.Controllers
{
    public class AccountController : Controller
    {
        private AppSettings AppSettings { get; }
        private readonly IServiceProvider _serviceProvider;
        private readonly Constants _constants;
        private readonly IDataProtector _protector;

        public AccountController(IOptions<AppSettings> settings, IServiceProvider serviceProvider, Constants constants, IDataProtectionProvider provider)
        {
            AppSettings = settings.Value;
            //ConfigureService();
            _serviceProvider = serviceProvider;
            _constants = constants;
            _protector = provider.CreateProtector("ResetPassword");
        }

        public IActionResult Login()
        {
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home");            

            //AppHttpContext.Current.Session.Clear();
            ViewBag.Token = AppSettings.TokenKey;
            ViewBag.AppId = AppSettings.AppGuid;
            ViewBag.Sin = AppSettings.Sin;
            ViewBag.Secure = $"{AppSettings.SecUrl}/Account";
            ViewBag.RecaptchaV3SiteKey = AppSettings.RecaptchaV3SiteKey;
            //HttpContext.Session.SetInt32(Sessionlogin, 0);
            return View();
        }

        public IActionResult forgotpassword()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult WebLogin() => RedirectToAction("Login");

        [HttpPost]
        public async Task<JsonResult> WebLogin(Login l)
        {
            var result = await SignIn(l);

            if (!result.Success)
                result.ClearData = AesUtils.Decrypt(result.Data);
            else
            {
                var res = await GenerateToken(result, true, l.Username);
                if (!String.IsNullOrEmpty(result.Mistake))
                {
                    result.Success = false;
                    result.ClearData = result.Mistake;
                }
                else
                {
                    var response = res.Value as ApiResponse;
                    if (response == null) return Json(result);
                    result.Success = response.Success;
                    result.ClearData = AesUtils.Decrypt(response.Data).FromBase64();
                    result.Token = _constants.Token?.Replace("Bearer ", "").Trim();

                }
            }


            return Json(result);


        }

        private async Task<ApiResponse> SignIn(Login l)
        {
            var loginResponse = new ApiResponse();

            HttpContext.Session.SetString("usrName", l.Username);

            try
            {
                var data = JsonConvert.SerializeObject(l);

                using (var client = new HttpClient())
                {
                    var content = new StringContent(data, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(AppSettings.SecUrl + "/Auth/Login", content);

                    var result = await response.Content.ReadAsStringAsync();

                    if (IsValidJson(result))
                        loginResponse = ApiResponse.FromJson(result);
                    else
                        loginResponse.Data = "Respuesta invalida del servidor de autenticación";
                }
            }
            catch (Exception e)
            {
                loginResponse.Data = e.Message;
            }

            return loginResponse;
        }

      
        private static bool IsValidJson(string json)
        {
            try
            {
                JsonConvert.DeserializeObject(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        ////private async Task<JsonResult> GenerateToken(ApiResponse response, bool setToken, string userLogin)
        ////{
        ////    try
        ////    {
        ////        var responseToken = AesUtils.Decrypt(response.Data);

        ////        if (string.IsNullOrEmpty(responseToken))
        ////        {
        ////            response.Mistake = "Petición no valida";
        ////            return StringResponse(response.Mistake);
        ////        }

        ////        ClaimsPrincipal tokenClaims = new ClaimsPrincipal();
        ////        string validationError = string.Empty;

        ////        if (!IsValidToken(responseToken, ref tokenClaims, ref validationError))
        ////        {
        ////            response.Mistake = $"El token de autenticación no es valido. {validationError}";
        ////            return StringResponse(response.Mistake);
        ////        }

        ////        if (tokenClaims.Claims.FirstOrDefault(x => x.Type == "username").Value.ToLower() != userLogin.ToLower())
        ////        {
        ////            response.Mistake = $"Se ha detectado un desfase de token";
        ////            return StringResponse(response.Mistake);
        ////        }
        ////        var handler = new JwtSecurityTokenHandler();

        ////        var userGuidValue = tokenClaims.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;

        ////        Guid.TryParse(userGuidValue, out var userGuid);

        ////        if (userGuid.Equals(Guid.Empty))
        ////        {
        ////            response.Mistake = "Los datos del token de autenticación no son validos";
        ////            return StringResponse(response.Mistake);
        ////        }

        ////        var claims = new List<Claim>();

        ////        claims.AddRange(tokenClaims.Claims.Select(claim => new Claim(claim.Type, claim.Value)));
        ////        claims.Remove(claims.Find(x => x.Type.Equals("iss")));
        ////        claims.Remove(claims.Find(x => x.Type.Equals("UserId")));
        ////        claims.Remove(claims.Find(x => x.Type.Equals("aud")));
        ////        //claims.Add(new Claim("UserId", user.User_Id.ToString()));
        ////        claims.Add(new Claim("UserGuid", userGuidValue));

        ////        var LstPermissions = await GetPermission(userGuid);

        ////        var encryptingBytes = Encoding.UTF8.GetBytes(AppSettings.EncryptingSecret.ToBase64());
        ////        Array.Resize(ref encryptingBytes, 32);

        ////        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SigningSecret.ToBase64()));
        ////        var encryptingKey = new SymmetricSecurityKey(encryptingBytes);

        ////        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);
        ////        var encryptingCredentials = new EncryptingCredentials(encryptingKey, SecurityAlgorithms.Aes256KW,
        ////            SecurityAlgorithms.Aes256CbcHmacSha512);

        ////        var jwtToken = handler.CreateJwtSecurityToken(
        ////            AppSettings.Issuer,
        ////            AppSettings.Audience,
        ////            new ClaimsIdentity(claims),
        ////            DateTime.Now,
        ////            DateTime.Now.AddDays(1),
        ////            DateTime.Now,
        ////            signingCredentials,
        ////            encryptingCredentials
        ////        );

        ////        var tokenString = handler.WriteToken(jwtToken);

        ////        var tempToken = $"Bearer {tokenString}";

        ////        if (setToken)
        ////            _constants.Token = tempToken;
        ////        else
        ////            _constants.TempToken = tempToken;

        ////        _constants.UserGuid = userGuid;

        ////        var data = Json(new
        ////        {
        ////            token = tokenString,
        ////            //role = user.User_Login,
        ////            //email = user.Email,
        ////            userId = userGuidValue //User_Id
        ////            //displayName = $"{user.First_Name} {user.Last_Name}"
        ////        });
        ////        return ObjectResponse(data.Value, 1, true);
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        response.Mistake = e.Message.Contains("See the inner exception for details")
        ////            ? e.InnerException?.Message ?? e.Message
        ////            : e.Message;
        ////        return StringResponse(response.Mistake);
        ////    }
        ////}
        /// <summary>
        /// 
        /// 
        private async Task<JsonResult> GenerateToken(ApiResponse response, bool setToken, string userLogin)
        {
            try
            {
                var responseToken = AesUtils.Decrypt(response.Data);

                if (string.IsNullOrEmpty(responseToken))
                {
                    response.Mistake = "Petición no valida";
                    return StringResponse(response.Mistake);
                }

                ClaimsPrincipal tokenClaims = new ClaimsPrincipal();
                string validationError = string.Empty;

                if (!IsValidToken(responseToken, ref tokenClaims, ref validationError))
                {
                    response.Mistake = $"El token de autenticación no es valido. {validationError}";
                    return StringResponse(response.Mistake);
                }

                if (tokenClaims.Claims.FirstOrDefault(x => x.Type == "username")?.Value.ToLower() != userLogin.ToLower())
                {
                    response.Mistake = $"Se ha detectado un desfase de token";
                    return StringResponse(response.Mistake);
                }

                var handler = new JwtSecurityTokenHandler();

                var userGuidValue = tokenClaims.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

                if (!Guid.TryParse(userGuidValue, out var userGuid) || userGuid == Guid.Empty)
                {
                    response.Mistake = "Los datos del token de autenticación no son validos";
                    return StringResponse(response.Mistake);
                }

                var namefull = tokenClaims.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;


                var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, userGuidValue),
                        new Claim("username", userLogin),
                        new Claim("UserGuid", userGuidValue),
                        new Claim("FullName", namefull)
                    };

               
                var signingKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(AppSettings.SigningSecret.ToBase64())
                );

                var signingCredentials = new SigningCredentials(
                    signingKey,
                    SecurityAlgorithms.HmacSha512
                );

                var jwtToken = handler.CreateJwtSecurityToken(
                    issuer: AppSettings.Issuer,
                    audience: AppSettings.Audience,
                    subject: new ClaimsIdentity(claims),
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: signingCredentials
                );

                var tokenString = handler.WriteToken(jwtToken);

                var tempToken = $"Bearer {tokenString}";

                if (setToken)
                    _constants.Token = tempToken;
                else
                    _constants.TempToken = tempToken;

                _constants.UserGuid = userGuid;

                return ObjectResponse(new
                {
                    token = tokenString,
                    userId = userGuidValue
                }, 1, true);
            }
            catch (Exception e)
            {
                response.Mistake = e.Message.Contains("See the inner exception for details")
                    ? e.InnerException?.Message ?? e.Message
                    : e.Message;

                return StringResponse(response.Mistake);
            }
        }

        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="retry"></param>
        /// <returns></returns>
        private async Task<List<Permision>> GetPermission(Guid UserGuid, int retry = 0)
        {
            if (retry == 5)
                return null;

            List<Permision> permissionList = new List<Permision>();
            string result;
            Login l = new Login();
            List<Guid> ids = new List<Guid>();

            ids.Add(new Guid("993927AE-2844-4863-8733-E1DB29D823B1"));

            try
            {
                var permissions = HttpContext.Session.GetString("Permissions");

                if (string.IsNullOrEmpty(permissions))
                {
                    l.Username = HttpContext.Session.GetString("usrName");
                    l.Lstapps = ids;
                    l.UserId = UserGuid;

                    var data = JsonConvert.SerializeObject(l);

                    using (var client = new HttpClient())
                    {
                        var content = new StringContent(data, Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(
                            AppSettings.SecUrl + "/Auth/GetPermissionbyGuids",
                            content
                        );

                        result = await response.Content.ReadAsStringAsync();
                    }

                    if (IsValidJson(result))
                    {
                        var loginResponse = ApiResponse.FromJson(result);

                        if (!loginResponse.Success)
                            return new List<Permision>();

                        if (string.IsNullOrEmpty(loginResponse.UserName) || loginResponse.UserName != l.Username)
                        {
                            await Task.Delay(new Random().Next(120, 500));
                            return await GetPermission(UserGuid, retry + 1);
                        }

                        var permisions = AesUtils.Decrypt(loginResponse.Data).FromBase64();
                        permissionList = JsonConvert.DeserializeObject<List<Permision>>(permisions);

                        HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(permissionList));
                    }
                }
                else
                {
                    permissionList = JsonConvert.DeserializeObject<List<Permision>>(permissions);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return permissionList;
        }
        //private List<Permision> GetPermission(Guid UserGuid, int retry = 0)
        //{
        //    if (retry == 5)
        //        return null;

        //    List<Permision> permissionList = new List<Permision>();
        //    string result;
        //    Login l = new Login();
        //    List<Guid> ids = new List<Guid>();

        //    ids.Add(new Guid("993927AE-2844-4863-8733-E1DB29D823B1"));
        //    try
        //    {
        //        var permissions = HttpContext.Session.GetString("Permissions");
        //        if (string.IsNullOrEmpty(permissions))
        //        {
        //            l.Username = HttpContext.Session.GetString("usrName");
        //            l.Lstapps = ids;
        //            l.UserId = UserGuid;
        //            var data = JsonConvert.SerializeObject(l);

        //            var httpRequest = WebRequest.Create(AppSettings.SecUrl + "/Auth/GetPermissionbyGuids"); //"https://serdigital.pdn1.com.mx/Api/Auth/Authenticate");
        //            httpRequest.Method = "POST";
        //            httpRequest.ContentType = "application/json";

        //            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
        //            streamWriter.Write(data);
        //            streamWriter.Close();

        //            var resp = httpRequest.GetResponse();
        //            var stream = resp.GetResponseStream();

        //            using (var sr = new StreamReader(stream))
        //                result = sr.ReadToEnd();

        //            if (IsValidJson(result))
        //            {
        //                var loginResponse = ApiResponse.FromJson(result);
        //                if (!loginResponse.Success)
        //                {
        //                    return new List<Permision>();
        //                }
        //                if (string.IsNullOrEmpty(loginResponse.UserName) || loginResponse.UserName != l.Username)
        //                {
        //                    Task.Delay(new Random().Next(120, 500));
        //                    return GetPermission(UserGuid, retry + 1);
        //                }

        //                var permisions = AesUtils.Decrypt(loginResponse.Data).FromBase64();
        //                permissionList = JsonConvert.DeserializeObject<List<Permision>>(permisions);
        //                HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(permissionList));

        //            }
        //        }
        //        else
        //        {
        //            permissionList = JsonConvert.DeserializeObject<List<Permision>>(permissions);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return permissionList;
        //}
        protected JsonResult StringResponse(string d, bool s = false)
        {
            return Json(new ApiResponse { Success = s, Data = d });
        }
        private bool IsValidToken(string authToken, ref ClaimsPrincipal tokenClaims, ref string error)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var encryptKeyString = AppSettings.SecurityServiceEncryptSecret.ToBase64();
                var encryptingBytes = Encoding.UTF8.GetBytes(encryptKeyString);
                Array.Resize(ref encryptingBytes, 32);

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SecurityServiceSecret.ToBase64()));
                var encryptingKey = new SymmetricSecurityKey(encryptingBytes);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(25),
                    ValidIssuer = AppSettings.SecurityServiceIssuer,
                    ValidAudience = AppSettings.AppGuid,
                    IssuerSigningKey = secretKey,
                    TokenDecryptionKey = encryptingKey
                };

                tokenClaims = tokenHandler.ValidateToken(authToken, validationParameters, out _);
                return true;
            }
            catch (InvalidOperationException ex)
            {
                error = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
        protected JsonResult ObjectResponse(object o, int? items, bool s = false)
        {
            try
            {
                var data = JsonConvert.SerializeObject(o).ToBase64();
                return Json(new ApiResponse { Success = s, Data = data, Items = items });
            }
            catch (Exception)
            {
                return StringResponse("Error al serializar información");
            }
        }

        public virtual async Task<JsonResult> ExistEmail(Login model)
        {
            var loginResponse = new ApiResponse();

            try
            {                           

               var data = JsonConvert.SerializeObject(new { email = model.email });
               using (var client = new HttpClient())
                {
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(
                            AppSettings.SecUrl + "/Auth/VerifyEmail", content);

                    var result = await response.Content.ReadAsStringAsync();

                    if (IsValidJson(result))
                        {
                            loginResponse.Success = true;
                            loginResponse.Data = "Correo valido";
                        }
                        else
                        {
                            loginResponse.Success = false;
                            loginResponse.Data = "Respuesta inválida del servidor";
                        }
               }
                
            }
            catch (Exception e)
            {
                loginResponse.Success = false;
                loginResponse.Data = e.Message;
            }

            return Json(new { exists = loginResponse.Success, message = loginResponse.Data });
        }

        [HttpPost]
        public JsonResult ForgotPassword(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return Json(new { success = false, message = "Ingrese el correo" });

                var data = $"{email}|{DateTime.UtcNow}";
                var token = _protector.Protect(data);
                var link = $"{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={Uri.EscapeDataString(token)}";
                //Sendemail(email, link);

                return Json(new { success = true, message = "Se envió el enlace" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    //    public void Sendemail(string email, string link)
    //    {
    //        var mail = new MailMessage();
    //        mail.To.Add(email);
    //        mail.Subject = "Recuperar acceso PDNINK";

    //        mail.Body = $@"
    //    <h2>Recuperación de acceso</h2>
    //    <p>Da clic en el siguiente enlace:</p>
    //    <a href='{link}'>Restablecer acceso</a>
    //    <br/><br/>
    //    <small>Si no solicitaste esto, ignora el mensaje</small>
    //";

    //        mail.IsBodyHtml = true;

    //        var smtp = new SmtpClient("smtp.office365.com", 587)
    //        {
    //            Credentials = new NetworkCredential("tuCorreo@empresa.com", "tuPassword"),
    //            EnableSsl = true
    //        };

    //        smtp.Send(mail);
    //    }

        public IActionResult ResetPassword(string token)
        {
            try
            {
                var data = _protector.Unprotect(token);
                var partes = data.Split('|');

                var email = partes[0];
                var fecha = DateTime.Parse(partes[1]);

                // ⏱ opcional expiración
                if (DateTime.UtcNow - fecha > TimeSpan.FromMinutes(30))
                    return View("TokenExpirado");

                ViewBag.Email = email;

                return View();
            }
            catch
            {
                return View("Error");
            }
        }


        [HttpGet]
        public ActionResult Reset_Password(string token, string email)
        {
            var model = new Pdnink.Models.ResetPassword
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(Pdnink.Models.ResetPassword  model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Aquí llamas a tu API o lógica
            // Ejemplo:
            // var result = await ResetPasswordAsync(model);

            return RedirectToAction("Login");
        }



       
        public IActionResult Originacion(string email)
        {
            //try
            //{
            //    if (string.IsNullOrEmpty(email))
            //        return Json(new { success = false, message = "Ingrese el correo" });

            //    var data = $"{email}|{DateTime.UtcNow}";
            //    var token = _protector.Protect(data);
            //    var link = $"{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={Uri.EscapeDataString(token)}";
            //    //Sendemail(email, link);

            //    return Json(new { success = true, message = "Se envió el enlace" });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { success = false, message = ex.Message });
            //}
            return View();
        }

    }
}
