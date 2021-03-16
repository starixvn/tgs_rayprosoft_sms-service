using System;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TGS.SmsSender
{
    internal class RabbitMQManager : IDisposable
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private bool _disposedValue;

        public IModel? SmsChannel { get; private set; }

        public RabbitMQManager(IOptions<SmsOptions> optionsAccessor)
        {
            if (optionsAccessor is null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            var options = optionsAccessor.Value;

            _factory = new ConnectionFactory()
            {
                Uri = options.RabbitMQUrl,
                UserName = options.UserName,
                Password = options.Password,
                VirtualHost = options.VirtualHost,
                DispatchConsumersAsync = true
            };

            _connection = _factory.CreateConnection();
            SmsChannel = _connection.CreateModel();
        }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    SmsChannel?.Close();
                    SmsChannel?.Dispose();
                    SmsChannel = null;

                    _connection.Close();
                    _connection.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
