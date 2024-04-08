namespace ThaiDuongScada.Infrastructure.Communication.Exceptions;
[Serializable]
public class MqttConnectionException : Exception
{
    public MqttConnectionException(string? message) : base(message)
    {
    }

    public MqttConnectionException() : base()
    {
    }

    public MqttConnectionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected MqttConnectionException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}