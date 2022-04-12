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
  private ships: Map<number, GameShip>;

  private getEndPoint: string = 'ship/GetById';
  private buyEndPoint: string = 'ship/BuyShip';
  private sellEndPoint: string = 'ship/SellShip';
  private addWeaponEndPoint: string = 'ship/AddWeapon';
  private addRepairEndPoint: string = 'ship/AddRepair';
  private removeWeaponEndPoint: string = 'ship/RemoveWeapon';
  private removeRepairEndPoint: string = 'ship/RemoveRepair';

  getShip(id: number): Observable<GameShip> {
    if (this.ships.has(id)) {
      return of(this.ships.get(id)!);
    }
    else {
      //get game ship
      return this.getShipFromServer(id);
    }
  }

  getShipFromServer(id: number): Observable<GameShip> {
    return this.dataApi.GetData<GameShip>(`${this.getEndPoint}?id=${id}`)
      .pipe(map(ship => {
        this.ships.set(ship.id, ship);
        return ship;
      }),
        catchError(this.errorLog.handleError<GameShip>('getShipFromServer')));
  }

  buyShip(shipId: number): Observable<number> {
    return this.startFieldService.getStartField().pipe(mergeMap(field => this.dataApi.PostData(this.buyEndPoint, { ShipId: shipId, StartFieldId: field.id })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('buyShip')))));
  }

  sellShip(shipId: number): Observable<number> {
    return this.startFieldService.getStartField().pipe(mergeMap(field => this.dataApi.DeleteData(this.sellEndPoint, { ShipId: shipId, StartFieldId: field.id })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('sellShip')))));
  }

  addWeapon(shipId: number, equipmentId: number): Observable<number> {
    return this.dataApi.PutData(this.addWeaponEndPoint, { ShipId: shipId, EquipmentId: equipmentId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('addWeapon')));
  }

  addRepair(shipId: number, equipmentId: number): Observable<number> {
    return this.dataApi.PutData(this.addRepairEndPoint, { ShipId: shipId, EquipmentId: equipmentId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('addRepair')));
  }

  removeWeapon(shipId: number, equipmentId: number): Observable<number> {
    return this.dataApi.PutData(this.removeWeaponEndPoint, { ShipId: shipId, EquipmentId: equipmentId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('removeWeapon')));
  }

  removeRepair(shipId: number, equipmentId: number): Observable<number> {
    return this.dataApi.PutData(this.removeRepairEndPoint, { ShipId: shipId, EquipmentId: equipmentId })
      .pipe(map(state => state),
        catchError(this.errorLog.handleError<number>('removeRepair')));
  }

  deleteShip(id: number) {
    this.ships.delete(id);
  }

  clear(){
    this.ships.clear();
  }

}
