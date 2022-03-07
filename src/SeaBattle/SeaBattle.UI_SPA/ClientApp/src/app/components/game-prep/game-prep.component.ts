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

  constructor(private fieldService: DataGameFieldService) { }

  ngOnInit(): void {
    this.fieldService.getGameField().subscribe(f => this.gameField = f);
  }

  onNotifyGameField(cell: GameFieldCell) {
    console.log(cell);
  }

}
