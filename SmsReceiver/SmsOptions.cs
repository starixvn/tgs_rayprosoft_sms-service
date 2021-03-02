#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。

using System;

namespace TGS.Sms
{
    public class SmsOptions
    {
        public Uri RabbitMQUrl { get; set; }

        public string UserName { get; set; } = "guest";

        public string Password { get; set; } = "guest";

        public string QueueName { get; set; }

        public string ExchangeRouteKey { get; set; }
    }
}

#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
