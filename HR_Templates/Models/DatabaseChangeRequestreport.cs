namespace HR_Templates.Models
{    
    public class DatabaseChangeRequestReport
    {
        public string ReportName { get; set; }
        public string rpClave { get; set; }

        public string rpClaveS { get; set; }

        public string rpFase { get; set; }

        public DateTime? rpFechadb { get; set; }

        public string rpSistema { get; set; }

        public string rpBD { get; set; }

        public string rpNSolicitante { get; set; }

        public string rpElemento { get; set; }

        public string rpVersion { get; set; }

        public string rpDescripcion { get; set; }

        public string rpJustificacion { get; set; }

        public string rpObservacionesdb { get; set; }

        public string rpNAfectado1 { get; set; }

        public string rpNAfectado2 { get; set; }

        public string rpND { get; set; }

        public string rpNDR { get; set; }

        public bool rpRNuevo { get; set; }

        public bool rpRCambio { get; set; }

        public bool rpRDefecto { get; set; }

        public bool rpRMejora { get; set; }

        public bool rpAutorizacion { get; set; }

        public bool rpNoAutorizacion { get; set; }
    }
}
