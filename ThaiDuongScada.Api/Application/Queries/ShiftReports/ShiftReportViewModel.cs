namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;
public class ShiftReportViewModel
{
    public ShiftReportViewModel(int id, int mouldSlot, double oEE, double p, double a, double q, double l, int shiftNumber, DateTime date, string deviceId, int productCount, int defectCount, double productPercentage, double averageInjectionCycle, double averageInjectionTime)
    {
        Id = id;
        MouldSlot = mouldSlot;
        OEE = oEE;
        P = p;
        A = a;
        Q = q;
        L = l;
        ShiftNumber = shiftNumber;
        Date = date;
        DeviceId = deviceId;
        ProductCount = productCount;
        DefectCount = defectCount;
        ProductPercentage = productPercentage;
        AverageInjectionCycle = averageInjectionCycle;
        AverageInjectionTime = averageInjectionTime;
    }

    public int Id { get; set; }
    public int MouldSlot { get; set; }
    public double OEE { get; set; }
    public double P { get; set; }
    public double A { get; set; }
    public double Q { get; set; }
    public double L { get; set; }
    public int ShiftNumber { get; set; }
    public DateTime Date { get; set; }
    public string DeviceId { get; set; }
    public int ProductCount { get; set; }
    public int DefectCount { get; set; }
    public double ProductPercentage { get; set; }
    public double AverageInjectionCycle { get; set; }
    public double AverageInjectionTime { get; set; }
    public int StandardProductCount => ProductCount > 0 ? (int)((double)ProductCount / ProductPercentage) : 0;
}
