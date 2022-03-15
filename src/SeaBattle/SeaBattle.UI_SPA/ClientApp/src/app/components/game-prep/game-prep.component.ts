import { Component, OnInit, ViewChild } from '@angular/core';
import { DataGameFieldService } from '../../services/data-game-field.service';
import { DataStartFieldService } from '../../services/data-start-field.service';
import { DataShipService } from '../../services/data-ship.service';
import { Observable, forkJoin } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';
import { StartFieldCell } from '../../data/start-field-cell';
import { StartField } from 'src/app/data/start-field';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { InitializeGameService } from '../../services/initialize-game.service';
import { Router } from "@angular/router";
import { GameShip } from 'src/app/data/game-ship';

@Component({
  selector: 'app-game-prep',
  templateUrl: './game-prep.component.html',
  styleUrls: ['./game-prep.component.scss']
})
export class GamePrepComponent implements OnInit {

  gameField: GameField;
  labels: boolean[][];
  startField: StartField;
  selectedShipId: number;
  gameFieldHeight: string;

  displayedColumns: string[] = ['shipType', 'size', 'maxHp', 'speed', 'weapons', 'repairs', 'buttons'];
  dataSource: MatTableDataSource<GameShip>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private gameFieldService: DataGameFieldService,
    private startFieldService: DataStartFieldService,
    private shipService: DataShipService,
    private initService: InitializeGameService) {
    this.gameFieldHeight = "50vh";
  }

  ngOnInit(): void {
    this.gameFieldService.getGameField().subscribe(f => {
      this.gameField = f;

      this.startFieldService.getStartField().subscribe(sf => {
        this.updateListShips(sf)

        this.labels = [];

        for (var i: number = 0; i < f.sizeX; i++) {
          this.labels[i] = [];
          for (var j: number = 0; j < f.sizeY; j++) {
            this.labels[i][j] = false;
          }
        }

        sf.startFieldCells.forEach(c => this.labels[c.x - 1][c.y - 1] = true);
      });
    });
  }

  onNotifyGameFieldClick(cell: GameFieldCell) {
    console.log("click");
    //console.log(cell);
  }

  onNotifyGameFieldDblClick(cell: GameFieldCell) {
    console.log("dblclick");
    //console.log(cell);
  }

  onNotifyShopShip() {
    this.startFieldService.getStartFieldFromServer().subscribe(field => this.updateListShips(field));
  }

  select(shipId: number) {
    this.selectedShipId = shipId;
  }

  sellShip(row: GameShip) {
    this.shipService.sellShip(row.id).subscribe(state => {
      if (state === 10) {
        this.startFieldService.getStartFieldFromServer().subscribe(field => this.updateListShips(field));
      }
    });
  }

  addWeapon(row: GameShip) {
    if (row.shipType !== 2 && row.size > row.weapons.length + row.repairs.length) {
      this.initService.GetWeapons().pipe(mergeMap(w => this.shipService.addWeapon(row.id, w[0].id))).subscribe(s => {
        if (s === 10) {
          this.shipService.getShipFromServer(row.id)
            .subscribe(sh => this.startFieldService.getStartField()
              .subscribe(field => this.updateListShips(field)));
        }
      });
    }
  }

  removeWeapon(row: GameShip) {
    if (row.shipType !== 2 && row.weapons.length > 0) {
      this.initService.GetWeapons().pipe(mergeMap(w => this.shipService.removeWeapon(row.id, w[0].id))).subscribe(s => {
        if (s === 10) {
          this.shipService.getShipFromServer(row.id)
            .subscribe(sh => this.startFieldService.getStartField()
              .subscribe(field => this.updateListShips(field)));
        }
      });
    }
  }

  addRepair(row: GameShip) {
    if (row.shipType !== 1 && row.size > row.weapons.length + row.repairs.length) {
      this.initService.GetRepairs().pipe(mergeMap(r => this.shipService.addRepair(row.id, r[0].id))).subscribe(s => {
        if (s === 10) {
          this.shipService.getShipFromServer(row.id)
            .subscribe(sh => this.startFieldService.getStartField()
              .subscribe(field => this.updateListShips(field)));
        }
      });
    }
  }

  removeRepair(row: GameShip) {
    if (row.shipType !== 1 && row.repairs.length > 0) {
      this.initService.GetRepairs().pipe(mergeMap(r => this.shipService.removeRepair(row.id, r[0].id))).subscribe(s => {
        if (s === 10) {
          this.shipService.getShipFromServer(row.id)
            .subscribe(sh => this.startFieldService.getStartField()
              .subscribe(field => this.updateListShips(field)));
        }
      });
    }
  }

  private updateListShips(field: StartField) {
    const ships$: Observable<GameShip>[] = [];

    this.startField = field;

    field.gameShipsId.forEach(shipId => {
      const ship$: Observable<GameShip> = this.shipService.getShip(shipId);
      ships$.push(ship$);
    });

    forkJoin(ships$).subscribe((ships: GameShip[]) => {
      this.dataSource = new MatTableDataSource(ships);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }
}
