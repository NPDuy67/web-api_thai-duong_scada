namespace ThaiDuongScada.Api.Application.Commands.DeviceMoulds;

[DataContract]
public class UpdateShiftTimeViewModel
{
    [DataMember]
    public EShiftDuration ShiftDuration { get; set; }
}
