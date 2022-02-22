import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataApiService } from '../core/services/data-api.service';
import { GameSizeLimit } from '../data/game-size-limit';

@Injectable({
  providedIn: 'root'
})
export class InitializeGameService {

  constructor(private dataApi: DataApiService) { }

  gameSizeUrl: string = 'game/GetLimits';

  public GetGameSize(): Observable<GameSizeLimit> {
    return this.dataApi.GetData(this.gameSizeUrl) as Observable<GameSizeLimit>;
  }
}
