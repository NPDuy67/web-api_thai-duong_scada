namespace ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
public class Shot
{
    public double InjectionTime { get; private set; }
    public double InjectionCycle { get; private set; }
    public DateTime TimeStamp { get; private set; }

    public Shot(double injectionTime, double injectionCycle, DateTime timeStamp)
    {
        InjectionTime = injectionTime;
        InjectionCycle = injectionCycle;
        TimeStamp = timeStamp;
    }
}
