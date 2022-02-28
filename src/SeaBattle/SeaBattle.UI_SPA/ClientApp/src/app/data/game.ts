import { Player } from './player';

export interface Game {
  id: number
  currentPlayerMove: string
  maxNumberOfPlayers: number
  gameState: number
  players : Player[]
}
