import { AfterViewInit, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { Client, Order, OrderItem } from '../../../../domain/product';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '../../../../app/material/material.module';
import { DatePipe, DecimalPipe } from '@angular/common';
import { WebApiService } from '../../../../services/web-api.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-order-details-dialog',
  standalone: true,
  imports: [MaterialModule, DatePipe, DecimalPipe],
  templateUrl: './order-details-dialog.component.html',
  styleUrl: './order-details-dialog.component.scss'
})
export class OrderDetailsDialogComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ["product", "quantity", "price", "total"];
  order: Order;
  age = 0;
  orderItems: OrderItem[] = [];
  client: Client;
  dataSource = new MatTableDataSource<OrderItem>([]);
  clients: { [key: number]: Client } = {}

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | null = null;
  @ViewChild(MatSort) sort: MatSort | null = null;
  constructor(@Inject(MAT_DIALOG_DATA) data: { order: Order, client: Client }, private ref: MatDialogRef<OrderDetailsDialogComponent>, private api: WebApiService) {
    this.order = data.order;
    var diff_ms = Date.now() - new Date(data.client.dateOfBirth).getTime();
    var age_dt = new Date(diff_ms);

    this.age = Math.abs(age_dt.getUTCFullYear() - 1970);

    this.client = data.client;
  }
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  ngOnInit(): void {
    this.refresh()
  }
  async refresh() {
    this.dataSource.data = await this.api.getOrderItems(this.order.id);
  }
  close() {
    this.ref.close();
  }
}
