import { Component, OnInit } from '@angular/core';
import { Observable, interval, Subscription } from 'rxjs';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';
import { Game } from '../../data/game';
import { Player } from '../../data/player';
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
  selectedShipId: number | null;
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
    this.selectedShipId = null;
    this.isMoved = false;
    this.isActioned = false;
  }

  ngOnInit(): void {
    this.source = interval(this.updateTimeOut);

    this.gameFieldService.getGameField().subscribe(f => this.gameField = f);

    this.userService.getUser().then(user => this.gameService.getGame().subscribe(game => {
      this.game = game;
      this.players = [];
      this.player = game.players.find(p => p.name === (user?.profile.name ?? ''));

      if (this.player) {
        this.players.push(this.player.name);
      }

      game.players.forEach(p => {
        if (p.name !== this.players[0]) {
          this.players.push(p.name);
        }
      })
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

  }

  onNotifyGameFieldDblClick(cell: GameFieldCell) {
    if (this.game.currentPlayerMove == this.player?.name &&
      cell &&
      cell.gameShipId &&
      !this.isActioned &&
      ((this.selectedShipId == null && cell.gameShipId) || (
        this.selectedShipId && cell.gameShipId === this.selectedShipId))) {
      this.actionService.repairAll(cell.gameShipId).subscribe(state => {
        if (state === 10 || state === 21) {
          this.isActioned = true;
          this.selectedShipId = cell.gameShipId;

          this.actionService.getVissibleShips(cell.gameShipId!, ActionType.repair).subscribe(ships => {
            ships.forEach(shipId => this.shipService.getShipFromServer(shipId))
          })

          this.gameFieldService.getGameFieldFromServer().subscribe(field => this.gameField = field);

          if (this.isMoved) {
            this.endMove();
          }
        }
      })
    }
  }

  endMove() {
    this.isActioned = false;
    this.isMoved = false;
    this.selectedShipId = null;

    this.actionService.endMove().subscribe(state => {
      if (state === 10) {
        this.gameService.getGameFromServer().subscribe(game => {
          this.game = game;
          this.subscription = this.source.subscribe(val => this.update());
        })
      }
    })
  }

  private update() {
    this.gameService.getGameFromServer().subscribe(game => {
      if (game.currentPlayerMove !== this.game.currentPlayerMove) {
        this.game = game;
        this.gameFieldService.getGameFieldFromServer().subscribe(field => this.gameField = field);
        this.subscription.unsubscribe();
      }
    })
  }
}
