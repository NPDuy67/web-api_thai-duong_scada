namespace ThaiDuongScada.Host.Application.Commands;
public class DefectCountNotification : INotification
{
    public string DeviceId { get; set; }
    public int MouldSlot { get; set; }
    public int DefectCount { get; set; }
    public DateTime Timestamp { get; set; }

    public DefectCountNotification(string deviceId, int mouldSlot, int defectCount, DateTime timestamp)
    {
        DeviceId = deviceId;
        MouldSlot = mouldSlot;
        DefectCount = defectCount;
        Timestamp = timestamp;
    }
}
