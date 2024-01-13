import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { Client, Order } from '../../../domain/product';
import { WebApiService } from '../../../services/web-api.service';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MaterialModule } from '../../../app/material/material.module';
import { DatePipe, DecimalPipe } from '@angular/common';
import { MatSort } from '@angular/material/sort';
import { OrderDetailsDialogComponent } from './order-details-dialog/order-details-dialog.component';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [MaterialModule, DatePipe, DecimalPipe],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit, OnDestroy, AfterViewInit {
  subs: Subscription[] = [];
  displayedColumns: string[] = ["id", "timestamp", "client", "total"];
  dataSource = new MatTableDataSource<Order>([]);
  clients: { [key: number]: Client } = {}

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | null = null;
  @ViewChild(MatSort) sort: MatSort | null = null;

  constructor(private api: WebApiService, private dialog: MatDialog) {

  }
  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  ngOnInit(): void {
    this.refresh()
  }

  async refresh() {
    (await this.api.getClients()).forEach(c => {
      this.clients[c.id] = c;
    })
    this.dataSource.data = await this.api.getOrders()
    // this.showDetails(this.dataSource.data[0])

  }
  showDetails(order: Order) {
    this.dialog.open(OrderDetailsDialogComponent, {
      data: {
        order,
        client: this.clients[order.client]
      }
    })
  }


}
