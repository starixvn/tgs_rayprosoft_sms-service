using System.Diagnostics.Tracing;

namespace TGS.SmsService.Metrics
{
    internal static class SmsApiMetrics
    {
        public static EventSource EventSource { get; }

        public static IncrementingEventCounter SendSmsRequestCounter { get; }

        static SmsApiMetrics()
        {
            EventSource = new EventSource("TGS.SmsService");

            SendSmsRequestCounter = new IncrementingEventCounter("SendSmsRequest", EventSource);
        }

        public static void Init()
        {
        }

        public static void IncrementSendSmsRequest()
        {
            SendSmsRequestCounter.Increment();
        }
    }
}
