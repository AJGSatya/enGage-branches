using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;

namespace enGage.Web.Helper
{
    public static class Timeline
    {
        public static IDisposable Capture(string eventName, string appName = "enGage")
        {
#pragma warning disable 618
            var timer = GlimpseConfiguration.GetConfiguredTimerStrategy()();
            if (timer == null)
                return null;
            var broker = GlimpseConfiguration.GetConfiguredMessageBroker();
            if (broker == null)
                return null;
#pragma warning restore 618
            return new TimelineCapture(timer, broker, eventName, appName);
        }
    }

    public class TimelineCapture : IDisposable
    {
        private readonly string _eventName;
        private readonly IExecutionTimer _timer;
        private readonly IMessageBroker _broker;
        private readonly TimeSpan _startOffset;
        private readonly string _appName;

        public TimelineCapture(IExecutionTimer timer, IMessageBroker broker, string eventName, string appName)
        {
            _timer = timer;
            _appName = appName;
            _broker = broker;
            _eventName = eventName;
            _startOffset = _timer.Start();
        }

        public void Dispose()
        {
            _broker.Publish(new TimelineMessage(_eventName, _appName, _timer.Stop(_startOffset)));
        }
    }

    public class TimelineMessage : ITimelineMessage
    {
        private static readonly Dictionary<string, TimelineCategoryItem> Mappings = new Dictionary<string, TimelineCategoryItem>();

        public TimelineMessage(string eventName, string appName, TimerResult result)
        {
            Id = Guid.NewGuid();
            EventName = eventName;

            if (!Mappings.ContainsKey(appName))
                Mappings[appName] = new TimelineCategoryItem(appName, "green", "blue");
            EventCategory = Mappings[appName];

            Offset = result.Offset;
            StartTime = result.StartTime;
            Duration = result.Duration;
        }

        public Guid Id { get; private set; }
        public TimeSpan Offset { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string EventName { get; set; }
        public TimelineCategoryItem EventCategory { get; set; }
        public string EventSubText { get; set; }
    }
}