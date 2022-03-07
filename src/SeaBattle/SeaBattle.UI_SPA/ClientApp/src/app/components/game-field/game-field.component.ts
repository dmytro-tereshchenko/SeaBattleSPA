import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { GameField } from '../../data/game-field';
import { GameFieldCell } from '../../data/game-field-cell';

@Component({
  selector: 'app-game-field',
  templateUrl: './game-field.component.html',
  styleUrls: ['./game-field.component.scss']
})
export class GameFieldComponent implements OnInit {

  @Input() field: GameField
  @Output() notify = new EventEmitter<GameFieldCell>();

  constructor() { }

  ngOnInit(): void {
  }

}
