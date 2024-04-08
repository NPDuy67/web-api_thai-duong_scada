namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;

public class ShiftReportAverageViewModel
{
    public ShiftReportAverageViewModel(double oEE, double p, double a, double q)
    {
        OEE = oEE;
        P = p;
        A = a;
        Q = q;
    }

    public double OEE { get; set; }
    public double P { get; set; }
    public double A { get; set; }
    public double Q { get; set; }
}
