using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.X509;

namespace HR_Templates.Pages
{
    public class FormatosModel : PageModel
    {
        [BindProperty]
        public string FormatoSeleccionado { get; set; }

        public List<string> Formatos { get; set; }


        private static readonly HashSet<string> formatosExcel_xls = new HashSet<string>
        {
            "Actualización de datos generales",
            "Cambio de puesto o área del personal"

        };

        private static readonly HashSet<string> formatosExcel_xlsx = new HashSet<string>
        {            
            
            "AUTORIZACIÓN DE PAGO DE BONO",
            "Cédula COVID-19",
            "Cédula de capacitación e inducción al puesto",
            "Check list de documentación del becario",
            "Check list documentación del personal",
            "Check list salida de personal",
            "Consulta de expedientes del personal",      
            "Designación de beneficiarios del becario",
            "Designación de beneficiarios",
            "Entrevista de salida",
            "Evaluación del curso de capacitación",
            "Finiquito o liquidación",
            "Formato de evaluación del colaborador de nuevo ingreso",
            "Formato de evaluación del colaborador de nuevo ingreso",
            "Guía de inducción a personal de nuevo ingreso",
            "Guía de inducción de becario",
            "Perfil y descripción del puesto",
            "PERMISO DE PATERNIDAD",
            "Permisos para atención de asuntos de trabajo",
            "Permisos para atención de asuntos personales en el horario laboral de trabajo",
            "Plan o proyecto de trabajo para el becario",
            "Programación de jornada laboral",
            "Reporte de entrevista de selección becario",
            "Reporte de entrevista de selección",         
            "Requerimiento de becario",
            "Requisición de personal",
            "Solicitud de baja de personal",
            "Solicitud de baja del becario",
            "Solicitud de cursos de capacitación",
            "Solicitud de empleo",
            "SOLICITUD DE PRESTAMO AL PERSONAL",          
            "Solicitud de vacaciones",
            "SOLICITUD Y AUTORIZACIÓN DE INCREMENTO DE SUELDO",
           
        };

        public void OnGet()
        {
            Formatos = ObtenerFormatos();
        }

        public IActionResult OnPost()
        {
            Formatos = ObtenerFormatos();

            if (string.IsNullOrEmpty(FormatoSeleccionado))
            {
                ModelState.AddModelError("", "Selecciona un formato");
                return Page();
            }

            bool esExcel_xlsx = formatosExcel_xlsx.Contains(FormatoSeleccionado);
            bool esExcel_xls = formatosExcel_xls.Contains(FormatoSeleccionado);

            if (esExcel_xlsx)
                return RedirectToPage("Spreadsheet", new { name = FormatoSeleccionado, Extension = "xlsx" });
            else if(esExcel_xls)
            {
                return RedirectToPage("Spreadsheet", new { name = FormatoSeleccionado, Extension = "xls" });
            }
            else//Tomar desde el reporting service el reporte 
                return RedirectToPage("RichEdit", new { name = FormatoSeleccionado });
        }

        private List<string> ObtenerFormatos()
        {
            return new List<string>
            {
                 "Aceptación del reglamento de becarios",
            "Actualización de datos generales",
            "AUTORIZACIÓN DE PAGO DE BONO",
            "Aviso de privacidad",
            "Cambio de puesto o área del personal",
            "Carta de aceptación de pago de beca",
            "Carta de aceptación del código de ética",
            "Carta de conocimiento y aceptación del manual de cumplimiento",
            "Carta de reserva y confidencialidad",
            "Carta de trabajo en instituciones financieras e inhabilitaciones",
            "Cédula COVID-19",
            "Cédula de capacitación e inducción al puesto",
            "Check list de documentación del becario",
            "Check list documentación del personal",
            "Check list salida de personal",
            "Consulta de expedientes del personal",
            "Convenio individual de beca",
            "Derechos intelectuales",
            "Designación de beneficiarios del becario",
            "Designación de beneficiarios",
            "Entrevista de salida",
            "Evaluación del curso de capacitación",
            "Finiquito o liquidación",
            "Formato de evaluación del colaborador de nuevo ingreso",
            "Formato de evaluación del colaborador de nuevo ingreso",
            "Guía de inducción a personal de nuevo ingreso",
            "Guía de inducción de becario",
            "Lista general de asistencia",
            "Perfil y descripción del puesto",
            "PERMISO DE PATERNIDAD",
            "Permisos para atención de asuntos de trabajo",
            "Permisos para atención de asuntos personales en el horario laboral de trabajo",
            "Plan o proyecto de trabajo para el becario",
            "Postulación de compra de vehículo utilitario",
            "Programación de jornada laboral",
            "Reporte de entrevista de selección becario",
            "Reporte de entrevista de selección",
            "Reporte de evaluación de competencias del becario",
            "Requerimiento de becario",
            "Requisición de personal",
            "Solicitud de baja de personal",
            "Solicitud de baja del becario",
            "Solicitud de cursos de capacitación",
            "Solicitud de empleo",
            "SOLICITUD DE PRESTAMO AL PERSONAL",
            "Solicitud de vacaciones",         
            "SOLICITUD Y AUTORIZACIÓN DE INCREMENTO DE SUELDO",
            };
        }
    }
}