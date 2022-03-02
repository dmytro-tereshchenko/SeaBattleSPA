import { AfterViewInit, Component, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { GameSearch } from '../../data/game-search';
import { InitializeGameService } from '../../services/initialize-game.service';
import { DataGameService } from '../../services/data-game.service';
import { AuthService } from '../../core/services/auth.service';
import { Router } from "@angular/router"

@Component({
  selector: 'app-search-game-table',
  templateUrl: './search-game-table.component.html',
  styleUrls: ['./search-game-table.component.scss']
})
export class SearchGameTableComponent implements AfterViewInit {

  displayedColumns: string[] = ['id', 'maxNumberOfPlayers', 'gameFieldSize', 'players'];
  dataSource: MatTableDataSource<GameSearch>;
  waitPlayers: boolean;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private apiService: InitializeGameService,
    private gameService: DataGameService,
    private userService: AuthService,
    private router: Router) { }

  ngAfterViewInit() {
    this.apiService.GetSearch().subscribe(g => {
      this.dataSource = new MatTableDataSource(g);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      if (g.length > 0 && g[0].gameState === 3) {
        this.router.navigate(['/game-prep']);
      }
      if (g.length > 0) {
        this.userService.getUser().then(u => {
          if (g[0].players.split(", ").includes(u?.profile.name ?? "")) {
            this.waitPlayers = true;
          }
        })
      }
    })
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  joinGame(row: GameSearch) {
    this.gameService.joinPlayer(row.id).subscribe();
  }
}
