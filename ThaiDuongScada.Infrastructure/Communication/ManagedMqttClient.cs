using MQTTnet.Client;
using MQTTnet;
using Microsoft.Extensions.Options;
using ThaiDuongScada.Infrastructure.Communication.Exceptions;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ThaiDuongScada.Infrastructure.Communication;
public class ManagedMqttClient
{
    public MqttOptions Options { get; set; }
    public bool IsConnected => _mqttClient is not null && _mqttClient.IsConnected;

    public event Func<MqttMessage, Task>? MessageReceived;

    private IMqttClient? _mqttClient;
    private readonly Timer _reconnectTimer;
    private List<string> SubScribeTopics { get; set; } = new List<string>();

    public ManagedMqttClient(IOptions<MqttOptions> options)
    {
        Options = options.Value;
        _reconnectTimer = new Timer(10000);
        _reconnectTimer.Elapsed += OnReconnectTimerElapsed;
    }

    public async Task ConnectAsync()
    {
        _reconnectTimer.Enabled = false;
        if (_mqttClient is not null)
        {
            await _mqttClient.DisconnectAsync();
            _mqttClient.Dispose();
        }

        _mqttClient = new MqttFactory().CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(Options.Host, Options.Port)
            .WithTimeout(TimeSpan.FromSeconds(Options.CommunicationTimeout))
            .WithClientId(Options.ClientId)
            .WithCredentials(Options.UserName, Options.Password)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(Options.KeepAliveInterval));

        _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceived;
        _mqttClient.DisconnectedAsync += OnDisconnected;

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(Options.CommunicationTimeout));
        var result = await _mqttClient.ConnectAsync(mqttClientOptions.Build(), timeout.Token);

        if (result.ResultCode != MqttClientConnectResultCode.Success)
        {
            _reconnectTimer.Enabled = true;
        }
        else
        {
            foreach (var topic in SubScribeTopics)
            {
                await _mqttClient.SubscribeAsync(topic);
            }
            Console.WriteLine("Connected");
        }
    }

    public async Task DisconnectAsync()
    {
        await _mqttClient.DisconnectAsync();
    }

    public async Task Subscribe(string topic)
    {
        if (_mqttClient is null)
        {
            throw new InvalidOperationException("MQTT Client is not connected.");
        }

        var topicFilter = new MqttTopicFilterBuilder()
            .WithTopic(topic)
            .Build();

        var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
            .WithTopicFilter(topicFilter)
            .Build();

        var result = await _mqttClient.SubscribeAsync(subscribeOptions);

        foreach (var subscription in result.Items)
        {
            if (subscription.ResultCode != MqttClientSubscribeResultCode.GrantedQoS0 &&
                subscription.ResultCode != MqttClientSubscribeResultCode.GrantedQoS1 &&
                subscription.ResultCode != MqttClientSubscribeResultCode.GrantedQoS2)
            {
                Console.WriteLine($"MQTT Client Subscription {subscription.TopicFilter.Topic} Failed: {subscription.ResultCode}");
                SubScribeTopics.Add(topic);
            }
        }
    }

    public async Task Publish(string topic, string payload, bool retainFlag)
    {
        if (_mqttClient is null)
        {
            throw new InvalidOperationException("MQTT Client is not connected.");
        }

        var applicationMessageBuilder = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithRetainFlag(retainFlag)
            .WithPayload(payload);

        var applicationMessage = applicationMessageBuilder.Build();

        var result = await _mqttClient.PublishAsync(applicationMessage);

        if (result.ReasonCode != MqttClientPublishReasonCode.Success)
        {
            Console.WriteLine($"MQTT Client Publish {applicationMessage.Topic} Failed: {result.ReasonCode}");
        }
    }

    private async Task OnDisconnected(MqttClientDisconnectedEventArgs eventArgs)
    {
        await ConnectAsync();
    }

    private async void OnReconnectTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        await ConnectAsync();
    }

    private async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
    {
        var topic = e.ApplicationMessage.Topic;
        var payload = e.ApplicationMessage.ConvertPayloadToString();

        if (MessageReceived is not null)
        {
            await MessageReceived(new MqttMessage(topic, payload));
        }
    }
}
