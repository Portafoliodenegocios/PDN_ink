using Azure.Core;
using DevExpress.Charts.Native;
using HR_Templates.Helpers;
using HR_Templates.Models;
using HR_Templates.Proxys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;


namespace HR_Templates.Pages
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FormatosModel : PageModel
    {
        [BindProperty]
        public string Document_Option_Id { get; set; }

        [BindProperty]
        public bool Is_SSRS { get; set; }

        [BindProperty]
        public string _token { get; set; }

        public List<FormatoVM> Formatos { get; set; }

        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;

        public string  stoken { get; set; }
        public FormatosModel(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }


        public async Task OnGet()
        {          

            ViewData["_Token"] = HttpContext.Request.Query["access_token"].ToString();

            var result = await  PDNInk_vw_Document_Type_OptionCBR
                               .Instance
                               .GetBy("Is_Active = True AND Report_Type_Id = 1 ");

            Formatos = result
                             .Select(r => new FormatoVM
                                {
                                    Id = r.Document_Option_Id,
                                    Name = r.Document_Option ,
                                    Is_SSRS = r.Report_Type_Id == 1 ? true : false
                                })
                              .OrderBy(x => x.Name)
                              .ToList();
                            
        }

     

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Document_Option_Id))
            {
                ModelState.AddModelError("", "Selecciona un formato");
                return Page();
            }

            //bool esExcel_xlsx = formatosExcel_xlsx.Contains(Document_Option_Id);
            //bool esExcel_xls = formatosExcel_xls.Contains(Document_Option_Id);


            string  sFullname = User.Claims.FirstOrDefault(x => x.Type == "FullName").Value;

            if (Is_SSRS)
                return RedirectToPage("VisorReport", new { id = Document_Option_Id, name = sFullname }); 
            else 
               return RedirectToPage("Spreadsheet", new { id = Document_Option_Id, Extension = "xlsx" });

            //else if(esExcel_xls)
            //{
            //    return RedirectToPage("Spreadsheet", new { name = Document_Option_Id, Extension = "xls" });
            //}
            //else//Tomar desde el reporting service el reporte 
               
            
        }

        
        //private async IActionResult GetDocumentsClassification()
        //{

        //    var result = await PDNInk_cat_Document_ClassificationCBR.Instance.GetBy($" Is_Active = True AND Document_Classification_Id = 2");

        //    return Ok(result);
 
          
        //}

        private async Task<IActionResult> GetDocumentsClassification()
        {
            var result = await PDNInk_cat_Document_TypeCBR.Instance.GetBy("Is_Active = True AND Document_Classification_Id = 2");

            var data = result 
                .Select(r => new
                {
                    id = r.Document_Type_Id, 
                    name = r.Document_Type_Description   
                })
                .OrderBy(x => x.name);

            return  new JsonResult(data);
        }

        //public async Task<JsonResult> OnGetDocumentsClassification()
        //{
        //    var data = await GetDocumentsClassificationData();
        //    return new JsonResult(data);
        //}

        //private async Task<IEnumerable<object>> GetDocumentsClassificationData()
        //{
        //    var result = await  PDNInk_cat_Document_TypeCBR 
        //        .Instance
        //        .GetBy("Is_Active = True AND Document_Classification_Id = 2");

        //    return result
        //        .Select(r => new
        //        {
        //            id = r.Document_Classification_Id,
        //            name = r.Document_Classification
        //        })
        //        .OrderBy(x => x.name);
        //}


    }
}

//return new List<string>
//{
//     "Aceptación del reglamento de becarios",
//"Actualización de datos generales",
//"AUTORIZACIÓN DE PAGO DE BONO",
//"Aviso de privacidad",
//"Cambio de puesto o área del personal",
//"Carta de aceptación de pago de beca",
//"Carta de aceptación del código de ética",
//"Carta de conocimiento y aceptación del manual de cumplimiento",
//"Carta de reserva y confidencialidad",
//"Carta de trabajo en instituciones financieras e inhabilitaciones",
//"Cédula COVID-19",
//"Cédula de capacitación e inducción al puesto",
//"Check list de documentación del becario",
//"Check list documentación del personal",
//"Check list salida de personal",
//"Consulta de expedientes del personal",
//"Convenio individual de beca",
//"Derechos intelectuales",
//"Designación de beneficiarios del becario",
//"Designación de beneficiarios",
//"Entrevista de salida",
//"Evaluación del curso de capacitación",
//"Finiquito o liquidación",
//"Formato de evaluación del colaborador de nuevo ingreso",
//"Formato de evaluación del colaborador de nuevo ingreso",
//"Guía de inducción a personal de nuevo ingreso",
//"Guía de inducción de becario",
//"Lista general de asistencia",
//"Perfil y descripción del puesto",
//"PERMISO DE PATERNIDAD",
//"Permisos para atención de asuntos de trabajo",
//"Permisos para atención de asuntos personales en el horario laboral de trabajo",
//"Plan o proyecto de trabajo para el becario",
//"Postulación de compra de vehículo utilitario",
//"Programación de jornada laboral",
//"Reporte de entrevista de selección becario",
//"Reporte de entrevista de selección",
//"Reporte de evaluación de competencias del becario",
//"Requerimiento de becario",
//"Requisición de personal",
//"Solicitud de baja de personal",
//"Solicitud de baja del becario",
//"Solicitud de cursos de capacitación",
//"Solicitud de empleo",
//"SOLICITUD DE PRESTAMO AL PERSONAL",
//"Solicitud de vacaciones",         
//"SOLICITUD Y AUTORIZACIÓN DE INCREMENTO DE SUELDO",
//};