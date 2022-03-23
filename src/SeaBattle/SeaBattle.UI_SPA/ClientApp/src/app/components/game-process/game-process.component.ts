import { Component, OnInit } from '@angular/core';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';
import { Game } from '../../data/game';
import { colorPlayers } from '../../data/mock-color-players';
import { DataGameService } from '../../services/data-game.service';
import { DataGameFieldService } from '../../services/data-game-field.service';
import { AuthService } from '../../core/services/auth.service';

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

  constructor(private gameFieldService: DataGameFieldService,
    private gameService: DataGameService,
    private userService: AuthService) {
    this.gameFieldHeight = "80vh";
    this.clickCell = null;
    this.selectedShipId = null;
  }

  ngOnInit(): void {
    this.gameFieldService.getGameField().subscribe(f => this.gameField = f);

    this.userService.getUser().then(user => this.gameService.getGame().subscribe(game => {
      this.game = game;
      this.players = [];
      var player = game.players.find(p => p.name === (user?.profile.name ?? ''));

      if (player) {
        this.players.push(player.name);
      }

      game.players.forEach(p => {
        if (p.name !== this.players[0]) {
          this.players.push(p.name);
        }
      })
    }))
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

  }

}
