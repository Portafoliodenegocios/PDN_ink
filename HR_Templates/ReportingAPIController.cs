using BoldReports.Models.ReportViewer;
using BoldReports.RDL.DOM;
using BoldReports.Web.ReportDesigner;
using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Templates
{
    [Route("api/[controller]/[action]")]
    public class ReportingAPIController : Controller, IReportDesignerController
    {

    private Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;
    private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;
    private readonly IConfiguration _configuration;

        public ReportingAPIController(Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

    [NonAction]
    private string GetFilePath(string itemName, string key)
    {
        string rootPath = this._hostingEnvironment.WebRootPath != null ? this._hostingEnvironment.WebRootPath : this._hostingEnvironment.ContentRootPath;
        string dirPath = Path.Combine(rootPath, "Cache", key);

        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }

        return Path.Combine(dirPath, itemName);
    }

    public object GetImage(string key, string image)
    {
        return ReportDesignerHelper.GetImage(key, image, this);
    }

    public object GetResource(ReportResource resource)
    {
        return ReportHelper.GetResource(resource, this, _cache);
    }


        public void LoadReport(ReportViewerOptions reportOption, string reportName)
        {

            var reportSettings = _configuration.GetSection("ReportServer");

            // Cambia esto si el server pide la URL base:
            reportOption.ReportModel.ReportServerUrl = "http://hermes/ReportServer";

            // Asegúrate de que no haya dobles slashes //
            reportOption.ReportModel.ReportPath = "/Reports/rptPersonalLoanApplication";

            reportOption.ReportModel.ReportServerCredential = new System.Net.NetworkCredential(
                "reports", "Syst3m10", "cclpv");
            //var reportSettings = _configuration.GetSection("ReportServer");
            //string folder = reportSettings["ReportsFolder"];

            //// Configuración del Servidor
            //reportOption.ReportModel.ReportServerUrl = reportSettings["ReportServerUrl"];

            //// Construcción de la ruta: /Reports/MiReporteDeVentas
            //reportOption.ReportModel.ReportPath = $"/{folder}/{reportName}";

            //// Credenciales desde appsettings.json
            //reportOption.ReportModel.ReportServerCredential = new System.Net.NetworkCredential(
            //    reportSettings["ReportsUserName"],
            //    reportSettings["ReportsPassword"],
            //    reportSettings["ReportsDomain"]
            //);
        }

        [NonAction]
    public void OnInitReportOptions(ReportViewerOptions reportOption)
    {
            LoadReport(reportOption, "PepsFormatDetermination");


            

            var parameters = new List<BoldReports.Web.ReportParameter>();

            parameters.Add(new BoldReports.Web.ReportParameter  { Name = "rpArea",  Values= new List<string> { "Sistemas" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpCantidadL", Values = new List<string> { "Dos mil" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpCantidadN", Values = new List<string> { "2000" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpCapital", Values = new List<string> { "Gabriela Ruiz" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpDescuento", Values = new List<string> { "1" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpFecha", Values = new List<string> { "04/14/2026" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpFechaI", Values = new List<string> { "04/14/2026" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpFechaIU", Values = new List<string> { "05/16/2026" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpFormaDPago", Values = new List<string> { "True" } });

            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpJefe", Values = new List<string> { "Roberto Bayo" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpModo", Values = new List<string> { "true" } });

            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpMotivo", Values = new List<string> { "Motivo" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpNombre", Values = new List<string> { "Emmanuel Hidalgo" } });

            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpNomina", Values = new List<string> { "1087" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpNoPagos", Values = new List<string> { "1" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpObservaciones", Values = new List<string> { "bla bla" } });
            parameters.Add(new BoldReports.Web.ReportParameter { Name = "rpPrimerPago", Values = new List<string> { "True" } });

            reportOption.ReportModel.Parameters = parameters;
        }

    [NonAction]
    public void OnReportLoaded(ReportViewerOptions reportOption)
    {
        //You can update report options here
    }

    [HttpPost]
    public object PostDesignerAction([FromBody] Dictionary<string, object> jsonResult)
    {
        return ReportDesignerHelper.ProcessDesigner(jsonResult, this, null, this._cache);
    }

    public object PostFormDesignerAction()
    {
        return ReportDesignerHelper.ProcessDesigner(null, this, null, this._cache);
    }

    public object PostFormReportAction()
    {
        return ReportHelper.ProcessReport(null, this, this._cache);
    }

    [HttpPost]
    public object PostReportAction([FromBody] Dictionary<string, object> jsonResult)
    {
        return ReportHelper.ProcessReport(jsonResult, this, this._cache);
    }

    [NonAction]
    public bool SetData(string key, string itemId, ItemInfo itemData, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (itemData.Data != null)
        {
            System.IO.File.WriteAllBytes(this.GetFilePath(itemId, key), itemData.Data);
        }
        else if (itemData.PostedFile != null)
        {
            var fileName = itemId;
            if (string.IsNullOrEmpty(itemId))
            {
                fileName = System.IO.Path.GetFileName(itemData.PostedFile.FileName);
            }

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                itemData.PostedFile.OpenReadStream().CopyTo(stream);
                byte[] bytes = stream.ToArray();
                var writePath = this.GetFilePath(fileName, key);

                System.IO.File.WriteAllBytes(writePath, bytes);
                stream.Close();
                stream.Dispose();
            }
        }
        return true;
    }
    [NonAction]
    public ResourceInfo GetData(string key, string itemId)
    {
        var resource = new ResourceInfo();
        try
        {
            var filePath = this.GetFilePath(itemId, key);
            if (itemId.Equals(Path.GetFileName(filePath), StringComparison.InvariantCultureIgnoreCase) && System.IO.File.Exists(filePath))
            {
                resource.Data = System.IO.File.ReadAllBytes(filePath);
            }
            else
            {
                resource.ErrorMessage = "File not found from the specified path";
            }
        }
        catch (Exception ex)
        {
            resource.ErrorMessage = ex.Message;
        }
        return resource;
    }

    [HttpPost]
    public void UploadReportAction()
    {
        ReportDesignerHelper.ProcessDesigner(null, this, this.Request.Form.Files[0], this._cache);
    }
}
    }