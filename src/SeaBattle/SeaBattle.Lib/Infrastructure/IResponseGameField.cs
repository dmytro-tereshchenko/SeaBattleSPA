using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Infrastructure
{
    public interface IResponseGameField : IResponse
    {
        public IGameField Value { get; }
    }
}
