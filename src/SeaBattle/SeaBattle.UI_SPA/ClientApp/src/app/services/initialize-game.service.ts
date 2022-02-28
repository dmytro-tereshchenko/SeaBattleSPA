import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { DataApiService } from '../core/services/data-api.service';
import { ErrorLogService } from '../services/error-log.service';
import { GameSizeLimit } from '../data/game-size-limit';
import { Game } from '../data/game';
import { GameSearch } from '../data/game-search';

@Injectable({
  providedIn: 'root'
})
export class InitializeGameService {

  constructor(private dataApi: DataApiService, private errorLog: ErrorLogService) { }

  gameSizeUrl: string = 'game/GetLimits';
  gameSearch: string = 'game/GetSearch';

  public GetGameSize(): Observable<GameSizeLimit> {
    return this.dataApi.GetData<GameSizeLimit>(this.gameSizeUrl)
    .pipe(
      catchError(this.errorLog.handleError<GameSizeLimit>('GetGameSize')));
  }

  public GetSearch(): Observable<GameSearch[]> {
    return this.dataApi.GetData<GameSearch[]>(this.gameSearch)
    .pipe(
      catchError(this.errorLog.handleError<GameSearch[]>('GetSearch')));
  }
}
