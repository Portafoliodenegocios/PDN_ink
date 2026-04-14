using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Spreadsheet;
using DevExpress.Charts.Model;
using DevExpress.DataAccess.Design;
using DevExpress.Docs;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HR_Templates.Pages
{
    [IgnoreAntiforgeryToken]
    public class SpreadsheetModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Extension { get; set; }

        public void OnGet()
        {
            ViewData["NameTemplate"]  = $"{Name}.{Extension}";
            
        }
      
      
        public IActionResult OnGetDxSpreadsheetRequest()
        {
            System.IO.File.WriteAllText("C:\\temp\\SI_ENTRA_GET.txt", "OK");
            return SpreadsheetRequestProcessor.GetResponse(HttpContext);
            
        }
              
        public IActionResult OnPostDxSpreadsheetRequest()
        {
             return SpreadsheetRequestProcessor.GetResponse(HttpContext);

            //var result = SpreadsheetRequestProcessor.GetResponse(HttpContext);

            //string command = HttpContext.Request.Form["DXCallbackArgument"];

            //if (command == "SAVE_DOC")
            //{
            //    string folder = DirectoryManagmentUtils.GetSessionFolderKey(HttpContext);

            //    string sourcePath = Path.Combine(
            //        Directory.GetCurrentDirectory(),
            //        "App_Data",
            //        folder,
            //        "Solicitud de vacaciones.xlsx"
            //    );

            //    string destPath = Path.Combine(
            //        Directory.GetCurrentDirectory(),
            //        "wwwroot",
            //        "Docs",
            //        "Solicitud de vacaciones.xlsx"
            //    );

            //    if (System.IO.File.Exists(sourcePath))
            //    {
            //        System.IO.File.Copy(sourcePath, destPath, true);
            //    }
            //}

            //return result;

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

        public IActionResult OnGetExportToPdf( )
        {
        
        var fullPath = Path.Combine( Directory.GetCurrentDirectory(),   
                                     DirectoryManagmentUtils.GetDocumentSampleFolderPath(HttpContext),
                                    "Solicitud de vacaciones.xlsx"
                                   );

            if (!System.IO.File.Exists(fullPath))
            return NotFound();

        using (Workbook workbook = new Workbook())
        {
            workbook.LoadDocument(fullPath);

            using (MemoryStream pdfStream = new MemoryStream())
            {
                workbook.ExportToPdf(pdfStream);
                pdfStream.Position = 0;

                return File(pdfStream.ToArray(), "application/pdf", "Resultado.pdf");
            }
        }
    }

        public void SaveDocument(SpreadsheetClientState spreadsheetState)
        {
            var spreadsheet = SpreadsheetRequestProcessor.GetSpreadsheetFromState(spreadsheetState);

            // Saves an active document back to the file you opened it from
            spreadsheet.Save();

            var path = Path.Combine("wwwroot/signatures", $"Excelupdate.png");
            // Saves a copy of an active document to a file in the server's file system.
            spreadsheet.SaveCopy(path);

            // Saves a copy of an active document to a byte array
            byte[] documentContent = spreadsheet.SaveCopy(DocumentFormat.Xlsx);

            // Saves a copy of an active document to a stream
            var stream = new MemoryStream();
            spreadsheet.SaveCopy(stream, DocumentFormat.Xlsx);
        }
      
        [IgnoreAntiforgeryToken]
        public IActionResult OnPostRibbonSaveToFile([FromBody] SpreadsheetStateRequest request)
        {

            if (request?.SpreadsheetState == null)
                return BadRequest("No se recibió el estado");

            var spreadsheet = SpreadsheetRequestProcessor
                .GetSpreadsheetFromState(request.SpreadsheetState);

            spreadsheet.Save(); 
            var excelStream = new MemoryStream();
            spreadsheet.SaveCopy(excelStream, DocumentFormat.Xlsx);
            excelStream.Position = 0;
            var path = Path.Combine("wwwroot", "signatures", "ExcelGenerado.xlsx");          
            Directory.CreateDirectory(Path.Combine("wwwroot", "signatures"));

            System.IO.File.WriteAllBytes(path, excelStream.ToArray());




            var SignFile = Path.Combine("wwwroot/signatures", "sign_Documento1.png");
            var originalPdf = Path.Combine("wwwroot/signatures", "Excelupdate.pdf");
            string signedPdf = System.IO.Path.Combine(Path.Combine("wwwroot/signatures", "Document_sign.pdf"));


            excelStream.Position = 0;

            using (Workbook workbook = new Workbook())
            {
                workbook.LoadDocument(excelStream, DocumentFormat.Xlsx);

                using (MemoryStream pdfStream = new MemoryStream())
                {
                    workbook.ExportToPdf(pdfStream);                  
                    System.IO.File.WriteAllBytes(originalPdf, pdfStream.ToArray());
                }
            }




            using (var reader = new iTextSharp.text.pdf.PdfReader(originalPdf))
            using (var fs = new FileStream(signedPdf, FileMode.Create))
            {
                var stamper = new iTextSharp.text.pdf.PdfStamper(reader, fs);
                int totalPages = reader.NumberOfPages;
                var Imgsign = iTextSharp.text.Image.GetInstance(SignFile);
                Imgsign.ScaleToFit(150f, 80f);
                Imgsign.SetAbsolutePosition(30, 245);

                //if (totalPages > 2)
                //    nopagesdesc = 2;

                var content = stamper.GetOverContent(1);
                content.AddImage(Imgsign);

                stamper.FormFlattening = true;

                stamper.Close();
                reader.Close();
                if (!System.IO.File.Exists(signedPdf))
                    throw new Exception("No se creó el archivo firmado físicamente.");
            }



            return new JsonResult(new { success = true });
        }


    }





}

public class SpreadsheetStateRequest
{
    public DevExpress.AspNetCore.Spreadsheet.SpreadsheetClientState SpreadsheetState { get; set; }
}