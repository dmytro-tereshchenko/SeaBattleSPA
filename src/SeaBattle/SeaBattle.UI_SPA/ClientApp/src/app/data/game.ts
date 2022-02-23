import { Player } from './player';

export interface Game {
  id: number
  currentPlayerMove: string
  MaxNumberOfPlayers: number
  GameState: number
  Players : Player[]
}
