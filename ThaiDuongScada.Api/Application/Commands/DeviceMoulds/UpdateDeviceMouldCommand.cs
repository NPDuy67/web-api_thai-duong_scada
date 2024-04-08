using System.Globalization;

namespace ThaiDuongScada.Api.Application.Commands.DeviceMoulds;
public class UpdateDeviceMouldCommand : IRequest<bool>
{
    public string DeviceId { get; set; }
    public int MouldSlot { get; set; }
    public string MouldId { get; set; }
    public int PreviousShotCount { get; set; }

    public UpdateDeviceMouldCommand(string deviceId, int mouldSlot, string mouldId, int previousShotCount)
    {
        DeviceId = deviceId;
        MouldSlot = mouldSlot;
        MouldId = mouldId;
        PreviousShotCount = previousShotCount;
    }
}
