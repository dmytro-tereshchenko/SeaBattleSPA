import { Component, OnInit } from '@angular/core';
import { Observable, interval, Subscription } from 'rxjs';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';
import { Game } from '../../data/game';
import { Player } from '../../data/player';
import { GameShip } from '../../data/game-ship';
import { Direction } from '../../data/direction';
import { colorPlayers } from '../../data/mock-color-players';
import { DataGameService } from '../../services/data-game.service';
import { DataGameFieldService } from '../../services/data-game-field.service';
import { DataShipService } from '../../services/data-ship.service';
import { ShipActionService } from '../../services/ship-action.service';
import { AuthService } from '../../core/services/auth.service';
import { ActionType } from 'src/app/data/action-type';

@Component({
  selector: 'app-game-process',
  templateUrl: './game-process.component.html',
  styleUrls: ['./game-process.component.scss']
})
export class GameProcessComponent implements OnInit {

  game: Game;
  gameField: GameField;
  labels: boolean[][];
  selectedShip: GameShip | null;
  gameFieldHeight: string;
  clickCell: GameFieldCell | null;
  private colorShips: string[] = colorPlayers;
  private players: string[];
  private player: Player | undefined;
  private isMoved: boolean;
  private isActioned: boolean;
  private updateTimeOut: number = 1000;
  private subscription: Subscription;
  private source: Observable<number>

  private shipCoords: GameFieldCell[] = [];
  private tempCoords: GameFieldCell[] = [];

  constructor(private gameFieldService: DataGameFieldService,
    private gameService: DataGameService,
    private shipService: DataShipService,
    private userService: AuthService,
    private actionService: ShipActionService) {
    this.gameFieldHeight = "80vh";
    this.clickCell = null;
    this.selectedShip = null;
    this.isMoved = false;
    this.isActioned = false;
  }

