import { Injectable } from '@angular/core';
import { DataApiService } from '../core/services/data-api.service';
import { ErrorLogService } from '../services/error-log.service';
import { Game } from '../data/game';
import { Player } from '../data/player';
import { Observable, of, map, catchError, mergeMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataGameService {

  constructor(private dataApi: DataApiService, private errorLog: ErrorLogService) {
    this.game = null;
  }

  private game: Game | null;

  private getEndPoint: string = 'game/Get';
  private createEndPoint: string = 'game/Create';
  private addPlayerEndPoint: string = 'game/JoinPlayer';
  private readyPlayerEndPoint: string = 'game/ReadyPlayer';

  getGame(): Observable<Game> {
    if (this.game !== null) {
      return of(this.game);
    }
    else {
      //update game
      return this.getGameFromServer();
    }
  }

  getGameFromServer(): Observable<Game> {
    return this.dataApi.GetData<Game>(this.getEndPoint).pipe(map(game => this.game = game),
      catchError(this.errorLog.handleError<Game>('getGameFromServer')));
  }

  createGame(numberPlayers: Number): Observable<Game> {
    return this.dataApi.PostData(this.createEndPoint, { players: numberPlayers }).pipe(map(game => this.game = game),
      catchError(this.errorLog.handleError<Game>('createGame')));
  }

  joinPlayer(gameId: Number): Observable<Game> {
    return this.dataApi.PutData(this.addPlayerEndPoint, { gameId: gameId }).pipe(map(game => this.game = game),
      catchError(this.errorLog.handleError<Game>('joinPlayer')));
  }

  readyPlayer(): Observable<number> {
    return this.getGame().pipe(mergeMap(game => this.dataApi.PutData(this.readyPlayerEndPoint, { gameId: game.id }).pipe(map(state => state),
      catchError(this.errorLog.handleError<number>('readyPlayer')))));
  }
}
