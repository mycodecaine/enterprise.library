namespace Cdcn.Enterprise.Library.Infrastructure.MessageQueue.RabbitMQ.Settings
{
    public class RabbitMqSetting
    {
        public const string DefaultSectionName = "RabbitMq";


        public string ConnectionString { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the host name.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the queue name.
        /// </summary>
        public string QueueName { get; set; }
    }
}
