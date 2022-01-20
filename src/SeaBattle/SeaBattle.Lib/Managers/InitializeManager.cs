using SeaBattle.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    public class InitializeManager
    {
        private IUnitOfWork _repository;

        private ushort _minSizeX;

        private ushort _maxSizeX;

        private ushort _minSizeY;

        private ushort _maxSizeY;

        private byte _maxNumberOfTeams;

        public InitializeManager(IUnitOfWork repository, ushort minSizeX, ushort maxSizeX, ushort minSizeY, ushort maxSizeY, byte maxNumberOfTeams)
        {
            _repository = repository;
            _minSizeX = minSizeX;
            _maxSizeX = maxSizeX;
            _minSizeY = minSizeY;
            _maxSizeY = maxSizeY;
            _maxNumberOfTeams = maxNumberOfTeams;
        }

        public IResponseGameField CreateGameField(ushort sizeX, ushort sizeY)
        {
            if (sizeX < _minSizeX || sizeX > _maxSizeX || sizeY < _minSizeY || sizeY > _maxSizeY)
                return new ResponseGameField(null, StateCode.InvalidFieldSize);
            GameField field = new GameField(sizeX, sizeY);
            _repository.GameFields.Create(field);
            return new ResponseGameField(field, StateCode.Success);
        }
    }
}
