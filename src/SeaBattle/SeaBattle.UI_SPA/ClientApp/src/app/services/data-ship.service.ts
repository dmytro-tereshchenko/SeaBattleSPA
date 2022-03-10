import { Injectable } from '@angular/core';
import { GameShip } from '../data/game-ship';
import { Observable, of, map, catchError } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { DataApiService } from '../core/services/data-api.service';
import { ErrorLogService } from '../services/error-log.service';
import { DataStartFieldService } from '../services/data-start-field.service';

@Injectable({
  providedIn: 'root'
})
export class DataShipService {

  constructor(private dataApi: DataApiService,
    private errorLog: ErrorLogService,
    private startFieldService: DataStartFieldService) {
    this.ships = new Map<number, GameShip>();
  }

  //dictionary: key - shipId, value - ship
  ships: Map<number, GameShip>;

  getEndPoint: string = 'ship/GetById';
  buyEndPoint: string = 'ship/BuyShip';
  sellEndPoint: string = 'ship/SellShip';
  addWeaponEndPoint: string = 'ship/AddWeapon';
  addRepairEndPoint: string = 'ship/AddRepair';
  removeWeaponEndPoint: string = 'ship/RemoveWeapon';
  removeRepairEndPoint: string = 'ship/RemoveRepair';

  public getShip(id: number): Observable<GameShip> {
    if (this.ships.has(id)) {
      return of(this.ships.get(id)!);
    }
    else {
      //get game ship
      return this.getShipFromServer(id);
    }
  }

  public getShipFromServer(id: number): Observable<GameShip> {
    return this.dataApi.GetData<GameShip>(`${this.getEndPoint}?id=${id}`)
      .pipe(map(ship => {
        this.ships.set(ship.id, ship);
        return ship;
      }),
        catchError(this.errorLog.handleError<GameShip>('getShipFromServer')));
  }

  public buyShip(shipId: number): Observable<number> {
    return this.startFieldService.getStartField().pipe(mergeMap(field => this.dataApi.PostData(this.buyEndPoint, { ShipId: shipId, StartFieldId: field.id })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('buyShip')))));
  }

  public sellShip(shipId: number): Observable<number> {
    return this.startFieldService.getStartField().pipe(mergeMap(field => this.dataApi.DeleteData(`${this.sellEndPoint}?ShipId=${shipId}&StartFieldId=${field.id}`)
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('sellShip')))));
  }

  public addWeapon(shipId: number, equipmentId: number): Observable<number> {
    return this.dataApi.PutData(this.addWeaponEndPoint, { ShipId: shipId, EquipmentId: equipmentId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('addWeapon')));
  }

  public addRepair(shipId: number, equipmentId: number): Observable<number> {
    return this.dataApi.PutData(this.addRepairEndPoint, { ShipId: shipId, EquipmentId: equipmentId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('addRepair')));
  }

  public removeWeapon(shipId: number, equipmentId: number): Observable<number> {
    return this.dataApi.PutData(this.removeWeaponEndPoint, { ShipId: shipId, EquipmentId: equipmentId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('removeWeapon')));
  }

  public removeRepair(shipId: number, equipmentId: number): Observable<number> {
    return this.dataApi.PutData(this.removeRepairEndPoint, { ShipId: shipId, EquipmentId: equipmentId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('removeRepair')));
  }

}
