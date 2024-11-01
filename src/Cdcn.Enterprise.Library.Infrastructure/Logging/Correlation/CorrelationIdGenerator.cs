using Cdcn.Enterprise.Library.Domain.Common;

namespace Cdcn.Enterprise.Library.Logging.Correlation;

public class CorrelationIdGenerator : ICorrelationIdGenerator
{
    private string _correlationId = Guid.NewGuid().ToString("D");
    public string Get() => _correlationId;

    public void Set(string correlationId) => _correlationId = correlationId;

}