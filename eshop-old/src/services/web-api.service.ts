import { Client, Order, Product, ProductPriceChange, ProductStockChange } from './../domain/product';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WebApiService {
  baseUrl = "http://localhost:5288"
  constructor(private http: HttpClient) {

  }

  async getProducts(): Promise<Product[]> {
    return firstValueFrom(this.http.get<Product[]>(`${this.baseUrl}/products`))
  }
  async insertProduct(product: Product): Promise<Product> {
    return firstValueFrom(this.http.post<Product>(`${this.baseUrl}/products`, product))
  }

  async updateProduct(product: Product): Promise<Product> {
    return firstValueFrom(this.http.put<Product>(`${this.baseUrl}/products`, product))
  }
  async deleteProduct(product: Product): Promise<void> {
    return firstValueFrom(this.http.delete<void>(`${this.baseUrl}/products/${product.id}`))
  }
  async getProductPriceChanges(product: Product): Promise<ProductPriceChange[]> {
    return firstValueFrom(this.http.get<ProductPriceChange[]>(`${this.baseUrl}/products/price-changes/${product.id}`))
  }
  async getProductStockChanges(product: Product): Promise<ProductStockChange[]> {
    return firstValueFrom(this.http.get<ProductStockChange[]>(`${this.baseUrl}/products/stock-changes/${product.id}`))
  }
  async getClients(): Promise<Client[]> {
    return firstValueFrom(this.http.get<Client[]>(`${this.baseUrl}/clients`))
  }
  async insertClient(client: Client): Promise<Client> {
    return firstValueFrom(this.http.post<Client>(`${this.baseUrl}/clients`, client))
  }

  async updateClient(client: Client): Promise<Client> {
    return firstValueFrom(this.http.put<Client>(`${this.baseUrl}/clients`, client))
  }
  async deleteClient(client: Client): Promise<void> {
    return firstValueFrom(this.http.delete<void>(`${this.baseUrl}/clients/${client.id}`))
  }

  async getOrders(client: number = -1): Promise<Order[]> {
    return firstValueFrom(this.http.get<Order[]>(`${this.baseUrl}/orders/${client}`))
  }
}
