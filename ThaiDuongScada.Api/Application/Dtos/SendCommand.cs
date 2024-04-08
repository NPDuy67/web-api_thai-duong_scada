namespace ThaiDuongScada.Api.Application.Dtos;

public class SendCommand
{
    public SendCommand(DateTime timeStamp, int command)
    {
        TimeStamp = timeStamp;
        Command = command;
    }

    public DateTime TimeStamp { get; set; }
    public int Command { get; set; }
}
