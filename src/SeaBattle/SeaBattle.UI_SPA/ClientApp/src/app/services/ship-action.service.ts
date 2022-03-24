import { Injectable } from '@angular/core';
import { Observable, of, map, catchError } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { GameFieldCell } from '../data/game-field-cell';
import { Direction } from '../data/direction';
import { ActionType } from '../data/action-type';
import { DataApiService } from '../core/services/data-api.service';
import { ErrorLogService } from '../services/error-log.service';
import { DataGameFieldService } from './data-game-field.service';

@Injectable({
  providedIn: 'root'
})
export class ShipActionService {

  constructor(private dataApi: DataApiService,
    private errorLog: ErrorLogService,
    private fieldService: DataGameFieldService) { }

  private moveEndPoint: string = 'GameField/MoveShip';
  private attackEndPoint: string = 'GameField/AttackShip';
  private repairEndPoint: string = 'GameField/RepairShip';
  private repairAllEndPoint: string = 'GameField/RepairAllShip';
  private endMoveEndPoint: string = 'Game/EndMove';
  private vissibleShipsEndPoint: string = 'Ship/GetVisibleShips';

  move(shipId: number, cell: GameFieldCell, direction: Direction): Observable<number> {
    return this.fieldService.getGameField().pipe(mergeMap(field => this.dataApi.PutData<number>(this.moveEndPoint, { GameShipId: shipId, TPosX: cell.x, TPosY: cell.y, Direction: direction, GameFieldId: field.id })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('moveShip')))));
  }

  attack(shipId: number, cell: GameFieldCell): Observable<number> {
    return this.fieldService.getGameField().pipe(mergeMap(field => this.dataApi.PutData<number>(this.attackEndPoint, { GameShipId: shipId, TPosX: cell.x, TPosY: cell.y, GameFieldId: field.id })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('attackShip')))));
  }

  repair(shipId: number, cell: GameFieldCell): Observable<number> {
    return this.fieldService.getGameField().pipe(mergeMap(field => this.dataApi.PutData<number>(this.repairEndPoint, { GameShipId: shipId, TPosX: cell.x, TPosY: cell.y, GameFieldId: field.id })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('repairShip')))));
  }

  repairAll(shipId: number): Observable<number> {
    return this.fieldService.getGameField().pipe(mergeMap(field => this.dataApi.PutData<number>(this.repairAllEndPoint, { GameShipId: shipId, GameFieldId: field.id })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('repairAllShip')))));
  }

  endMove(): Observable<number> {
    return this.fieldService.getGameField().pipe(mergeMap(field => this.dataApi.PutData<number>(this.endMoveEndPoint, { id: field.gameId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('endMove')))));
  }

  getVissibleShips(shipId: number, action: ActionType): Observable<number[]> {
    return this.fieldService.getGameField().pipe(mergeMap(field => this.dataApi.GetData<number[]>(`${this.endMoveEndPoint}?GameShipId=${shipId}&GameFieldId=${field.id}&Action=${action}`)
      .pipe(map(ships => ships),
        catchError(this.errorLog.handleError<number[]>('getVissibleShips')))));
  }
}
