export type Root = Weather[]

export interface Weather {
  date: string
  temperatureC: number
  temperatureF: number
  summary: string
}
