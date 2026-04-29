namespace Pdnink.Models
{
    public class VacationRequest
    {
        public string Datenow { get; set; }
        public string EmployeeName { get; set; }
        public string PayrollNumber { get; set; }
        public string HireDate { get; set; }
        public int YearsInCompany { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }

        public string Periodo { get; set; }
        public int EntitledDays { get; set; }
        public int DaysTaken { get; set; }
        public int DaysRequested { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int RemainingDays { get; set; }
        public string Observations { get; set; }
        public string BossName { get; set; }
        public string RHBoss { get; set; }

        
    }

}

