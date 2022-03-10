import { Injectable } from '@angular/core';
import { DataApiService } from '../core/services/data-api.service';
import { ErrorLogService } from '../services/error-log.service';
import { Game } from '../data/game';
import { Player } from '../data/player';
import { Observable, of, map, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataGameService {

  constructor(private dataApi: DataApiService, private errorLog: ErrorLogService) {
    this.game = null;
  }

  game: Game | null;

  get: string = 'game/Get';
  create: string = 'game/Create';
  addPlayer: string = 'game/JoinPlayer';

  public getGame(): Observable<Game> {
    if (this.game !== null) {
      return of(this.game);
    }
    else {
      //update game
      return this.getGameFromServer();
    }
  }

  public getGameFromServer(): Observable<Game> {
    return this.dataApi.GetData<Game>(this.get).pipe(map(game => this.game = game),
      catchError(this.errorLog.handleError<Game>('getGameFromServer')));
  }

  public createGame(numberPlayers: Number): Observable<Game> {
    return this.dataApi.PostData(this.create, { players: numberPlayers }).pipe(map(game => this.game = game),
      catchError(this.errorLog.handleError<Game>('createGame')));
  }

  public joinPlayer(gameId: Number): Observable<Game> {
    return this.dataApi.PutData(this.addPlayer, { gameId: gameId }).pipe(map(game => this.game = game),
      catchError(this.errorLog.handleError<Game>('joinPlayer')));
  }

}
