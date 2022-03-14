import { Component, OnInit, ViewChild } from '@angular/core';
import { DataGameFieldService } from '../../services/data-game-field.service';
import { DataStartFieldService } from '../../services/data-start-field.service';
import { DataShipService } from '../../services/data-ship.service';
import { Observable, forkJoin } from 'rxjs';
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

  displayedColumns: string[] = ['shipType', 'size', 'maxHp', 'speed', 'weapons', 'repairs'];
  dataSource: MatTableDataSource<GameShip>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private gameFieldService: DataGameFieldService,
    private startFieldService: DataStartFieldService,
    private shipService: DataShipService) {
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

  onNotifyGameField(cell: GameFieldCell) {
    console.log(cell);
  }

  onNotifyShopShip() {
    this.startFieldService.getStartFieldFromServer().subscribe(field => this.updateListShips(field));
  }

  select(shipId: number) {
    this.selectedShipId = shipId;
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
