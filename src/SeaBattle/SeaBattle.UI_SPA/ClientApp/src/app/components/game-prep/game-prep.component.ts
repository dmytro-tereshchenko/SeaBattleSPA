import { Component, OnInit, ViewChild } from '@angular/core';
import { DataGameFieldService } from '../../services/data-game-field.service';
import { DataStartFieldService } from '../../services/data-start-field.service';
import { DataShipService } from '../../services/data-ship.service';
import { DataGameService } from '../../services/data-game.service';
import { Observable, forkJoin } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';
import { StartFieldCell } from '../../data/start-field-cell';
import { StartField } from '../../data/start-field';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { InitializeGameService } from '../../services/initialize-game.service';
import { ShipActionService } from '../../services/ship-action.service';
import { Router } from "@angular/router";
import { GameShip } from '../../data/game-ship';
import { Direction } from '../../data/direction';

@Component({
  selector: 'app-game-prep',
  templateUrl: './game-prep.component.html',
  styleUrls: ['./game-prep.component.scss']
})
export class GamePrepComponent implements OnInit {

  gameField: GameField;
  labels: boolean[][];
  startField: StartField;
  selectedShip: GameShip | null;
  gameFieldHeight: string;

  clickCell: GameFieldCell | null;

  private shipCoords: GameFieldCell[] = [];
  private tempCoords: GameFieldCell[] = [];

