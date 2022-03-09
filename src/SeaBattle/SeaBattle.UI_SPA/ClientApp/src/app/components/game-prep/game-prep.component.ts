import { Component, OnInit } from '@angular/core';
import { DataGameFieldService } from '../../services/data-game-field.service';
import { Observable } from 'rxjs';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';

@Component({
  selector: 'app-game-prep',
  templateUrl: './game-prep.component.html',
  styleUrls: ['./game-prep.component.scss']
})
export class GamePrepComponent implements OnInit {

  gameField: GameField;
  labels: boolean[][];

  constructor(private fieldService: DataGameFieldService) { }

  ngOnInit(): void {
    this.fieldService.getGameField().subscribe(f => {
      this.gameField = f;

      this.labels = [];
      for (var i: number = 0; i < f.sizeX; i++) {
        this.labels[i] = [];
        for (var j: number = 0; j < f.sizeY; j++) {
          this.labels[i][j] = false;
        }
      }

      // this.gameField.gameFieldCells[1][1]=<GameFieldCell>{
      //   id: null,
      //   x: 1,
      //   y: 1,
      //   stern: false,
      //   gameShipId: 1,
      //   playerId: 1
      // }
      // this.gameField.gameFieldCells[1][2]=<GameFieldCell>{
      //   id: null,
      //   x: 1,
      //   y: 2,
      //   stern: false,
      //   gameShipId: 1,
      //   playerId: 1
      // }

      // let scores = new Map<string, string>();
      // scores.set("bill", "bill@somewhere.com");
      // scores.set("bob", "bob@somewhere.com");
      // scores.get("bill");
      // console.log(scores.has("bill")); // true
    });
  }

  onNotifyGameField(cell: GameFieldCell) {
    console.log(cell);
  }

}
