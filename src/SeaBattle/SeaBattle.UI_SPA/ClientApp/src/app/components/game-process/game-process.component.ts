import { Component, OnInit } from '@angular/core';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';
import { Game } from '../../data/game';
import { DataGameService } from '../../services/data-game.service';
import { DataGameFieldService } from '../../services/data-game-field.service';

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

  constructor(private gameFieldService: DataGameFieldService,
    private gameService: DataGameService) {
    this.gameFieldHeight = "80vh";
    this.clickCell = null;
    this.selectedShipId = null;
  }

  ngOnInit(): void {
    this.gameFieldService.getGameField().subscribe(f => this.gameField = f);
    this.gameService.getGame().subscribe(g => this.game = g);
  }

  onNotifyGameFieldClick(cell: GameFieldCell) {

  }

  onNotifyGameFieldDblClick(cell: GameFieldCell) {

  }

}
