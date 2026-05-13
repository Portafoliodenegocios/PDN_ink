namespace HR_Templates.Models
{
    public class PositionChangeReportRequest
    {
        public string ReportName { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }

        public DateTime rpFechaCambio { get; set; }
        public int rpNEmpleadop { get; set; }
        public string rpDireccion { get; set; }

        public string rpApellidoP { get; set; }
        public string rpApellidoM { get; set; }
        public string rpNombre { get; set; }

        public int rpNSS { get; set; }
        public DateTime rpFechaIngreso { get; set; }

        public bool rpReubicacion { get; set; }
        public string rpDonde { get; set; }

        public string rpNivel { get; set; }
        public string rpHoraInicio { get; set; }
        public string rpHoraFinal { get; set; }

        public string rpNombreNuevoPuesto { get; set; }
        public string rpNombreAreaNueva { get; set; }
        public string rpObjetivoCambio { get; set; }

        public string rpObservaciones { get; set; }

        public string rpJefeAnterior { get; set; }
        public string rpCapitalHumano { get; set; }
        public string rpNuevoJefe { get; set; }
    }
}