namespace ThaiDuongScada.Api.Application.Commands.DeviceMouldsl;

[DataContract]
public class UpdateDeviceMouldViewModel
{
    [DataMember]
    public string? MouldId { get; set; }
    [DataMember]
    public int PreviousShotCount { get; set; }
}
