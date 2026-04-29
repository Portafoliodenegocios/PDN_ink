using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Pdnink.Models;
using Rotativa.AspNetCore;
using Spire.Xls;

namespace Pdnink.Controllers
{
    public class DocumentsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult VacationRequest()
        {
            return View();  
        }

        public IActionResult VacationRequestII()
        {
            return View();
        }
        

        public IActionResult GeneratePdf(VacationRequest model)
        {
            return new ViewAsPdf("VacationRequestPdf", model)
            {
                FileName = "SolicitudVacaciones.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        public IActionResult GenerateVacationPdf()
        {
            var model = new VacationRequest
            {
                EmployeeName = "Juan Pérez",
                PayrollNumber = "12345",
                Position = "Developer",
                Department = "IT"
            };

            var excel = FillExcelTemplate(model);

            var pdf = ConvertExcelToPdf(excel);

            return File(pdf, "application/pdf", "SolicitudVacaciones.pdf");
        }

        public byte[] FillExcelTemplate(VacationRequest model)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/templates/VacationTemplate.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(new FileInfo(path));

            var sheet = package.Workbook.Worksheets[0];

            sheet.Cells["B8"].Value = model.EmployeeName;
            sheet.Cells["E8"].Value = model.PayrollNumber;
            sheet.Cells["B9"].Value = model.HireDate;
            sheet.Cells["B10"].Value = model.Position;
            sheet.Cells["E10"].Value = model.Department;

            return package.GetAsByteArray();
        }

        public byte[] ConvertExcelToPdf(byte[] excelBytes)
        {
            using var ms = new MemoryStream(excelBytes);

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);

            using MemoryStream pdfStream = new MemoryStream();
            workbook.SaveToStream(pdfStream, FileFormat.PDF);

            return pdfStream.ToArray();
        }

    }
}
