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

  private game: Game | null;

  private get: string = 'game/Get';
  private create: string = 'game/Create';
  private addPlayer: string = 'game/JoinPlayer';

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
    return this.dataApi.GetData<Game>(this.get).pipe(map(game => this.game = game),
      catchError(this.errorLog.handleError<Game>('getGameFromServer')));
  }

  createGame(numberPlayers: Number): Observable<Game> {
    return this.dataApi.PostData(this.create, { players: numberPlayers }).pipe(map(game => this.game = game),
      catchError(this.errorLog.handleError<Game>('createGame')));
  }

  joinPlayer(gameId: Number): Observable<Game> {
    return this.dataApi.PutData(this.addPlayer, { gameId: gameId }).pipe(map(game => this.game = game),
      catchError(this.errorLog.handleError<Game>('joinPlayer')));
  }

}
