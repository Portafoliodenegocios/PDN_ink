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

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VisorReportModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        private readonly IConfiguration _configuration;
        
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
        
         var result = await PDNInk_Report_Type_OptionCBR
               .Instance
               .GetBy($"Is_Active = True AND Document_Option_Id = {Id}");

           if (result.Count >  0)
           {
                var rpttypeopt =  result.FirstOrDefault();
                ViewData["NameReport"] = rpttypeopt?.Report_File;               
                ViewData["FullName"] = User.Claims.FirstOrDefault(x => x.Type == "FullName").Value; 
                ViewData["Position"] = User.Claims.FirstOrDefault(x => x.Type == "Position").Value; 
                ViewData["EmployeeId"] = User.Claims.FirstOrDefault(x => x.Type == "EmployeeId").Value;
                ViewData["Area"] = User.Claims.FirstOrDefault(x => x.Type == "Area").Value;
                ViewData["Token_"]  = access_token;
            }

       
        }

        public async Task<IActionResult> OnPostGenerateVehicleReport([FromForm] VehicleReportRequest model)
        {
               

             var     parameters = new Dictionary<string, object> { { "rpFechaDocumento", model.rpFechaDocumento.ToString("yyyy-MM-ddTHH:mm:ss") },
                                                                      { "rpNombreColaborador", model.rpNombreColaborador },
                                                                      { "rpPuestoColaborador", model.rpPuestoColaborador },
                                                                      { "rpNEmpleado", model.rpNEmpleado },
                                                                      { "rpArea", model.rpArea },
                                                                      { "rpEntrega", model.rpentrega },
                                                                      { "rpDevolución", model.rpDevolución },
                                                                      { "rpFechaEntrega", model.rpFechaEntrega.ToString("yyyy-MM-ddTHH:mm:ss") },
                                                                      { "rpFechaDevolucion", model.rpFechaDevolucion.ToString("yyyy-MM-ddTHH:mm:ss") },
                                                                      { "rpPolizaSeguro", model.rpPolizaSeguro },
                                                                      { "rpTarjetaCirculacion", model.rpTarjetaCirculacion },
                                                                      { "rpVerificacion", model.rpVerificacion },
                                                                      { "rpManual", model.rpManual },
                                                                      { "rpMarcaTipo", model.rpMarcaTipo },
                                                                      { "rpPlacas", model.rpPlacas },
                                                                      { "rpKilometrajeInicial", model.rpKilometrajeInicial },
                                                                      { "rpKilometrajeFinal", model.rpKilometrajeFinal   },
                                                                      { "rpUsado", model.rpUsado },
                                                                      { "rpModelo", model.rpModelo },
                                                                      { "rpOtro", model.rpOtro   == null? string.Empty: model.rpOtro },
                                                                      { "rpObservacionCarroceria", model.rpObservacionCarroceria  == null? string.Empty:model.rpObservacionCarroceria   },
                                                                      { "rpObservacionLlantas", model.rpObservacionLlantas  == null? string.Empty: model.rpObservacionLlantas },
                                                                      { "rpObservacionIntExt", model.rpObservacionIntExt  == null? string.Empty: model.rpObservacionIntExt },
                                                                      { "rpNombreEntrega", model.rpNombreEntrega },
                                                                      { "rpNombreRecibePND", model.rpNombreRecibePND == null? string.Empty: model.rpNombreRecibePND },
                                                                      { "rpNombreEntrego", model.rpNombreEntrego == null? string.Empty: model.rpNombreEntrego    }


                                                                    };

            return await GenerateReportAsync(model.ReportName, parameters);


        }

        public async Task<IActionResult> OnPostGenerateReport([FromForm] ReportRequest model)
        {
            try
            {
                string reportName = model.ReportName;
                string Namefull = model.Fullname;
                bool IsParams = false;

                IDictionary<string, object> parameters = null;

                switch (reportName)
                {
                    case "rptAcceptanceScholarship":
                        parameters = new Dictionary<string, object> { { "rpFecha", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") }, { "rpNombre", Namefull } };
                        IsParams = true;
                        break;
                    case "rtpLetterofAcceptanceoftheCodeofEthicsandConduct":
                        parameters = new Dictionary<string, object> { { "rpFecha", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") }, { "rpNombre", Namefull } };
                        IsParams = true;
                        break;
                    case "rptLetterofAcceptanceofScholarshipPaymentIntoBankAccount":
                        parameters = new Dictionary<string, object> { { "rpFecha", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") }, { "rpNombre", Namefull } };
                        IsParams = true;
                        break;
                    case "rptLetterAcceptanceRegulations":
                        parameters = new Dictionary<string, object> { { "rpFecha", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") }, { "rpNombre", Namefull } };
                        IsParams = true;
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
  
        public async Task<IActionResult> OnPostGenerateBonoReport([FromForm] BonoReportRequest model)
        {           

               var  parameters = new Dictionary<string, object>
                {
                    { "rpFecha", model.rpFecha.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpNombreEmpleado", model.rpNombreEmpleado },
                    { "rpPuesto", model.rpPuesto },
                    { "rpArea", model.rpAreab },
                    { "rpNEmpleado", model.rpNEmpleadob },
                    { "rpMotivoBono", model.rpMotivoBono },
                    { "rpMontoBono", model.rpMontoBono },
                    { "rpPrimerFecha", model.rpPrimerFecha.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpSegundaFecha", model.rpSegundaFecham.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpNQuincenas", model.rpNQuincenas },
                    { "rpNMeses", model.rpNMeses },
                    { "rpObservaciones", model.rpObservaciones  }
                };

            return await GenerateReportAsync(model.ReportName, parameters);
        }

        public async Task<IActionResult> OnPostGeneratePrivacyNoticedReport([FromForm] PrivacyNoticereportRequestcs model)
        {
           
           var  parameters = new Dictionary<string, object>
                {
                    { "rpFecha", model.rp_Fecha.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpNombreEmpleado", model.rpNombre_Empleado },
                    { "rpLugar", model.rpLugar }
                   
                };
            return await GenerateReportAsync(model.ReportName, parameters);
        }

        public async Task<IActionResult> OnPostGeneratePositionChangeReport([FromForm] PositionChangeReportRequest model)
        {           
             
            var  parameters = new Dictionary<string, object>
                    {
                        { "rpFechaCambio", model.rpFechaCambio.ToString("yyyy-MM-ddTHH:mm:ss") },
                        { "rpNEmpleado", model.rpNEmpleadop },
                        { "rpDireccion", model.rpDireccion },
                        { "rpApellidoP", model.rpApellidoP },
                        { "rpApellidoM", model.rpApellidoM },
                        { "rpNombre", model.rpNombre },
                        { "rpNSS", model.rpNSS },
                        { "rpFechaIngreso", model.rpFechaIngreso.ToString("yyyy-MM-ddTHH:mm:ss") },
                        { "rpReubicacion", model.rpReubicacion },
                        { "rpDonde", model.rpDonde == null ? string.Empty : model.rpDonde  },
                        { "rpNivel", model.rpNivel },
                        { "rpHoraInicio", model.rpHoraInicio },
                        { "rpHoraFinal", model.rpHoraFinal },
                        { "rpNombreNuevoPuesto", model.rpNombreNuevoPuesto },
                        { "rpNombreAreaNueva", model.rpNombreAreaNueva },
                        { "rpObjetivoCambio", model.rpObjetivoCambio },
                        { "rpObservaciones", model.rpObservaciones == null ? string.Empty : model.rpObservaciones },
                        { "rpJefeAnterior", model.rpJefeAnterior },
                        { "rpCapitalHumano", model.rpCapitalHumano },
                        { "rpNuevoJefe", model.rpNuevoJefe }
                    };


            return await GenerateReportAsync(model.ReportName, parameters);
        }

         public async Task<IActionResult> OnPostGenerateNoDebtInfonavitReport([FromForm] NoDebtInfonavitReportRequest model)
         {         

           var  parameters = new Dictionary<string, object>
                {
                    { "rpFecha", model.rpFecha_.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpNombre", model.rpNombre_ },
                    { "rpSioNo", model.rpSioNo }

                };
            return await GenerateReportAsync(model.ReportName, parameters);

        }

        public async Task<IActionResult> OnPostGenerateSettlementLetterReport([FromForm] SettlementLetterReportRequest model)
        {            

            var    parameters = new Dictionary<string, object>
                            {
                                { "rpNombre", model.rpNombref },
                                { "rpNombreEmpresa", model.rpNombreEmpresa },
                                { "rpFecha", model.rpFechaf.ToString("yyyy-MM-ddTHH:mm:ss") },
                                { "rpNumero", model.rpNumero },
                                { "rpTexto", model.rpTexto },
                                { "rpFechaFinal", model.rpFechaFinal.ToString("yyyy-MM-ddTHH:mm:ss") }
                            };

            return await GenerateReportAsync(model.ReportName, parameters);

        }

        public async Task<IActionResult> OnPostGenerateInstructionLetterReport([FromForm] InstructionLetterreportRequestcs model)
        {          

           var  parameters = new Dictionary<string, object>
                {
                    { "rpFecha", model.rpFechai.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpNombreLic", model.rpNombreLic },
                    { "rpFechaPagare", model.rpFechaPagare.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpCantidadNumero", model.rpCantidadNumero },
                    { "rpCantidadLetra", model.rpCantidadLetra },
                    { "rpNombres", model.rpNombres },
                    { "rpCantidadD", model.rpCantidadD },
                    { "rpCantidadDL", model.rpCantidadDL }
                };

            return await GenerateReportAsync(model.ReportName, parameters);
        }

        public async Task<IActionResult> OnPostGenerateVehicleResponsibilityLetterReport([FromForm] VehicleResponsibilityLetterreportRequest model)
        {
          var   parameters = new Dictionary<string, object>
                {
                    { "rpFecha", model.rpFechar.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpPlacas", model.rpPlacasr },
                    { "rpMarca", model.rpMarca },
                    { "rpModelo", model.rpModelor },
                    { "rpMotor", model.rpMotor },
                    { "rpSerie", model.rpSerie },
                    { "rpArea", model.rpArear },
                    { "rpNombre", model.rpNombrer },
                    { "rpLicencia", model.rpLicencia },
                    { "rpDomicilio", model.rpDomicilio }
                };

            return await GenerateReportAsync(model.ReportName, parameters);
        }

        public async Task<IActionResult> OnPostGenerateScholarshipAgreementReport([FromForm] ScholarshipAgreementreportRequest model)
        {
           var  parameters = new Dictionary<string, object>
                        {
                            { "rpArea", model.rpAreat },
                            { "rpDomicilio", model.rpDomiciliot },
                            { "rpNombre", model.rpNombret },
                            { "rpEdad", model.rpEdad },
                            { "rpEstadoCivil", model.rpEstadoCivil },
                            { "rpSexo", model.rpSexo },
                            { "rpRFC", model.rpRFC },
                            { "rpCURP", model.rpCURP },
                            { "rpDomicilioB", model.rpDomicilioBt },
                            { "rpSC", model.rpSC },
                            { "rpCarrera", model.rpCarrera },
                            { "rpNombreU", model.rpNombreU },
                            { "rpMatricula", model.rpMatricula },
                            { "rpFechaI", model.rpFechaIt.ToString("yyyy-MM-ddTHH:mm:ss") },
                            { "rpFechaT", model.rpFechaTt.ToString("yyyy-MM-ddTHH:mm:ss") },
                            { "rpBeca", model.rpBeca },
                            { "rpBecaL", model.rpBecaL }
                        };

            return await GenerateReportAsync(model.ReportName, parameters);
        }

        public async Task<IActionResult> OnPostGenerateIntellectualPropertyRightsReport([FromForm] IntellectualPropertyRightsreportRequest model)
        {
           
             var   parameters = new Dictionary<string, object>
                        {
                            { "rpFechaDocumento", model.rpFechaDocumentoi.ToString("yyyy-MM-ddTHH:mm:ss") },
                            { "rpArea", model.rpArej },
                            { "rpPuesto", model.rpPuestoi },
                            { "rpNombreColaborador", model.rpNombreColaboradori }
                        };

                return await GenerateReportAsync(model.ReportName, parameters);
            }

        public async Task<IActionResult> OnPostGenerateDatabaseChangeRequestReport([FromForm] DatabaseChangeRequestReport model)
        {
           var  parameters = new Dictionary<string, object>
                {
                    { "rpClave", model.rpClave },
                    { "rpClaveS", model.rpClaveS },
                    { "rpFase", model.rpFase },
                    { "rpFecha", model.rpFechadb?.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpSistema", model.rpSistema },
                    { "rpBD", model.rpBD },
                    { "rpNSolicitante", model.rpNSolicitante },
                    { "rpElemento", model.rpElemento },
                    { "rpVersion", model.rpVersion },
                    { "rpDescripcion", model.rpDescripcion },
                    { "rpJustificacion", model.rpJustificacion },
                    { "rpObservaciones", model.rpObservacionesdb == null ? string.Empty: model.rpObservacionesdb  },
                    { "rpNAfectado1", model.rpNAfectado1 },
                    { "rpNAfectado2", model.rpNAfectado2  == null ? string.Empty: model.rpNAfectado2 },
                    { "rpND", model.rpND },
                    { "rpNDR", model.rpNDR },
                    { "rpRNuevo", model.rpRNuevo },
                    { "rpRCambio", model.rpRCambio },
                    { "rpRDefecto", model.rpRDefecto },
                    { "rpRMejora", model.rpRMejora },
                    { "rpAutorizacion", model.rpAutorizacion },
                    { "rpNoAutorizacion", model.rpNoAutorizacion }
                };

                return await GenerateReportAsync(model.ReportName, parameters);
            }

        public async Task<IActionResult> OnPostGenerateSoftwareDevelopmentRequestReport([FromForm] SoftwareDevelopmentRequest model)
        {
           var  parameters = new Dictionary<string, object>
                 {
                    { "rpFecha", model.rpFechasw?.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpNombre", model.rpNombresw },
                    { "rpArea", model.rpAreasw },
                    { "rpCR", model.rpCR },
                    { "rpNombreS", model.rpNombreS },
                    { "rpNombreS2", model.rpNombreS2 == null ? string.Empty: model.rpNombreS2  },
                    { "rpNombreS3", model.rpNombreS3 == null ? string.Empty: model.rpNombreS3 },
                    { "rpModulo", model.rpModulo },
                    { "rpModulo2", model.rpModulo2  == null ? string.Empty: model.rpModulo2 },
                    { "rpModulo3", model.rpModulo3  == null ? string.Empty: model.rpModulo3 },
                    { "rpComentarios", model.rpComentarios },
                    { "rpComentarios2", model.rpComentarios2  == null ? string.Empty: model.rpComentarios2  },
                    { "rpComentarios3", model.rpComentarios3  == null ? string.Empty: model.rpComentarios3  },
                    { "rpDescripcion", model.rpDescripcionsw },
                    { "rpJustificacion", model.rpJustificacionsw },
                    { "rpComentario", model.rpComentariosw   == null ? string.Empty: model.rpComentariosw },
                    { "rpNombreD", model.rpNombreD },
                    { "rpNombreG", model.rpNombreG },
                    { "rpNombreDS", model.rpNombreDS },
                    { "rpNuevo", model.rpNuevo },
                    { "rpCambio", model.rpCambio },
                    { "rpDefecto", model.rpDefecto },
                    { "rpMejora", model.rpMejora }
                 };

            return await GenerateReportAsync(model.ReportName, parameters);
        }

        public async Task<IActionResult> OnPostGenerateDelayNotificationReport([FromForm] DelayNotificationRequest model)
        {
           var parameters = new Dictionary<string, object>{
                            { "rpFecha", model.rpFechant?.ToString("yyyy-MM-ddTHH:mm:ss") },
                            { "rpNombre", model.rpNombrent },
                            { "rpPuesto", model.rpPuestont },
                            { "rpDepartamento", model.rpDepartamentont },
                            { "rpFechaHoy", model.rpFechaHoy?.ToString("yyyy-MM-ddTHH:mm:ss") },
                            { "rpJornada", model.rpJornada },
                            { "rpSancion", model.rpSancion?.ToString("yyyy-MM-ddTHH:mm:ss") },
                            { "rpNJefe", model.rpNJefe },              
                            { "rpRH", model.rpRH }
                         };
            return await GenerateReportAsync(model.ReportName, parameters);
        }

        public async Task<IActionResult> OnPostGenerateVehicleFailureReport([FromForm] VehicleFailureReportRequest model)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "rpFecha", model.rpFechavh?.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpNombre", model.rpNombrevh },
                    { "rpTipoVehiculo", model.rpTipoVehiculo },
                    { "rpColor", model.rpColor },
                    { "rpMarca", model.rpMarcavh },
                    { "rpModelo", model.rpModelovh },
                    { "rpNPlacas", model.rpNPlacasvh },
                    { "rpAño", model.rpAño },
                    { "rpKilometraje", model.rpKilometraje },
                    { "rpNChasis", model.rpNChasis },
                    { "rpDescripcion", model.rpDescripcionvh }
                };
            return await GenerateReportAsync(model.ReportName, parameters);
        }

        public async Task<IActionResult> OnPostGenerateCollectionMeetingReport([FromForm] CollectionMeetingReportRequest model)
        {
            var parameters = new Dictionary<string, object>
            {
                { "rpAsistentes", model.rpAsistentes },
                { "rpFechaCarteraVencida", model.rpFechaCarteraVencida?.ToString("yyyy-MM-ddTHH:mm:ss") },
                { "rpReporteCarteraVencida", model.rpReporteCarteraVencida },
                { "rpProceso", model.rpProceso == null? string.Empty: model.rpProceso },
                { "rpConvenios", model.rpConvenios == null? string.Empty: model.rpConvenios },
                { "rpDesistimientos", model.rpDesistimientos == null? string.Empty: model.rpDesistimientos }
            };
            return await GenerateReportAsync(model.ReportName, parameters);
        }

        public async Task<IActionResult> OnPostGenerateTemporaryLoanRequestReport([FromForm] TemporaryLoanRequest model)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "rpFecha", model.rpFechapt?.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpNombre", model.rpNombrept },
                    { "rpNomina", model.rpNomina },
                    { "rpFechaI", model.rpFechaIpt?.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpArea", model.rpAreapt },
                    { "rpMotivo", model.rpMotivopt },
                    { "rpCantidadN", model.rpCantidadNpt },
                    { "rpCantidadL", model.rpCantidadLpt },
                    { "rpPrimerPago", model.rpPrimerPago },
                    { "rpFechaU", model.rpFechaUpt?.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "rpObservaciones", model.rpObservacionespt == null ? string.Empty : model.rpObservacionespt },
                    { "rpNoPagos", model.rpNoPagos },
                    { "rpJefe", model.rpJefe },
                    { "rpCapital", model.rpCapital },
                    { "rpDescuento", model.rpDescuento },
                    { "rpModo", model.rpModo },
                    { "rpFormaDPago", model.rpFormaDPago }
                };
            return await GenerateReportAsync(model.ReportName, parameters);
        }

        private async Task<IActionResult> GenerateReportAsync(  string reportName,   IDictionary<string, object> parameters)
        {
            try
            {
                byte[] reportContent = await RenderReport(reportName, parameters, null);

                if (reportContent == null || reportContent.Length == 0)
                    return BadRequest("No se generó el reporte");

                string fileName = $"{reportName}.pdf";
                string folder = Path.Combine(_env.WebRootPath, "signatures");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string path = Path.Combine(folder, fileName);

                await System.IO.File.WriteAllBytesAsync(path, reportContent);

                return new JsonResult(new
                {
                    success = true,
                    url = "/signatures/" + fileName
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        protected async Task<byte[]> RenderReport(string reportName, IDictionary<string, object> parameters, string reportFolder) 
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
                TrustedUserHeader = null,
                Report = reportPath,
            };
            var taskLoadReport = await reportClient.LoadReportAsync(request1);
            execHeader.ExecutionID = taskLoadReport.executionInfo.ExecutionID;


                RS.ParameterValue[] reportParameters = null;

                if (parameters != null && parameters.Count > 0)
                    reportParameters = taskLoadReport.executionInfo.Parameters.Where(x => parameters.ContainsKey(x.Name))
                        .Select(x => new RS.ParameterValue() { Name = x.Name, Value = parameters[x.Name]?.ToString() }).ToArray();

                var request2 = new RS.SetExecutionParametersRequest()
            {
                ExecutionHeader = execHeader,
                TrustedUserHeader = null,//trustedUserHeader,
                Parameters = reportParameters,
                ParameterLanguage = "en-us"
            };

            await reportClient.SetExecutionParametersAsync(request2);//(execHeader, trustedUserHeader, reportParameters, "en-us");
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

        protected async Task<byte[]> RenderReportseeparam(string reportName, IDictionary<string, object> parameters, string reportFolder) 
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

                var request2 = new RS.SetExecutionParametersRequest()
                {
                    ExecutionHeader = execHeader,
                    TrustedUserHeader = null,//trustedUserHeader,
                    Parameters = reportParameters,
                    ParameterLanguage = "en-us"
                };

                await reportClient.SetExecutionParametersAsync(request2);//(execHeader, trustedUserHeader, reportParameters, "en-us");
                const string deviceInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

                var response =
                    await reportClient.RenderAsync(new RS.RenderRequest(execHeader, null, "PDF", deviceInfo));//(execHeader, trustedUserHeader, "PDF", deviceInfo));
                return response.Result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

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


    }
}

