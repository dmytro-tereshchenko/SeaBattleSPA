import { Injectable } from '@angular/core';
import { DataApiService } from '../core/services/data-api.service';
import { ErrorLogService } from '../services/error-log.service';
import { StartField } from '../data/start-field';
import { Observable, of, map, catchError } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { DataGameService } from './data-game.service';
import { StartFieldCell } from '../data/start-field-cell';
import { GameFieldCell } from '../data/game-field-cell';
import { Direction } from '../data/direction';

@Injectable({
  providedIn: 'root'
})
export class DataStartFieldService {

  constructor(private dataApi: DataApiService, private errorLog: ErrorLogService, private gameService: DataGameService) {
    this.startField = null;
  }

  private startField: StartField | null;

  private get: string = 'StartField/Get';
  private putShip: string = 'StartField/TransferShipToGameField';
  private removeShip: string = 'StartField/TransferShipFromGameField';

  getStartField(): Observable<StartField> {
    if (this.startField !== null) {
      return of(this.startField);
    }
    else {
      //update start field
      return this.getStartFieldFromServer();
    }
  }

  getStartFieldById(id: number): Observable<StartField> {
    return this.dataApi.GetData<StartField>(`${this.get}?id=${id}`)
      .pipe(catchError(this.errorLog.handleError<StartField>('getStartFieldById')));
  }

  getStartFieldFromServer(): Observable<StartField> {
    return this.gameService.getGame().pipe(mergeMap(game => this.dataApi.GetData<StartField>(`${this.get}?id=${game.id}`)
      .pipe(map(startField => this.startField = startField),
        catchError(this.errorLog.handleError<StartField>('getStartFieldFromServer')))));
  }

  putShipOnField(cell: GameFieldCell, direction: Direction, shipId: number): Observable<number> {
    return this.getStartField().pipe(mergeMap(field => this.dataApi.PutData<number>(this.putShip,
      { tPosX: cell.x, tPosY: cell.y, direction: direction, startFieldId: field.id, shipId: shipId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('putShipOnField')))));
  }

  removeShipFromField(shipId: number): Observable<number> {
    return this.getStartField().pipe(mergeMap(field => this.dataApi.PutData<number>(this.removeShip,
      { shipId: shipId, startFieldId: field.id })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('removeShipOnField')))));
  }
}
