namespace HR_Templates.Models
{
    public class TemporaryLoanRequest
    {
        public string ReportName { get; set; }

        public DateTime? rpFechapt { get; set; }

        public string rpNombrept { get; set; }

        public int rpNomina { get; set; }

        public DateTime? rpFechaIpt { get; set; }

        public string rpAreapt { get; set; }

        public string rpMotivopt { get; set; }

        public int rpCantidadNpt { get; set; }

        public string rpCantidadLpt { get; set; }

        public int rpPrimerPago { get; set; }

        public DateTime? rpFechaUpt { get; set; }

        public string rpObservacionespt { get; set; }

        public int rpNoPagos { get; set; }

        public string rpJefe { get; set; }

        public string rpCapital { get; set; }

        public float rpDescuento { get; set; }

        public bool rpModo { get; set; }

        public bool rpFormaDPago { get; set; }
    }
}
