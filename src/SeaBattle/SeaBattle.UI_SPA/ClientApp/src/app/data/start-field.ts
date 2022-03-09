import { StartFieldCell } from './start-field-cell';

export interface StartField {
  id: number
  points: number
  gameId: number
  startFieldCells : StartFieldCell[]
  gameShipsId : number[]
}