  ngOnInit(): void {
    this.source = interval(this.updateTimeOut);

    this.gameFieldService.getGameField().subscribe(f => {
      this.gameField = f;

      this.labels = [];

      for (var i: number = 0; i < f.sizeX; i++) {
        this.labels[i] = [];
        for (var j: number = 0; j < f.sizeY; j++) {
          this.labels[i][j] = false;
        }
      }
    });

    this.userService.getUser().then(user => this.gameService.getGame().subscribe(game => {
      this.game = game;
      this.players = [];
      this.player = game.players.find(p => p.name === user?.profile.name);

      if (this.player) {
        this.players.push(this.player.name);
      }

      game.players.forEach(p => {
        if (p.name !== this.players[0]) {
          this.players.push(p.name);
        }
      })

      //in case of current move isn't the player's move then auto-update and wait
      if (game.currentPlayerMove !== this.player?.name) {
        this.subscription = this.source.subscribe(val => this.update());
      }
    }))
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  getColor(playerName: string): string {
    if (playerName) {
      let index = this.players.findIndex(i => i === playerName);

      return index === -1 ? "black" : this.colorShips[index];
    }
    else {
      return "black";
    }
  }

  onNotifyGameFieldClick(cell: GameFieldCell) {
    let changeShip: boolean = false;
    if (cell &&
      !this.isMoved &&
      this.game.currentPlayerMove === this.player?.name) {
      if (cell.gameShipId) {
        this.shipService.getShip(cell.gameShipId).subscribe(ship => {
          if (ship.gamePlayerId === this.player?.id &&
            !this.isMoved &&
            !this.isActioned) {
            this.selectedShip = ship;
            changeShip = true;
            this.clickCell = null;
            this.getShipCoords();
            this.setMoveArea();
          }
        })
      }
      if (this.selectedShip &&
        !changeShip &&
        !this.clickCell &&
        this.selectedShip.size === 1 &&
        cell.gameShipId !== this.selectedShip?.id) {
        this.moveShip(cell, Direction.xDec);
      }
    }
  }

  onNotifyGameFieldDblClick(cell: GameFieldCell) {
    if (this.game.currentPlayerMove === this.player?.name &&
      cell &&
      cell.gameShipId &&
      !this.isActioned) {
      this.shipService.getShip(cell.gameShipId).subscribe(ship => {
        if (this.player?.id === ship.gamePlayerId) {
          //repair all visible team ships
          if ((this.selectedShip == null && cell.gameShipId) || (
            this.selectedShip && cell.gameShipId === this.selectedShip.id)) {
            this.actionService.repairAll(cell.gameShipId).subscribe(state => {
              if (state === 10 || state === 21) {
                this.isActioned = true;

                this.shipService.getShip(cell.gameShipId!).subscribe(ship => this.selectedShip = ship);

                this.actionService.getVissibleShips(cell.gameShipId!, ActionType.repair).subscribe(ships => {
                  if (ships) {
                    ships.forEach(shipId => this.shipService.getShipFromServer(shipId).subscribe())
                  }
                })

                this.updateAfterAction();
              }
            })
          }
          //repair target ship
          else if (this.selectedShip &&
            cell.gameShipId !== this.selectedShip.id) {
            this.actionService.repair(this.selectedShip.id, cell).subscribe(state => {
              if (state === 10) {
                this.isActioned = true;

                this.shipService.getShipFromServer(cell.gameShipId!).subscribe();

                this.updateAfterAction();
              }
            })
          }
        }
        //attack target ship
        else if (this.selectedShip) {
          this.actionService.attack(this.selectedShip.id, cell).subscribe(state => {
            if (state === 10 || state === 23) {
              this.isActioned = true;

              this.shipService.getShipFromServer(cell.gameShipId!).subscribe();

              this.updateAfterAction();
            }
          })
        }
      })
    }
  }

  onNotifyGameFieldMouseDown(cell: GameFieldCell) {
    if (this.selectedShip &&
      this.selectedShip?.size !== 1 &&
      !this.isMoved) {
      this.clickCell = cell;
    }
  }

  onNotifyGameFieldMouseOver(cell: GameFieldCell) {
    if (this.selectedShip &&
      this.clickCell &&
      this.selectedShip?.size !== 1 &&
      !this.isMoved) {
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
      this.tempCoords.length > 0 &&
      !this.isMoved) {
      this.moveShip(this.clickCell, this.actionService.getDirection(this.clickCell, cell));
    }
  }

  //in case actioned update game field 
  private updateAfterAction() {
    this.gameFieldService.getGameFieldFromServer().subscribe(field => this.gameField = field);

    if (this.isMoved) {
      this.endMove();
    }
    else {
      this.setMoveArea();
    }
  }

  private clearLabelArea() {
    for (var i: number = 0; i < this.gameField.sizeX; i++) {
      for (var j: number = 0; j < this.gameField.sizeY; j++) {
        this.labels[i][j] = false;
      }
    }
  }

  private setMoveArea() {
    const shipcells: GameFieldCell[] = [];

    this.gameField.gameFieldCells.forEach(cells => {
      shipcells.push(...cells.filter(cell => cell.gameShipId === this.selectedShip?.id))
    });

    for (var i: number = 0; i < this.gameField.sizeX; i++) {
      for (var j: number = 0; j < this.gameField.sizeY; j++) {
        this.labels[i][j] = Math.min(...shipcells.map(cell =>
          this.getDistanceBetween2Points(cell.x, cell.y, i + 1, j + 1))) <= this.selectedShip?.speed! && this.checkNearShip(i, j) ? true : false;
      }
    }
  }

  private setAtackArea() {
    this.getShipCoords();

    const centerShipX: number = (this.shipCoords[0].x + this.shipCoords[this.shipCoords.length - 1].x) / 2 - 0.5;
    const centerShipY: number = (this.shipCoords[0].y + this.shipCoords[this.shipCoords.length - 1].y) / 2 - 0.5;

    const ship: GameShip = new GameShip(this.selectedShip!);

    for (var i: number = 0; i < this.gameField.sizeX; i++) {
      for (var j: number = 0; j < this.gameField.sizeY; j++) {
        this.labels[i][j] = this.getDistanceBetween2Points(i + 0.5, j + 0.5, centerShipX, centerShipY) <= ship.attackRange() ? true : false;
      }
    }
  }

  private getShipCoords() {
    this.shipCoords = [];

    for (var i: number = 0; i < this.gameField.sizeX; i++) {
      for (var j: number = 0; j < this.gameField.sizeY; j++) {
        if (this.gameField.gameFieldCells[i][j].gameShipId === this.selectedShip?.id) {
          this.shipCoords.push(this.gameField.gameFieldCells[i][j]);

          let ti: number = i + 1;
          while (this.gameField.gameFieldCells[ti][j].gameShipId === this.selectedShip?.id) {
            this.shipCoords.push(this.gameField.gameFieldCells[ti][j]);
            ti++;
          }

          let tj: number = j + 1;
          while (this.gameField.gameFieldCells[i][tj].gameShipId === this.selectedShip?.id) {
            this.shipCoords.push(this.gameField.gameFieldCells[i][tj]);
            tj++;
          }

          return;
        }
      }
    }
  }

  private getDistanceBetween2Points(num1X: number, num1Y: number, num2X: number, num2Y: number): number {
    return Math.sqrt(Math.pow(num1X - num2X, 2) + Math.pow(num1Y - num2Y, 2));
  }

  private checkNearShip(x: number, y: number): boolean {
    for (var i: number = x - 1 < 0 ? 0 : x - 1; i < (x + 2 >= this.gameField.sizeX ? this.gameField.sizeX : x + 2); i++) {
      for (var j: number = y - 1 < 0 ? 0 : y - 1; j < (y + 2 >= this.gameField.sizeY ? this.gameField.sizeY : y + 2); j++) {
        const cell: GameFieldCell = this.gameField.gameFieldCells[i][j];
        if (cell.gameShipId && cell.gameShipId !== this.selectedShip?.id) {
          return false;
        }
      }
    }

    return true;
  }

  endMove() {
    if (this.game.currentPlayerMove === this.player?.name) {
      this.isActioned = false;
      this.isMoved = false;
      this.selectedShip = null;
      this.clickCell = null;
      this.clearLabelArea();

      this.actionService.endMove().subscribe(state => {
        if (state === 10) {
          this.gameService.getGameFromServer().subscribe(game => {
            this.game = game;
            this.subscription = this.source.subscribe(val => this.update());
          })
        }
      })
    }
  }

  private moveShip(cell: GameFieldCell, direction: Direction) {
    if (this.selectedShip) {
      this.actionService.move(this.selectedShip.id, cell, direction).subscribe(state => {
        if (state === 10) {
          this.isMoved = true;

          this.gameFieldService.getGameFieldFromServer().subscribe(f => {
            this.gameField = f;

            if (this.isActioned) {
              this.endMove();
            }
            else {
              this.setAtackArea();
            }
          });
        }
        else {
          this.tempCoords.forEach(c => {
            this.gameField.gameFieldCells[c.x - 1][c.y - 1].gameShipId = null;
            this.gameField.gameFieldCells[c.x - 1][c.y - 1].playerId = null;
          });
          this.shipCoords.forEach(c => {
            this.gameField.gameFieldCells[c.x - 1][c.y - 1].gameShipId = this.selectedShip?.id!;
            this.gameField.gameFieldCells[c.x - 1][c.y - 1].playerId = this.selectedShip?.gamePlayerId!;
          });
          this.tempCoords = [];
        }

        this.clickCell = null;
      })
    }
  }

  private update() {
    this.gameService.getGameFromServer().subscribe(game => {
      if (game.currentPlayerMove !== this.game.currentPlayerMove) {
        //update game
        this.game = game;

        //update game field
        this.gameFieldService.getGameFieldFromServer().subscribe(field => {
          this.gameField = field;

          //update ships
          const shipsId: number[] = [];
          field.gameFieldCells.forEach(async (shipcells) => shipcells.forEach(
            async (shipCell) => {
              if (shipCell.gameShipId && !shipsId.includes(shipCell.gameShipId)) {
                this.shipService.getShipFromServer(shipCell.gameShipId).subscribe();
                shipsId.push(shipCell.gameShipId);
              }
            }
          ))
        });
      }

      //stop update and wait move of this player
      if (this.game.currentPlayerMove === this.player?.name) {
        this.subscription.unsubscribe();
      }
    })
  }
}
