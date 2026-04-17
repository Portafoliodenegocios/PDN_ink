using DevExpress.AspNetCore;
using DevExpress.Web.Office;
using Microsoft.AspNetCore.StaticFiles;

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
