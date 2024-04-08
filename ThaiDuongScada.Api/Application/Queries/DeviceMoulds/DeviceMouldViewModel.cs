using ThaiDuongScada.Api.Application.Queries.Moulds;

namespace ThaiDuongScada.Api.Application.Queries.DeviceMoulds;
public class DeviceMouldViewModel
{
    public DeviceMouldViewModel(string deviceId, string mouldId, int mouldSlot, int previousShotCount, MouldViewModel mouldInfo)
    {
        DeviceId = deviceId;
        MouldId = mouldId;
        MouldSlot = mouldSlot;
        PreviousShotCount = previousShotCount;
        MouldInfo = mouldInfo;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DeviceMouldViewModel() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public string DeviceId { get; set; }
    public string MouldId { get; set; }
    public int MouldSlot { get; set; }
    public int PreviousShotCount { get; set; }
    public MouldViewModel MouldInfo { get; set; }
}
