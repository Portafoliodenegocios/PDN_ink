using BoldReports;
using BoldReports.Processing.ObjectModels;
using BoldReports.RDL.DOM;
using DevExpress.AspNetCore.Spreadsheet;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.DocumentFormats.Xlsb;
using HR_Templates.Models;
using HR_Templates.Proxys;
using HR_Templates.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.ReportingServices2010;
using System.Net;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using RS = ReportingServices;



namespace HR_Templates.Pages
{

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VisorReportModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        private readonly IConfiguration _configuration;

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Extension { get; set; }

        public string Token_ { get; set; }

        public VisorReportModel(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        public async System.Threading.Tasks.Task OnGet(string access_token)
        {

            Token_ = access_token;
         var result = await PDNInk_Report_Type_OptionCBR
               .Instance
               .GetBy($"Is_Active = True AND Document_Option_Id = {Id}");

        if (result.Count >  0)
           {
                var rpttypeopt =  result.FirstOrDefault();
                ViewData["NameReport"] = rpttypeopt?.Report_File;
                ViewData["FullName"] = Name;
            }
        }


        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostGenerateReport([FromBody] ReportRequest model)
        {
            try
            {
                string reportName = model.ReportName;
                string Namefull =  model.Fullname;

                IDictionary<string, object> parameters = null;

                switch (reportName)
                {
                    case "rptAcceptanceScholarship":
                        parameters = new Dictionary<string, object> { { "rpFecha", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") },{ "rpNombre", Namefull } };
                       

                        break;
                    case "2":
                        Console.WriteLine("Martes");
                        break;
                    case "3":
                        Console.WriteLine("Miércoles");
                        break;
                    default:
                        Console.WriteLine("Otro día");
                        break;
                }

               

                byte[] reportContent = await RenderReport(reportName, parameters, null);

                if (reportContent == null || reportContent.Length == 0)
                    return BadRequest("No se generó el reporte");

                string fileName = $"{model.ReportName}.pdf";
                string folder = Path.Combine(_env.WebRootPath, "signatures");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string path = Path.Combine(folder, fileName);
                System.IO.File.WriteAllBytes(path, reportContent);


                return new JsonResult(new { success = true, url = "/signatures/" + fileName });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }


        protected async Task<byte[]> RenderReport(string reportName, IDictionary<string, object> parameters, string reportFolder) //object> parameters,
        {
            try
            {

           

            var reportSettings = _configuration.GetSection("ReportServer");
            string reportPath = $"/{reportFolder ?? reportSettings["ReportsFolder"]}/{reportName}";
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.SendTimeout = new TimeSpan(0, 50, 0);//aqui-10


            long MaxFileSize = Convert.ToInt64(reportSettings["ReportsMaxFileSize"]);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            binding.MaxReceivedMessageSize = MaxFileSize;


            RS.ReportExecutionServiceSoapClient reportClient = new RS.ReportExecutionServiceSoapClient(binding,
                                                                                                        new EndpointAddress(reportSettings["ReportServerUrl_2"]));

            var clientCredentials = new NetworkCredential(reportSettings["ReportsUserName"]?.ToString(),
                                                          reportSettings["ReportsPassword"]?.ToString(),
                                                          reportSettings["ReportsDomain"]?.ToString());
            reportClient.ClientCredentials.Windows.AllowedImpersonationLevel =
                System.Security.Principal.TokenImpersonationLevel.Impersonation;
            reportClient.ClientCredentials.Windows.ClientCredential = clientCredentials;

            reportClient.Endpoint.EndpointBehaviors.Add(new ReportingServiceEndPointBehavior());

            RS.TrustedUserHeader trustedUserHeader = new RS.TrustedUserHeader();
            RS.ExecutionHeader execHeader = new RS.ExecutionHeader();

            trustedUserHeader.UserName = clientCredentials.UserName;

            var request1 = new RS.LoadReportRequest()
            {
                TrustedUserHeader = null,//trustedUserHeader,
                Report = reportPath,
            };
            var taskLoadReport = await reportClient.LoadReportAsync(request1);
            execHeader.ExecutionID = taskLoadReport.executionInfo.ExecutionID;


                RS.ParameterValue[] reportParameters = null;
                if (parameters != null && parameters.Count > 0)
                    reportParameters = taskLoadReport.executionInfo.Parameters.Where(x => parameters.ContainsKey(x.Name))
                        .Select(x => new RS.ParameterValue() { Name = x.Name, Value = parameters[x.Name].ToString() }).ToArray();

                var equest2 = new RS.SetExecutionParametersRequest()
            {
                ExecutionHeader = execHeader,
                TrustedUserHeader = null,//trustedUserHeader,
                Parameters = reportParameters,
                ParameterLanguage = "en-us"
            };

            await reportClient.SetExecutionParametersAsync(equest2);//(execHeader, trustedUserHeader, reportParameters, "en-us");
            const string deviceInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

            var response =
                await reportClient.RenderAsync(new RS.RenderRequest(execHeader, null, "PDF", deviceInfo));//(execHeader, trustedUserHeader, "PDF", deviceInfo));
            return response.Result;

            }
            catch (Exception ex )
            {

                throw ex;
            }
        }



        //[IgnoreAntiforgeryToken]
        //public async Task<IActionResult> OnGetLoadReport(string creditId, string reportId)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(creditId) || string.IsNullOrEmpty(reportId))
        //            return BadRequest("Parámetros inválidos");

        //        string reportName = "PepsFormatDetermination";

        //        var parameters = new Dictionary<string, object>
        //       {
        //          { "Credit_Id", creditId }
        //       };

        //        byte[] reportContent = await RenderReport(reportName, parameters, null);

        //        if (reportContent == null || reportContent.Length == 0)
        //            return StatusCode(500, "No se pudo generar el reporte");

        //        Response.Headers.Add("Content-Disposition", "inline; filename=reporte.pdf");

        //        return File(reportContent, "application/pdf");
        //    }
        //    catch
        //    {
        //        return StatusCode(500, "Error");
        //    }
        //}

        //[IgnoreAntiforgeryToken]
        //public async Task<IActionResult> OnPostLoadReport(string creditId, string reportId)
        //{

        //    var reportSettings = _configuration.GetSection("ReportServer");

        //    try
        //    {
        //        if (string.IsNullOrEmpty(creditId) || string.IsNullOrEmpty(reportId))
        //            return BadRequest("Parámetros inválidos");

        //        string reportName = string.Empty;
        //        IDictionary<string, object> parameters = null;

        //        //  rptPersonalLoanApplication

        //        reportName = "PepsFormatDetermination";
        //        parameters = new Dictionary<string, object> { { "Credit_Id", creditId } };


        //        byte[] reportContent = await RenderReport(reportName, parameters, null);

        //        if (reportContent == null || reportContent.Length == 0)
        //            return StatusCode(500, "No se pudo generar el reporte");

        //        string pdfName = $"{creditId}_{reportName}_{DateTime.Now:yyyyMddTHHmmss}.pdf";

        //        return File(reportContent, "application/pdf", pdfName);
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = $"[EXCEPTION] {ex.GetType().Name}: {ex.Message}";
        //        msg = msg + $"[STACK] {ex.StackTrace}";
        //        msg = msg + $"[DATA] creditId={creditId}, reportId={reportId}";

        //        return StatusCode(500, "Ocurrió un error inesperado al generar el reporte");
        //    }


        //}








        public IActionResult OnPostSaveSignature([FromBody] SignatureRequest request)
        {
            if (string.IsNullOrEmpty(request.ImageData))
                return BadRequest("No signature received.");
            var base64Data = request.ImageData.Replace("data:image/png;base64,", "");
            var bytes = Convert.FromBase64String(base64Data);
            var path = Path.Combine("wwwroot/signatures", $"{request.DocumentName}.png");
            System.IO.File.WriteAllBytes(path, bytes);

            var SignFile = Path.Combine("wwwroot/signatures", $"{request.DocumentName}.png");
            var originalPdf = Path.Combine("wwwroot/signatures", $"{request.DocumentName}.pdf");
            string signedPdf = System.IO.Path.Combine(Path.Combine("wwwroot/signatures", $"{request.DocumentName}_sign.pdf"));

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




        //[IgnoreAntiforgeryToken]
        //public IActionResult OnPostAddSigninFile([FromBody] SpreadsheetStateRequest request)
        //{


            

        //    var excelStream = new MemoryStream();
         
        //    excelStream.Position = 0;
        //    var path = Path.Combine("wwwroot", "signatures", $"{request.NameFile}.xlsx");
        //    Directory.CreateDirectory(Path.Combine("wwwroot", "signatures"));
        //    System.IO.File.WriteAllBytes(path, excelStream.ToArray());

        //    var SignFile = Path.Combine("wwwroot/signatures", $"{request.NameFile}.png");
        //    var originalPdf = Path.Combine("wwwroot/signatures", $"{request.NameFile}.pdf");
        //    string signedPdf = System.IO.Path.Combine(Path.Combine("wwwroot/signatures", $"{request.NameFile}_sign.pdf"));
        //    excelStream.Position = 0;

        //    using (Workbook workbook = new Workbook())
        //    {
        //        workbook.LoadDocument(excelStream, DocumentFormat.Xlsx);

        //        using (MemoryStream pdfStream = new MemoryStream())
        //        {
        //            workbook.ExportToPdf(pdfStream);
        //            System.IO.File.WriteAllBytes(originalPdf, pdfStream.ToArray());
        //        }
        //    }

        //    using (var reader = new iTextSharp.text.pdf.PdfReader(originalPdf))
        //    using (var fs = new FileStream(signedPdf, FileMode.Create))
        //    {
        //        var stamper = new iTextSharp.text.pdf.PdfStamper(reader, fs);
        //        int totalPages = reader.NumberOfPages;
        //        var Imgsign = iTextSharp.text.Image.GetInstance(SignFile);
        //        Imgsign.ScaleToFit(150f, 80f);
        //        Imgsign.SetAbsolutePosition(30, 245);

        //        //if (totalPages > 2)
        //        //    nopagesdesc = 2;

        //        var content = stamper.GetOverContent(1);
        //        content.AddImage(Imgsign);
        //        stamper.FormFlattening = true;

        //        stamper.Close();
        //        reader.Close();
        //        if (!System.IO.File.Exists(signedPdf))
        //            throw new Exception("No se creó el archivo firmado físicamente.");
        //    }



        //    return new JsonResult(new { success = true });
        //}


    }
}