  displayedColumns: string[] = ['shipType', 'size', 'maxHp', 'speed', 'weapons', 'repairs', 'buttons'];
  dataSource: MatTableDataSource<GameShip>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private gameFieldService: DataGameFieldService,
    private startFieldService: DataStartFieldService,
    private shipService: DataShipService,
    private initService: InitializeGameService,
    private gameService: DataGameService,
    private actionService: ShipActionService,
    private router: Router) {
    this.gameFieldHeight = "50vh";
    this.clickCell = null;
    this.selectedShip = null;
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
    let changeShip: boolean = false;
    if (cell) {
      if (cell.gameShipId) {
        changeShip = true;
        this.shipService.getShip(cell.gameShipId).subscribe(ship => {
          this.selectedShip = ship;
          this.clickCell = null;
          this.getShipCoords();
        })
      }
      if (this.selectedShip &&
        !changeShip &&
        !this.clickCell &&
        this.selectedShip.size === 1 &&
        cell.gameShipId !== this.selectedShip?.id) {
        if (this.shipCoords.length > 0) {
          this.startFieldService.removeShipFromField(this.selectedShip.id).subscribe(state => {
            if (state === 10) {
              this.putShip(cell, Direction.xDec);
            }
          })
        }
        else {
          this.putShip(cell, Direction.xDec);
        }
      }
    }
  }

  onNotifyGameFieldDblClick(cell: GameFieldCell) {
    if (cell.gameShipId) {
      this.startFieldService.removeShipFromField(cell.gameShipId).subscribe(state => {
        if (state === 10) {
          this.startFieldService.getStartFieldFromServer()
            .subscribe(field => this.updateListShips(field));

          this.gameFieldService.getGameFieldFromServer().subscribe(f => this.gameField = f);
        }

        this.clickCell = null;
      })
    }
  }

  onNotifyGameFieldMouseDown(cell: GameFieldCell) {
    if (this.selectedShip &&
      this.selectedShip?.size !== 1) {
      this.clickCell = cell;
    }
  }

  onNotifyGameFieldMouseOver(cell: GameFieldCell) {
    if (this.selectedShip &&
      this.clickCell &&
      this.selectedShip?.size !== 1) {
      this.shipCoords.forEach(c => {
        this.gameField.gameFieldCells[c.x - 1][c.y - 1].gameShipId = null;
        this.gameField.gameFieldCells[c.x - 1][c.y - 1].playerId = null;
      });
      this.tempCoords.forEach(c => {
        this.gameField.gameFieldCells[c.x - 1][c.y - 1].gameShipId = null;
        this.gameField.gameFieldCells[c.x - 1][c.y - 1].playerId = null;
      });

      this.tempCoords = [];

      let i: number = 0;
      let temp: number;

      switch (this.actionService.getDirection(this.clickCell, cell)) {
        case Direction.xDec: {
          while (i < this.selectedShip.size) {
            temp = this.clickCell.x - i - 1;
            if (temp < 0) {
              this.tempCoords = [];
              this.shipCoords.forEach(c => {
                this.gameField.gameFieldCells[c.x - 1][c.y - 1].gameShipId = this.selectedShip?.id!;
                this.gameField.gameFieldCells[c.x - 1][c.y - 1].playerId = this.selectedShip?.gamePlayerId!;
              });
              return;
            }
            else {
              this.tempCoords.push(this.gameField.gameFieldCells[temp][this.clickCell.y - 1])
            }
            i++;
          }
          break;
        }
        case Direction.xInc: {
          while (i < this.selectedShip.size) {
            temp = this.clickCell.x + i - 1;
            if (temp >= this.gameField.sizeX) {
              this.tempCoords = [];
              this.shipCoords.forEach(c => {
                this.gameField.gameFieldCells[c.x - 1][c.y - 1].gameShipId = this.selectedShip?.id!;
                this.gameField.gameFieldCells[c.x - 1][c.y - 1].playerId = this.selectedShip?.gamePlayerId!;
              });
              return;
            }
            else {
              this.tempCoords.push(this.gameField.gameFieldCells[temp][this.clickCell.y - 1]);
            }
            i++;
          }
          break;
        }
        case Direction.yDec: {
          while (i < this.selectedShip.size) {
            temp = this.clickCell.y - i - 1;
            if (temp < 0) {
              this.tempCoords = [];
              this.shipCoords.forEach(c => {
                this.gameField.gameFieldCells[c.x - 1][c.y - 1].gameShipId = this.selectedShip?.id!;
                this.gameField.gameFieldCells[c.x - 1][c.y - 1].playerId = this.selectedShip?.gamePlayerId!;
              });
              return;
            }
            else {
              this.tempCoords.push(this.gameField.gameFieldCells[this.clickCell.x - 1][temp]);
            }
            i++;
          }
          break;
        }
        case Direction.yInc: {
          while (i < this.selectedShip.size) {
            temp = this.clickCell.y + i - 1;
            if (temp >= this.gameField.sizeX) {
              this.tempCoords = [];
              this.shipCoords.forEach(c => {
                this.gameField.gameFieldCells[c.x - 1][c.y - 1].gameShipId = this.selectedShip?.id!;
                this.gameField.gameFieldCells[c.x - 1][c.y - 1].playerId = this.selectedShip?.gamePlayerId!;
              });
              return;
            }
            else {
              this.tempCoords.push(this.gameField.gameFieldCells[this.clickCell.x - 1][temp]);
            }
            i++;
          }
          break;
        }
      }

      if (this.tempCoords.length > 0) {
        this.tempCoords.forEach(c => {
          this.gameField.gameFieldCells[c.x - 1][c.y - 1].gameShipId = this.selectedShip?.id!;
          this.gameField.gameFieldCells[c.x - 1][c.y - 1].playerId = this.selectedShip?.gamePlayerId!;
        });
      }
    }
  }

  onNotifyGameFieldMouseUp(cell: GameFieldCell) {
    if (this.selectedShip &&
      this.selectedShip?.size !== 1 &&
      this.clickCell &&
      this.tempCoords.length > 0) {
      if (this.shipCoords.length > 0) {
        this.startFieldService.removeShipFromField(this.selectedShip.id).subscribe(state => {
          if (state === 10) {
            this.putShip(this.clickCell!, this.actionService.getDirection(this.clickCell!, cell));
          }
        })
      }
      else {
        this.putShip(this.clickCell, this.actionService.getDirection(this.clickCell, cell));
      }
    }
  }

  private getShipCoords() {
    this.shipCoords = [];

    for (var i: number = 0; i < this.gameField.sizeX; i++) {
      for (var j: number = 0; j < this.gameField.sizeY; j++) {
        if (this.gameField.gameFieldCells[i][j]?.gameShipId === this.selectedShip?.id) {
          this.shipCoords.push(this.gameField.gameFieldCells[i][j]);

          let ti: number = i + 1;
          while (this.gameField.gameFieldCells[ti][j]?.gameShipId === this.selectedShip?.id) {
            this.shipCoords.push(this.gameField.gameFieldCells[ti][j]);
            ti++;
          }

          let tj: number = j + 1;
          while (this.gameField.gameFieldCells[i][tj]?.gameShipId === this.selectedShip?.id) {
            this.shipCoords.push(this.gameField.gameFieldCells[i][tj]);
            tj++;
          }

          return;
        }
      }
    }
  }

  private putShip(cell: GameFieldCell, direction: Direction) {
    if (this.selectedShip) {
      this.startFieldService.putShipOnField(cell, direction, this.selectedShip?.id).subscribe(state => {
        if (state === 10) {
          this.selectedShip = null;

          this.startFieldService.getStartFieldFromServer()
            .subscribe(field => this.updateListShips(field));

          this.gameFieldService.getGameFieldFromServer().subscribe(f => this.gameField = f);
        }
        //in case wrong position for put ship, replace ship on the old position
        else if (this.shipCoords.length > 0) {
          const oldDirection: Direction = this.selectedShip?.size === 1 ? Direction.xDec : this.actionService.getDirection(this.shipCoords[0], this.shipCoords[1]);
          this.startFieldService.putShipOnField(this.shipCoords[0], oldDirection, this.selectedShip?.id!).subscribe(state => {
            if (state === 10) {
              this.selectedShip = null;

              this.gameFieldService.getGameFieldFromServer().subscribe(f => this.gameField = f);
            }
          });
        }

        this.tempCoords = [];
        this.shipCoords = [];
        this.clickCell = null;
      })
    }
  }

  onNotifyShopShip() {
    this.startFieldService.getStartFieldFromServer().subscribe(field => this.updateListShips(field));
  }

  select(row: GameShip) {
    this.selectedShip = row;
    this.clickCell = null;
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

    //in case an empty list of ships
    if (field.gameShipsId.length === 0) {
      const ships: GameShip[] = [];
      this.dataSource = new MatTableDataSource(ships);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      return;
    }

    //otherwise get ships to list
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

  ready() {
    this.gameService.readyPlayer().subscribe(state => {
      if (state === 10) {
        this.router.navigate(['/wait-begin-game']);
      }
    })
  }
}
