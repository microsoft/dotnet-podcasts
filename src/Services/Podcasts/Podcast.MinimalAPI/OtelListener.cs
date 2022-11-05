using System.Collections.Concurrent;
using System.Diagnostics.Tracing;

// Just for debugging things
class OtelListener : EventListener
{
    public static ILoggerFactory _factory;
    private ConcurrentDictionary<string, ILogger> _loggerMap = new();

    public OtelListener()
    {

    }

    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (eventSource.Name.StartsWith("OpenTelemetry"))
        {
            if (_loggerMap.TryGetValue(eventSource.Name, out _)) return;
            ILogger CreateAndRegister(string name)
            {
                var l = _factory.CreateLogger(name);
                base.EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All);
                return l;
            }
            var logger = _loggerMap.AddOrUpdate(eventSource.Name, CreateAndRegister, (name, existing) => existing);
        }
        base.OnEventSourceCreated(eventSource);
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        if (_loggerMap[eventData.EventSource.Name] is { } logger)
        {
            logger.Log(LogLevel.Information, eventData.Message, (eventData.Payload?.ToArray() ?? Array.Empty<object>()));
        }
    }
}