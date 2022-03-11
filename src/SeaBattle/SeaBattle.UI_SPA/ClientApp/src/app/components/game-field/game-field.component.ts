import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';
import { DataGameService } from '../../services/data-game.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-game-field',
  templateUrl: './game-field.component.html',
  styleUrls: ['./game-field.component.scss']
})
export class GameFieldComponent implements OnInit {

  @Input() field: GameField;
  @Input() labels: boolean[][];
  @Input() height: string;
  @Output() notify = new EventEmitter<GameFieldCell>();

  color: string;
  private colorShips: string[] = ["green", "yellow", "brown", "gray"];
  private playersId: number[];

  constructor(private userService: AuthService,
    private gameService: DataGameService) { }

  ngOnInit(): void {
    this.userService.getUser().then(user => this.gameService.getGame().subscribe(game => {
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
    let index = this.playersId.findIndex(i => i === cell.playerId);

    return index === -1 ? this.labelled(cell) ? "rgba(255, 255, 255, 0.5)" : "rgba(255, 255, 255, 0)" : this.colorShips[index];
  }

  selected(cell: GameFieldCell): boolean {
    if (cell === undefined) {
      return false;
    }
    else if (cell.gameShipId && this.playersId.includes(cell.playerId ?? -1)) {
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

    return this.labels[cell.x][cell.y]
  }

}
