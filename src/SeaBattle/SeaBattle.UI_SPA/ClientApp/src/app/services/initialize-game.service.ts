import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { DataApiService } from '../core/services/data-api.service';
import { ErrorLogService } from '../services/error-log.service';
import { GameSizeLimit } from '../data/game-size-limit';
import { Game } from '../data/game';
import { GameSearch } from '../data/game-search';
import { Ship } from '../data/ship';
import { Weapon } from '../data/weapon';
import { Repair } from '../data/repair';

@Injectable({
  providedIn: 'root'
})
export class InitializeGameService {

  constructor(private dataApi: DataApiService, private errorLog: ErrorLogService) { }

  gameSizeUrl: string = 'game/GetLimits';
  gameSearch: string = 'game/GetSearch';
  shoppingShips: string = 'ship/GetShoppingList';
  weapons: string = 'ship/GetWeapons';
  repairs: string = 'ship/GetRepairs';

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

  public GetShips(): Observable<Ship[]> {
    return this.dataApi.GetData<Ship[]>(this.shoppingShips)
      .pipe(
        catchError(this.errorLog.handleError<Ship[]>('GetShips')));
  }

  public GetWeapons(): Observable<Weapon[]> {
    return this.dataApi.GetData<Weapon[]>(this.weapons)
      .pipe(
        catchError(this.errorLog.handleError<Weapon[]>('GetWeapons')));
  }

  public GetRepairs(): Observable<Repair[]> {
    return this.dataApi.GetData<Repair[]>(this.repairs)
      .pipe(
        catchError(this.errorLog.handleError<Repair[]>('GetRepairs')));
  }
}
