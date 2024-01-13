import { Component } from '@angular/core';
import { MaterialModule } from '../../app/material/material.module';
import { ProductsComponent } from './products/products.component';
import { OrdersComponent } from './orders/orders.component';
import { ClientsComponent } from './clients/clients.component';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [MaterialModule, ProductsComponent, OrdersComponent, ClientsComponent, CommonModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent {
  selectedEntity: "clients" | "products" | "orders" = "orders";
  constructor(private router: Router) {

  }
  logout() {
    sessionStorage.clear();
    this.router.navigateByUrl("/login")
  }
}
