import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { DataApiService } from '../core/services/data-api.service';
import { ErrorLogService } from '../services/error-log.service';
import { GameSizeLimit } from '../data/game-size-limit';

@Injectable({
  providedIn: 'root'
})
export class InitializeGameService {

  constructor(private dataApi: DataApiService, private errorLog: ErrorLogService) { }

  gameSizeUrl: string = 'game/GetLimits';

  public GetGameSize(): Observable<GameSizeLimit> {
    return this.dataApi.GetData<GameSizeLimit>(this.gameSizeUrl)
    .pipe(
      catchError(this.errorLog.handleError<GameSizeLimit>('GetGameSize')));
  }
}
