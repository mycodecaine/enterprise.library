namespace Cdcn.Enterprise.Library.Domain.Common
{
    /// <summary>
    /// Generate Correlation Id
    /// Notes : https://chatgpt.com/share/6799684c-2d80-8007-947a-23e1736519ac
    /// </summary>
    public interface ICorrelationIdGenerator
    {
        string Get();
        void Set(string correlationId);
    }
}
