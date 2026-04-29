using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class SignatureModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string DocumentName { get; set; }

    public void OnGet()
    {
        
    }

    public IActionResult OnPostSaveSignature([FromBody] SignatureRequest request)
    {
        if (string.IsNullOrEmpty(request.ImageData))
            return BadRequest("No signature received.");
        var base64Data = request.ImageData.Replace("data:image/png;base64,", "");
        var bytes = Convert.FromBase64String(base64Data);
        var path = Path.Combine("wwwroot/signatures", $"sign_{request.DocumentName}.png");
        System.IO.File.WriteAllBytes(path, bytes);
        return new JsonResult(new { success = true });
    }

   
}

public class SignatureRequest
{
    public string ImageData { get; set; }
    public string DocumentName { get; set; }
}