using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pdnink.Middleware;
using Pdnink_Coremvc.Helpers;
using Pdnink_Coremvc.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using DevExtreme.AspNet.Mvc;
using DevExpress.AspNetCore;




var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(co =>
    {
        co.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
    });
});

var appSettingsSection = builder.Configuration.GetSection("AppSettings");


builder.Services.Configure<AppSettings>(appSettingsSection);


// Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
//builder.Services.AddDevExpressControls();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession( options =>
         { 
                options.Cookie.Name = ".pdnink.Session";
             double minutes = builder.Configuration.GetValue<double>("AppSettings:SessionExpireMinutes");

             options.IdleTimeout = TimeSpan.FromMinutes(minutes);
             options.Cookie.HttpOnly = true;
             options.Cookie.IsEssential = true;
         });
builder.Services.AddScoped<Pdnink_Coremvc.Helpers.Constants>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = (context) =>
                       {

                           return Task.CompletedTask;
                       }
                   };

                   options.SaveToken = true;
                   options.UseSecurityTokenValidators = true;

                   string signingsecret = builder.Configuration.GetValue<string>("AppSettings:SigningSecret");

                   var encryptingBytes = System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("AppSettings:EncryptingSecret").ToBase64());
                   Array.Resize(ref encryptingBytes, 32);
                   var encryptingKey = new SymmetricSecurityKey(encryptingBytes);


                   options.TokenValidationParameters = new TokenValidationParameters
                   {

                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,                       

                       IssuerSigningKey = new SymmetricSecurityKey(
                           System.Text.Encoding.UTF8.GetBytes(signingsecret.ToBase64())),

                       TokenDecryptionKey = encryptingKey,
                       ValidIssuer = builder.Configuration.GetValue<string>("AppSettings:Issuer"),
                       ValidAudience = builder.Configuration.GetValue<string>("AppSettings:Audience"),
                       ClockSkew = TimeSpan.FromMinutes(5),
                       RequireSignedTokens = false,
                   };

                   options.TokenValidationParameters.SignatureValidator = (token, _) => new JwtSecurityToken(token);

               });




var app = builder.Build();


//Rotativa.AspNetCore.RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseMiddleware<TokenHeader>();

app.UseAuthentication();   

app.UseAuthorization();

app.MapStaticAssets();

//app.UseDevExpressControls();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
