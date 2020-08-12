namespace RestAPI.Models
{
    public class CalcHoursDatabaseSettings : ICalcHours
    {
        public string BankHoursCollection { get; set; }
        public string CompanyCollection { get; set; }
        public string EmployeeCollection { get; set; }
        public string EmployeeHoursCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICalcHours
    {
        string BankHoursCollection { get; set; }
        string CompanyCollection { get; set; }
        string EmployeeCollection { get; set; }
        string EmployeeHoursCollection { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}