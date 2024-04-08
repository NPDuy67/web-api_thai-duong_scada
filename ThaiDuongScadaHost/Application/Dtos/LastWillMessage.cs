namespace ThaiDuongScada.Host.Application.Dtos;
public class LastWillMessage
{
    public List<Machine> Machines { get; set; }

    public LastWillMessage()
    {
        Machines = new List<Machine>();
    }

    public class Machine
    {
        public string MachineId { get; set; }

        public Machine(string machineId)
        {
            MachineId = machineId;
        }
    }
}
