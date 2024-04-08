using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;

namespace ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
public class ShiftReport : IAggregateRoot, IComparable<ShiftReport>
{
    public int Id { get; set; }
    public string DeviceId { get; set; }
    public Device Device { get; set; }
    public int MouldSlot { get; set; }
    public int ShiftNumber { get; set; }
    public DateTime Date { get; set; }
    public EShiftDuration ShiftDuration { get; set; }
    public List<Shot> Shots { get; set; }
    public int ProductCount { get; set; }
    public int DefectCount { get; set; }
    public double ProductPercentage { get; set; }
    public double AverageInjectionCycle => Shots.Count > 0 ? Shots.Average(s => s.InjectionCycle) : 0;
    public double AverageInjectionTime => Shots.Count > 0 ? Shots.Average(s => s.InjectionTime) : 0;
    public double TotalInjectionCycle => Shots.Sum(x => x.InjectionCycle);
    public double TotalInjectionTime => Shots.Sum(x => x.InjectionTime);
    public TimeSpan ElapsedTime => ShiftTimeHelper.GetShiftElapsedTime(Date, ShiftDuration, ShiftNumber);
    public double A
    {
        get
        {
            double a = Shots.Count > 0 ? TimeSpan.FromSeconds(TotalInjectionCycle) / ElapsedTime : 0;
            a = (a > 1) ? 1 : a;
            return a;
        }
    }
    public double Q => Shots.Count > 0 ? (double)(ProductCount - DefectCount) / (double)ProductCount : 0;
    public double P => Shots.Count > 0 ? TotalInjectionTime / TotalInjectionCycle : 0;
    public double OEE => Shots.Count > 0 ? A * P * Q : 0;
    public double L => AverageInjectionCycle - AverageInjectionTime;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ShiftReport() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ShiftReport(int mouldSlot, int shiftNumber, DateTime date, Device device, EShiftDuration shiftDuration)
    {
        MouldSlot = mouldSlot;
        ShiftNumber = shiftNumber;
        Date = date;
        Device = device;
        DeviceId = device.DeviceId;
        Shots = new List<Shot>();
        ShiftDuration = shiftDuration;
    }

    public ShiftReport(int mouldSlot, Device device, EShiftDuration shiftDuration, DateTime time)
    {
        var shiftNumber = ShiftTimeHelper.GetShiftNumber(time, shiftDuration);
        var date = ShiftTimeHelper.GetShiftDate(time, shiftDuration);
        MouldSlot = mouldSlot;
        ShiftNumber = shiftNumber;
        Date = date;
        Device = device;
        DeviceId = device.DeviceId;
        Shots = new List<Shot>();
        ShiftDuration = shiftDuration;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ShiftReport(int shiftNumber, DateTime date, string deviceId, int mouldSlot, int productCount, double productPercentage, List<Shot> shots)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        ShiftNumber = shiftNumber;
        Date = date;
        DeviceId = deviceId;
        MouldSlot = mouldSlot;
        ProductCount = productCount;
        ProductPercentage = productPercentage;
        Shots = shots;
    }

    public void AddShot(double injectionTime, double injectionCycle, DateTime timestamp)
    {
        if (!Shots.Any(x => x.TimeStamp == timestamp))
        {
            var shot = new Shot(injectionTime, injectionCycle, timestamp);
            Shots.Add(shot);
        }
    }

    public void SetProductCount(int productCount)
    {
        ProductCount = productCount;
    }

    public void SetProductPercentage(double productPercentage) 
    { 
        ProductPercentage = productPercentage;
    }

    public void SetDefectCount(int defectCount)
    {
        DefectCount = defectCount;
    }

    public int CompareTo(ShiftReport other)
    {
        return this.DeviceId.CompareTo(other.DeviceId);
    }
}
