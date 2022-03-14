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

  private gameSizeUrl: string = 'game/GetLimits';
  private gameSearch: string = 'game/GetSearch';
  private shoppingShips: string = 'ship/GetShoppingList';
  private weapons: string = 'ship/GetWeapons';
  private repairs: string = 'ship/GetRepairs';

  GetGameSize(): Observable<GameSizeLimit> {
    return this.dataApi.GetData<GameSizeLimit>(this.gameSizeUrl)
      .pipe(
        catchError(this.errorLog.handleError<GameSizeLimit>('GetGameSize')));
  }

  GetSearch(): Observable<GameSearch[]> {
    return this.dataApi.GetData<GameSearch[]>(this.gameSearch)
      .pipe(
        catchError(this.errorLog.handleError<GameSearch[]>('GetSearch')));
  }

  GetShips(): Observable<Ship[]> {
    return this.dataApi.GetData<Ship[]>(this.shoppingShips)
      .pipe(
        catchError(this.errorLog.handleError<Ship[]>('GetShips')));
  }

  GetWeapons(): Observable<Weapon[]> {
    return this.dataApi.GetData<Weapon[]>(this.weapons)
      .pipe(
        catchError(this.errorLog.handleError<Weapon[]>('GetWeapons')));
  }

  GetRepairs(): Observable<Repair[]> {
    return this.dataApi.GetData<Repair[]>(this.repairs)
      .pipe(
        catchError(this.errorLog.handleError<Repair[]>('GetRepairs')));
  }
}
