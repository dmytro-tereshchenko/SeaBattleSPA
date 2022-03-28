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
          if (ship.gamePlayerId === this.player?.id) {
            if (this.selectedShip?.id !== cell.gameShipId) {
              this.clickCell = null;
            }
            else if (this.labels[cell.x - 1][cell.y - 1]) {
              this.clickCell = cell;
            }

            //can change choice of player ship if you haven't moved or actioned 
            if (!this.isMoved && !this.isActioned) {
              this.selectedShip = ship;
              changeShip = true;
              this.setMoveArea();
            }
          }
        })
      }
      if (this.selectedShip && !changeShip) {
        if (this.clickCell) {
          this.moveShip(this.clickCell, this.actionService.getDirection(this.clickCell, cell));
        }
        else {
          this.shipService.getShip(this.selectedShip.id).subscribe(ship => {
            if (ship.size === 1) {
              if (cell.gameShipId !== this.selectedShip?.id) {
                this.moveShip(cell, Direction.xDec);
              }
            }
            else if (this.labels[cell.x - 1][cell.y - 1]) {
              this.clickCell = cell;
            }
          })
        }
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

  //in case actioned update game field 
  private updateAfterAction() {
    this.gameFieldService.getGameFieldFromServer().subscribe(field => this.gameField = field);

    if (this.isMoved) {
      this.endMove();
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

  private getDistanceBetween2Points(num1X: number, num1Y: number, num2X: number, num2Y: number): number {
    return Math.sqrt(Math.pow(num1X - num2X, 2) + Math.pow(num1Y - num2Y, 2));
  }

  private checkNearShip(x: number, y: number): boolean {
    for (var i: number = x - 1 < 0 ? 0 : x - 1; i <= (x + 1 >= this.gameField.sizeX ? this.gameField.sizeX : x + 1); i++) {
      for (var j: number = y - 1 < 0 ? 0 : y - 1; j <= (y + 1 >= this.gameField.sizeY ? this.gameField.sizeY : y + 1); j++) {
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

          this.gameFieldService.getGameFieldFromServer().subscribe(f => this.gameField = f);

          this.clearLabelArea();

          if (this.isActioned) {
            this.endMove();
          }
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
