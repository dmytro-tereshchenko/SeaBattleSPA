import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { GameSizeLimit } from '../../data/game-size-limit';
import { InitializeGameService } from '../../services/initialize-game.service';

@Component({
  selector: 'app-create-game',
  templateUrl: './create-game.component.html',
  styleUrls: ['./create-game.component.scss']
})
export class CreateGameComponent implements OnInit {

  constructor(private location: Location, private apiService: InitializeGameService) {
    this.players = 2;
    this.gameSize = {
      maxPlayerSize: 4,
      fieldMaxSizeX: 100,
      fieldMaxSizeY: 100,
      fieldMinSizeX: 10,
      fieldMinSizeY: 10
    } as GameSizeLimit;
    this.sizeX = this.gameSize.fieldMinSizeX;
    this.sizeY = this.gameSize.fieldMinSizeY;
  }

  players: Number;
  sizeX: Number;
  sizeY: Number;
  gameSize: GameSizeLimit;

  ngOnInit(): void {
    this.apiService.GetGameSize().subscribe(result => {
      this.gameSize = result;
      this.sizeX = result.fieldMinSizeX;
      this.sizeY = result.fieldMinSizeY;
    })
  }

  goBack(): void {
    this.location.back();
  }

  createGame(): void {
    console.log(this.players);
  }

}
