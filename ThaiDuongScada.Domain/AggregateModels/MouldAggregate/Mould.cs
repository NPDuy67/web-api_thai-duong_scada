namespace ThaiDuongScada.Domain.AggregateModels.MouldAggregate;
public class Mould: IAggregateRoot
{
    public string MouldId { get; set; }
    public string MouldName { get; set; }
    public int Cav { get; set; }
    public string DeviceType { get; set; }
    public int StandardOutput { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Mould() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Mould(string mouldId, string mouldName, int cav, string deviceType, int standardOutput)
    {
        MouldId = mouldId;
        MouldName = mouldName;
        Cav = cav;
        DeviceType = deviceType;
        StandardOutput = standardOutput;
    }
}
