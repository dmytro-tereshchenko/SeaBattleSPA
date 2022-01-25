namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Basic interface for a response of the state
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// State of the response
        /// </summary>
        /// <value><see cref="StateCode"/></value>
        StateCode State { get; }
    }
}
