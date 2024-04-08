using ThaiDuongScada.Api.Application.Queries.DeviceMoulds;
using ThaiDuongScada.Api.Application.Queries.Moulds;
using ThaiDuongScada.Api.Application.Queries.ShiftReports;

namespace ThaiDuongScada.Api.Application.Queries.Devices;

public class DeviceViewModel
{
    public DeviceViewModel(string deviceId, string deviceType, string deviceName, int minTemperature, int maxTemperature, double minPressure, double maxPressure, bool manipular, int displayPriority, IEnumerable<ShiftReportViewModel> shiftReports, IEnumerable<DeviceMouldViewModel> moulds)
    {
        DeviceId = deviceId;
        DeviceType = deviceType;
        DeviceName = deviceName;
        MinTemperature = minTemperature;
        MaxTemperature = maxTemperature;
        MinPressure = minPressure;
        MaxPressure = maxPressure;
        Manipular = manipular;
        DisplayPriority = displayPriority;
        ShiftReports = shiftReports;
        Moulds = moulds;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DeviceViewModel() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public string DeviceId { get; set; }
    public string DeviceType { get; set; }
    public string DeviceName { get; set; }
    public int MinTemperature { get; set; }
    public int MaxTemperature { get; set; }
    public double MinPressure { get; set; }
    public double MaxPressure { get; set; }
    public bool Manipular { get; set; }
    public int DisplayPriority { get; set; }
    public IEnumerable<ShiftReportViewModel> ShiftReports { get; set; }
    public IEnumerable<DeviceMouldViewModel> Moulds { get; set; }
}
