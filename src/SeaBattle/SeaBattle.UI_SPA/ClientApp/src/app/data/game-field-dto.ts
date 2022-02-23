import { GameFieldCell } from './game-field-cell';

export interface GameFieldDto {
  id: number
  sizeX: number
  sizeY: number
  gameId: number
  gameFieldCells : GameFieldCell[]
}
