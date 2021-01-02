using System.Collections.Generic;

namespace Chat.Api.Core.Configs
{
    public class ApiKeyConfig
    {
        public IEnumerable<ApiKey> ApiKeys { get; set; }
    }

    public class ApiKey
    {
        public string AppId { get; set; }
        public string Key { get; set; }
    }
}