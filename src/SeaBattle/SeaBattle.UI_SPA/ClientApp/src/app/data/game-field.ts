import { GameFieldCell } from './game-field-cell';

export interface GameField {
  id: number
  sizeX: number
  sizeY: number
  gameId: number
  gameFieldCells : GameFieldCell[][]
}
