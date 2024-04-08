namespace ThaiDuongScada.Api.Application.Queries.Moulds;
public class MouldViewModel
{
    public MouldViewModel(string mouldId, string mouldName, int cav, string deviceType, int standardOutput)
    {
        MouldId = mouldId;
        MouldName = mouldName;
        Cav = cav;
        DeviceType = deviceType;
        StandardOutput = standardOutput;
    }

    public string MouldId { get; set; }
    public string MouldName { get; set; }
    public int Cav { get; set; }
    public string DeviceType { get; set; }
    public int StandardOutput { get; set; }
}
