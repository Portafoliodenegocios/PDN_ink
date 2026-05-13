namespace HR_Templates.Models
{
    public class BonoReportRequest
    {
        public string ReportName { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }

        public DateTime rpFecha { get; set; }

        public string rpNombreEmpleado { get; set; }
        public string rpPuesto { get; set; }
        public string rpAreab { get; set; }
        public int rpNEmpleadob { get; set; }

        public string rpMotivoBono { get; set; }
        public int rpMontoBono { get; set; }

        public DateTime rpPrimerFecha { get; set; }
        public DateTime rpSegundaFecham { get; set; }

        public int rpNQuincenas { get; set; }
        public int rpNMeses { get; set; }

        public string rpObservaciones { get; set; }
    
    }
}
