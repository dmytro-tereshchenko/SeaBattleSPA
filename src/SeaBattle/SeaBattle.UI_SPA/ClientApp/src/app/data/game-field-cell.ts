export interface GameFieldCell {
  id: number | null
  x: number
  y: number
  stern: boolean
  gameShipId: number | null
  playerId: number | null
}
