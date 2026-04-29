using DevExpress.AspNetCore;
using DevExpress.CodeParser;
using DevExpress.Web.Office;
using HR_Templates.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

//builder.Services.AddDevExpressControls();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDevExpressControls(options =>
{
    options.AddSpreadsheet(
        spreadsheetOptions =>
        {
            spreadsheetOptions
                .AddAutoSaving(
                    (IDocumentInfo documentInfo, DocumentSavingEventArgs e) =>
                    {
                        byte[] documentContent = documentInfo.SaveCopy();
                        e.Handled = true;
                    }
                );
        }
    );
});


// ...

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        string rawSecret = "esta es la clave para firmar el token y asi validar su integridad en la aplicaci�n cliente";

        // Replicamos EXACTAMENTE lo que pusiste en el generador:
        // System.Text.Encoding.UTF8.GetBytes(signingsecret.ToBase64())
        var secretWithBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawSecret));
        var finalKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretWithBase64));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = finalKey,

            ValidateIssuer = true,
            ValidIssuer = "VD.Api",

            ValidateAudience = true,
            ValidAudience = "pw.delmoci.vd",

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Leemos el token de la URL
                var accessToken = context.Request.Query["access_token"].ToString();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    // Si el token viene con la palabra "Bearer ", se la quitamos
                    if (accessToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Token = accessToken.Substring(7);
                    }
                    else
                    {
                        context.Token = accessToken;
                    }
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                // Esto te ayudará a ver en la consola de VS por qué falló exactamente
                System.Diagnostics.Debug.WriteLine("Fallo de autenticación: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });


builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxResponseBufferSize = null;
    options.Limits.Http2.MaxStreamsPerConnection = 100;
    options.AllowSynchronousIO = true;
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();

// Example for Program.cs in ASP.NET Core
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".ftl"] = "text/plain"; // Map .ftl as plain text

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});


app.UseDevExpressControls();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Formatos");
    return Task.CompletedTask;
});
app.Run();
