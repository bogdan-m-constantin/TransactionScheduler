import { Client, Product } from './../../domain/product';
import { Component, HostListener, OnInit } from '@angular/core';
import { MaterialModule } from '../../app/material/material.module';
import { Router } from '@angular/router';
import { WebApiService } from '../../services/web-api.service';
import { Order } from '../../domain/product';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [MaterialModule, DecimalPipe],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  shoppingCart = true;
  client: Client | null = null;
  order: Order = {
    id: -1,
    client: 1,
    items: [],
    timestamp: new Date().toISOString(),
    total: 0
  }
  products: Product[] = []
  constructor(private router: Router, private api: WebApiService) {

  }
  @HostListener("click", ['$event'])
  onClick(ev: MouseEvent) {
    if (this.shoppingCart) {
      this.shoppingCart = false;
      ev.stopPropagation();

    }
  }
  logout() {
    sessionStorage.clear();
    this.router.navigateByUrl("/login")
  }
  showShoppingCart(ev: MouseEvent) {
    ev.stopPropagation();
    this.shoppingCart = !this.shoppingCart
  }
  ngOnInit(): void {
    this.api.getProducts().then(e => { this.products = e; e.forEach(e => this.addToCart(e)) })

    this.api.getClient(this.order.client).then(e => this.client = e);
  }
  addToCart(product: Product): void {
    let index = this.order.items.findIndex(e => e.productId == product.id);
    if (index == -1) {
      this.order.items.push({
        id: -1,
        price: product.price,
        productName: product.name,
        quantity: 1,
        productId: product.id
      })
    }
    else {
      this.order.items[index].quantity += 1;
    }
    this.order.total = this.order.items.reduce((prev, current) => prev + current.price * current.quantity, 0)

  }
  async finishOrder() {
    await this.api.insertOrder(this.order)
    this.order = {
      id: -1,
      client: 1,
      items: [],
      timestamp: new Date().toISOString(),
      total: 0
    }
    this.client = await this.api.getClient(this.order.client);
  }
}
