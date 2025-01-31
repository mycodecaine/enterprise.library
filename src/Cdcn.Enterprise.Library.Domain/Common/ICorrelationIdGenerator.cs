namespace Cdcn.Enterprise.Library.Domain.Common
{
    /// <summary>
    /// Generate Correlation Id
    /// Notes : https://chatgpt.com/share/6799684c-2d80-8007-947a-23e1736519ac
    /// </summary>
    public interface ICorrelationIdGenerator
    {
        /// <summary>
        /// Gets the current correlation ID.
        /// </summary>
        /// <returns>The current correlation ID.</returns>
        string Get();

        /// <summary>
        /// Sets the correlation ID.
        /// </summary>
        /// <param name="correlationId">The correlation ID to set.</param>
        void Set(string correlationId);
    }
}
