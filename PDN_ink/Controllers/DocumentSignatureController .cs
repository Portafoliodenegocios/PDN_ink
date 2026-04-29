using Microsoft.AspNetCore.Mvc;

namespace Pdnink.Controllers
{
    public class DocumentSignatureController : Controller
    {
        public IActionResult Signature(string documentName)
        {
            ViewBag.DocumentName = documentName;
            return PartialView("_SignaturePartial");
            
        }

        [HttpPost]
        public IActionResult SaveSignature(string imageData, string documentName)
        {
            if (string.IsNullOrEmpty(imageData))
                return BadRequest("No signature received.");
            var base64Data = imageData.Replace("data:image/png;base64,", "");
            var bytes = Convert.FromBase64String(base64Data);
            var path = Path.Combine("wwwroot/signatures", $"sign_{documentName}.png");
            System.IO.File.WriteAllBytes(path, bytes);
            return Ok(new { success = true });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
