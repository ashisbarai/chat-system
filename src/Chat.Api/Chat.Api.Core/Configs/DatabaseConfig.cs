namespace Chat.Api.Core.Configs
{
    public class DatabaseConfig
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string ConnectionString { get; set; }
    }
}