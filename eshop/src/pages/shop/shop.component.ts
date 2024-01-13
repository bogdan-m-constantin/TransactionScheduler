import { Product } from './../../domain/product';
import { Component, HostListener, OnInit } from '@angular/core';
import { MaterialModule } from '../../app/material/material.module';
import { Router } from '@angular/router';
import { WebApiService } from '../../services/web-api.service';
import { Order } from '../../domain/product';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  shoppingCart = true;
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
    this.api.getProducts().then(e => this.products = e)
  }
}
