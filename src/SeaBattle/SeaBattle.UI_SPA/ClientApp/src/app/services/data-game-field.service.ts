import { Injectable } from '@angular/core';
import { DataApiService } from '../core/services/data-api.service';
import { ErrorLogService } from '../services/error-log.service';
import { GameFieldDto } from '../data/game-field-dto';
import { GameField } from '../data/game-field';
import { GameFieldCell } from '../data/game-field-cell';
import { Observable, of, map, catchError } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { DataGameService } from './data-game.service';


@Injectable({
  providedIn: 'root'
})
export class DataGameFieldService {

  constructor(private dataApi: DataApiService,
    private errorLog: ErrorLogService,
    private gameService: DataGameService) {
    this.gameField = null;
  }

  gameField: GameField | null;

  get: string = 'GameField/GetById';
  create: string = 'GameField/Create';

  public getGameField(): Observable<GameField> {
    if (this.gameField !== null) {
      return of(this.gameField);
    }
    else {
      //update game field
      return this.getGameFieldFromServer();
    }
  }

  public getGameFieldById(id: number): Observable<GameField> {
    return this.dataApi.GetData<GameFieldDto>(`${this.get}?id=${id}`)
      .pipe(catchError(this.errorLog.handleError<any>('getGameFieldById')));
  }

  public getGameFieldFromServer(): Observable<GameField> {
    return this.gameService.getGame().pipe(mergeMap(game => this.dataApi.GetData<GameFieldDto>(`${this.get}?id=${game.id}`)
      .pipe(map(gameField => this.SetGameField(gameField)),
        catchError(this.errorLog.handleError<GameField>('getGameFieldFromServer')))));
  }

  public createGameField(sizeX: number, sizeY: number): Observable<GameField> {
    return this.gameService.getGame().pipe(mergeMap(game => this.dataApi.PostData(this.create, { GameId: game.id, SizeX: sizeX, SizeY: sizeY })
      .pipe(map(gameField => this.SetGameField(gameField)),
        catchError(this.errorLog.handleError<GameField>('createGameField')))));
  }

  private SetGameField(field: GameFieldDto): GameField {
    var cells: GameFieldCell[][];

    cells = [];

    for (var i: number = 0; i < field.sizeX; i++) {
      cells[i] = [];
      for (var j: number = 0; j < field.sizeY; j++) {
        cells[i][j] = <GameFieldCell>{
          id: null,
          x: i,
          y: j,
          stern: false,
          gameShipId: null,
          playerId: null
        }
      }
    }

    field.gameFieldCells.forEach(cell => {
      cells[cell.x][cell.y] = cell;
    });

    this.gameField = <GameField>{
      id: field.id,
      sizeX: field.sizeX,
      sizeY: field.sizeY,
      gameId: field.gameId,
      gameFieldCells: cells
    }

    return this.gameField;
  }
}
