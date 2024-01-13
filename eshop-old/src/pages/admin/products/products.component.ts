import { Product } from './../../../domain/product';
import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';

import { MatIconModule } from '@angular/material/icon';
import { MaterialModule } from '../../../app/material/material.module';
import { WebApiService } from '../../../services/web-api.service';
import { HttpClientModule } from '@angular/common/http';
import { DecimalPipe } from '@angular/common';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { ProductDialogComponent } from './product-dialog/product-dialog.component';
import { ProductChartsDialogComponent } from './product-charts-dialog/product-charts-dialog.component';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [MaterialModule, MatIconModule, HttpClientModule, DecimalPipe],
  providers: [],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit, AfterViewInit, OnDestroy {
  subs: Subscription[] = [];
  displayedColumns: string[] = ['actions', 'name', 'description', 'stock', 'price'];
  dataSource = new MatTableDataSource<Product>([]);
  constructor(private api: WebApiService, private dialog: MatDialog) {

  }
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | null = null;
  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  ngOnInit(): void {
    this.refresh()
  }

  async refresh() {
    this.dataSource.data = await this.api.getProducts()
  }
  showCharts(product: Product) {
    this.dialog.open(ProductChartsDialogComponent, {
      data: product,
      width: "80vw",
    })
  }
  createProduct() {
    this.showDialog({
      id: -1,
      description: "",
      name: "",
      price: 0,
      stock: 0
    }, (p) => {
      this.api.insertProduct(p).then((r) => { this.refresh().then(e => this.dataSource.paginator?.lastPage()) })
    })
  }
  updateProduct(prod: Product) {
    this.showDialog(prod, (p) => {
      this.api.updateProduct(p).then((r) => { })
    })
  }
  showDialog(prod: Product, callback: (product: Product) => void) {
    const ref = this.dialog.open(ProductDialogComponent, {
      data: prod
    })
    this.subs.push(ref.afterClosed().subscribe((val: any) => {
      if (val != null)
        callback(val);
    }))
  }
  deleteProduct(prod: Product) {
    this.api.deleteProduct(prod).then(e => this.refresh())
  }

}
