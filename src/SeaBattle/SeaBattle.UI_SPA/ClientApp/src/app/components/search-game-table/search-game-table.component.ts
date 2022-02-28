import {AfterViewInit, Component, ViewChild, Input } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { GameSearch } from '../../data/game-search';
import { InitializeGameService } from '../../services/initialize-game.service';
import { DataGameService } from '../../services/data-game.service';

export interface GameData {
  id: string;
  maxNumberOfPlayers: string;
  size: string;
  players: string;
}

@Component({
  selector: 'app-search-game-table',
  templateUrl: './search-game-table.component.html',
  styleUrls: ['./search-game-table.component.scss']
})
export class SearchGameTableComponent implements AfterViewInit {

  displayedColumns: string[] = ['id', 'maxNumberOfPlayers', 'gameFieldSize', 'players'];
  dataSource: MatTableDataSource<GameSearch>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private apiService: InitializeGameService,
    private gameService: DataGameService) {}

  ngAfterViewInit() {
    this.apiService.GetSearch().subscribe(g=>{
      this.dataSource = new MatTableDataSource(g);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;});
    }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  joinGame(row: GameSearch){
    this.gameService.joinPlayer(row.id).subscribe(g=>console.log(g));
  }
}
