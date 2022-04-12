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

  private gameField: GameField | null;

  private get: string = 'GameField/GetById';
  private create: string = 'GameField/Create';

  getGameField(): Observable<GameField> {
    if (this.gameField !== null) {
      return of(this.gameField);
    }
    else {
      //update game field
      return this.getGameFieldFromServer();
    }
  }

  getGameFieldById(id: number): Observable<GameField> {
    return this.dataApi.GetData<GameFieldDto>(`${this.get}?id=${id}`)
      .pipe(catchError(this.errorLog.handleError<any>('getGameFieldById')));
  }

  getGameFieldFromServer(): Observable<GameField> {
    return this.gameService.getGame().pipe(mergeMap(game => this.dataApi.GetData<GameFieldDto>(`${this.get}?id=${game.id}`)
      .pipe(map(gameField => this.SetGameField(gameField)),
        catchError(this.errorLog.handleError<GameField>('getGameFieldFromServer')))));
  }

  createGameField(sizeX: number, sizeY: number): Observable<GameField> {
    return this.gameService.getGame().pipe(mergeMap(game => this.dataApi.PostData(this.create, { GameId: game.id, SizeX: sizeX, SizeY: sizeY })
      .pipe(map(gameField => this.SetGameField(gameField)),
        catchError(this.errorLog.handleError<GameField>('createGameField')))));
  }

  SetGameField(field: GameFieldDto): GameField {
    var cells: GameFieldCell[][];

    cells = [];

    for (var i: number = 0; i < field.sizeX; i++) {
      cells[i] = [];
      for (var j: number = 0; j < field.sizeY; j++) {
        cells[i][j] = <GameFieldCell>{
          id: null,
          x: i + 1,
          y: j + 1,
          stern: false,
          gameShipId: null,
          playerId: null
        }
      }
    }

    field.gameFieldCells.forEach(cell => {
      cells[cell.x - 1][cell.y - 1] = cell;
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

  clear() {
    this.gameField = null;
  }
}
