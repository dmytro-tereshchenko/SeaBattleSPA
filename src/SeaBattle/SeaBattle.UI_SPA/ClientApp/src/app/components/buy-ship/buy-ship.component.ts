import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Ship } from 'src/app/data/ship';
import { InitializeGameService } from '../../services/initialize-game.service';
import { DataShipService } from '../../services/data-ship.service';

@Component({
  selector: 'app-buy-ship',
  templateUrl: './buy-ship.component.html',
  styleUrls: ['./buy-ship.component.scss']
})
export class BuyShipComponent implements OnInit {

  constructor(private initService: InitializeGameService,
    private shipService: DataShipService) { }

  @Output() notify = new EventEmitter();

  displayedColumns: string[] = ['shipType', 'size', 'maxHp', 'speed', 'cost', 'buttons'];
  dataSource: MatTableDataSource<Ship>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ngOnInit(): void {
    this.initService.GetShips().subscribe(s => {
      this.dataSource = new MatTableDataSource(s);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  buyShip(row: Ship) {
    this.shipService.buyShip(row.id).subscribe(state => {
      if (state === 10) {
        this.notify.emit();
      }
    });
  }

}
