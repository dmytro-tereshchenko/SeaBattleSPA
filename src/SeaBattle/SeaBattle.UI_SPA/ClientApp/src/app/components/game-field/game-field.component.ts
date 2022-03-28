import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';
import { colorPlayers } from '../../data/mock-color-players';
import { DataGameService } from '../../services/data-game.service';
import { DataShipService } from '../../services/data-ship.service';
import { AuthService } from '../../core/services/auth.service';
import { GameShip } from 'src/app/data/game-ship';

@Component({
  selector: 'app-game-field',
  templateUrl: './game-field.component.html',
  styleUrls: ['./game-field.component.scss']
})
export class GameFieldComponent implements OnInit {

  @Input() field: GameField;
  @Input() labels: boolean[][];
  @Input() height: string;
  @Input() selectedShipId: number | null;
  @Input() clickCell: GameFieldCell | null;
  @Output() notifyClick = new EventEmitter<GameFieldCell>();
  @Output() notifyDblClick = new EventEmitter<GameFieldCell>();

  toolTip: string;
  private colorShips: string[] = colorPlayers;
  private playersId: number[];
  private gameState: number;
  private isSingleClick: Boolean = true;

  constructor(private userService: AuthService,
    private gameService: DataGameService,
    private shipService: DataShipService) { }

  ngOnInit(): void {
    this.userService.getUser().then(user => this.gameService.getGame().subscribe(game => {
      this.gameState = game.gameState;
      this.playersId = [];
      var player = game.players.find(p => p.name === (user?.profile.name ?? ''));

      if (player) {
        this.playersId.push(player.id);
      }

      //if initialize field then show only players ship, otherwise, add ships of other players
      if (game.gameState !== 3) {
        game.players.forEach(p => {
          if (p.id !== this.playersId[0]) {
            this.playersId.push(p.id);
          }
        })
      }
    }))
  }

  getShipColor(cell: GameFieldCell): string {
    if (cell) {
      if (this.clickCell === cell || (cell.gameShipId !== null && cell.gameShipId === this.selectedShipId)) {
        return "red";
      }

      let index = this.playersId.findIndex(i => i === cell.playerId);

      return index === -1 ? this.labelled(cell) ? "rgba(255, 255, 255, 0.5)" : "rgba(255, 255, 255, 0)" : this.colorShips[index];
    }
    else {
      return "rgba(255, 255, 255, 0)";
    }
  }

  selected(cell: GameFieldCell): boolean {
    if (cell === undefined) {
      return false;
    }
    else if ((cell.gameShipId && this.playersId.includes(cell.playerId ?? -1)) || this.selectedShipId && (this.labelled(cell) || this.gameState === 4)) {
      return true;
    }
    else {
      return false;
    }
  }

  labelled(cell: GameFieldCell): boolean {
    if (cell === undefined || this.labels === undefined) {
      return false;
    }

    return this.labels[cell.x - 1][cell.y - 1]
  }

  callForClick(cell: GameFieldCell) {
    this.isSingleClick = true;
    setTimeout(() => {
      if (this.isSingleClick) {
        this.notifyClick.emit(cell);
      }
    }, 250);
  }

  callForDblClick(cell: GameFieldCell) {
    this.isSingleClick = false;
    this.notifyDblClick.emit(cell);
  }

  mouseEnter(cell: GameFieldCell): void {
    if (cell.gameShipId) {
      this.shipService.getShip(cell.gameShipId)
        .subscribe(ship => this.gameService.getGame()
          .subscribe(game => {
            const player = game.players.find(p => p.id === ship.gamePlayerId)?.name;
            let toolTip: string = `${player}\nHP: ${ship.hp}/${ship.maxHp}`;
            let gameShip: GameShip = new GameShip(ship);

            if (gameShip.damage() > 0) {
              toolTip += `\nDamage: ${gameShip.damage()}`;
            }

            if (gameShip.repairPower() > 0) {
              toolTip += `\nRepair power: ${gameShip.repairPower()}`;
            }

            this.toolTip = toolTip;
          }))
    }
    else {
      this.toolTip = "";
    }
  }
}
