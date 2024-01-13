import { Component, Inject } from '@angular/core';
import { Client, Order } from '../../../../domain/product';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '../../../../app/material/material.module';
import { DatePipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-order-details-dialog',
  standalone: true,
  imports: [MaterialModule, DatePipe, DecimalPipe],
  templateUrl: './order-details-dialog.component.html',
  styleUrl: './order-details-dialog.component.scss'
})
export class OrderDetailsDialogComponent {
  order: Order;
  client: Client
  constructor(@Inject(MAT_DIALOG_DATA) data: { order: Order, client: Client }, private ref: MatDialogRef<OrderDetailsDialogComponent>) {
    this.order = data.order;
    this.client = data.client;
  }

  close() {
    this.ref.close();
  }
}
