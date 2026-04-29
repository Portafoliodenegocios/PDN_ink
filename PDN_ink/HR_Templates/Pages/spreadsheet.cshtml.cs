using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Spreadsheet;
using DevExpress.Charts.Model;
using DevExpress.DataAccess.Design;
using DevExpress.Docs;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HR_Templates.Models;

namespace HR_Templates.Pages
{
    [IgnoreAntiforgeryToken]
    [Route("[action]")]
    public class SpreadsheetModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Extension { get; set; }

        public void OnGet()
        {
            ViewData["NameTemplate"]  = $"{Name}.{Extension}";
            ViewData["NameFile"] = $"{Name}";
        }      
      
        public IActionResult OnGetDxSpreadsheetRequest()
        {
            //System.IO.File.WriteAllText("C:\\temp\\SI_ENTRA_GET.txt", "OK");
            return SpreadsheetRequestProcessor.GetResponse(HttpContext);            
        }              
        public IActionResult OnPostDxSpreadsheetRequest()
        {
             return SpreadsheetRequestProcessor.GetResponse(HttpContext);           
        }
        public IActionResult OnPostSaveSignature([FromBody] SignatureRequest request)
        {
            if (string.IsNullOrEmpty(request.ImageData))
                return BadRequest("No signature received.");
            var base64Data = request.ImageData.Replace("data:image/png;base64,", "");
            var bytes = Convert.FromBase64String(base64Data);
            var path = Path.Combine("wwwroot/signatures", $"{request.DocumentName}.png");
            System.IO.File.WriteAllBytes(path, bytes);
            return new JsonResult(new { success = true });
        }
       
        [IgnoreAntiforgeryToken]
        public IActionResult OnPostRibbonSaveToFile([FromBody] SpreadsheetStateRequest request)
        {

            
            if (request?.SpreadsheetState == null)
                return BadRequest("No se recibió el estado");

            var spreadsheet = SpreadsheetRequestProcessor.GetSpreadsheetFromState(request.SpreadsheetState);
            spreadsheet.Save(); 

            var excelStream = new MemoryStream();
            spreadsheet.SaveCopy(excelStream, DocumentFormat.Xlsx);
            excelStream.Position = 0;
            var path = Path.Combine("wwwroot", "signatures", $"{request.NameFile}.xlsx");          
            Directory.CreateDirectory(Path.Combine("wwwroot", "signatures"));
            System.IO.File.WriteAllBytes(path, excelStream.ToArray());

            var SignFile = Path.Combine("wwwroot/signatures", $"{request.NameFile}.png");
            var originalPdf = Path.Combine("wwwroot/signatures", $"{request.NameFile}.pdf");
            string signedPdf = System.IO.Path.Combine(Path.Combine("wwwroot/signatures", $"{request.NameFile}_sign.pdf"));
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


        public IActionResult OnPostDxSpreadsheet()
        {
            return SpreadsheetRequestProcessor.GetResponse(HttpContext); 
        }


        /////***************************************
        ///
        //public void Save_Document(SpreadsheetClientState spreadsheetState)
        //{
        //    var spreadsheet = SpreadsheetRequestProcessor.GetSpreadsheetFromState(spreadsheetState);


        //    spreadsheet.Save();



        //    var path = Path.Combine("wwwroot/signatures", $"Excelupdate.png");
        //    spreadsheet.SaveCopy(path);

        //    byte[] documentContent = spreadsheet.SaveCopy(DocumentFormat.Xlsx);
        //    var stream = new MemoryStream();
        //    spreadsheet.SaveCopy(stream, DocumentFormat.Xlsx);
        //}


        //public IActionResult OnGetExportToPdf()
        //{

        //    var fullPath = Path.Combine(Directory.GetCurrentDirectory(),
        //                                 DirectoryManagmentUtils.GetDocumentSampleFolderPath(HttpContext),
        //                                "Solicitud de vacaciones.xlsx"
        //                               );

        //    if (!System.IO.File.Exists(fullPath))
        //        return NotFound();

        //    using (Workbook workbook = new Workbook())
        //    {
        //        workbook.LoadDocument(fullPath);

        //        using (MemoryStream pdfStream = new MemoryStream())
        //        {
        //            workbook.ExportToPdf(pdfStream);
        //            pdfStream.Position = 0;

        //            return File(pdfStream.ToArray(), "application/pdf", "Resultado.pdf");
        //        }
        //    }
        //}


    }





}

