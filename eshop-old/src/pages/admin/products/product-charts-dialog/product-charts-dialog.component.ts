import { ProductStockChange } from './../../../../domain/product';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexTitleSubtitle,
  NgApexchartsModule,
  ApexStroke
} from "ng-apexcharts";;
import { MaterialModule } from './../../../../app/material/material.module';
import { Component, Inject, OnInit } from '@angular/core';
import { Product, ProductPriceChange } from '../../../../domain/product';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { WebApiService } from '../../../../services/web-api.service';
import { DatePipe } from '@angular/common';
export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  title: ApexTitleSubtitle;
};

@Component({
  selector: 'app-product-charts-dialog',
  standalone: true,
  imports: [MaterialModule, NgApexchartsModule, DatePipe],
  templateUrl: './product-charts-dialog.component.html',
  styleUrl: './product-charts-dialog.component.scss'
})
export class ProductChartsDialogComponent implements OnInit {
  public chartOptions: Partial<ChartOptions> | null = null;

  priceChanges: ProductPriceChange[] = []
  stockChanges: ProductStockChange[] = []
  constructor(@Inject(MAT_DIALOG_DATA) public product: Product, private ref: MatDialogRef<ProductChartsDialogComponent>, private api: WebApiService, private datePipe: DatePipe) {

  }

  ngOnInit(): void {
    this.refresh();
  }
  async refresh() {
    const chartOptions: Partial<ChartOptions> = {
      series: [
        {
          name: "Price Changes",
          data: [],
        },
        {
          name: "Stock Changes",
          data: [],
        }
      ],
      chart: {
        height: 350,
        type: "line",

      },
      stroke: {
        curve: 'smooth'
      },
      title: {
        text: "Product History"
      },
      xaxis: {
        type: 'datetime',
        categories: []
      }
    }
    this.priceChanges = await this.api.getProductPriceChanges(this.product);
    this.stockChanges = await this.api.getProductStockChanges(this.product);
    const values: { [key: string]: { price: number, stock: number } } = {}
    const priceSeries: number[] = []
    const labels: string[] = []
    this.priceChanges.forEach(e => {
      values[e.timestamp] = { price: e.price, stock: 0 };
    });
    const stockSeries: number[] = []


    this.stockChanges.forEach(e => {
      if (values[e.timestamp])
        values[e.timestamp].stock = e.stock;
      else
        values[e.timestamp] = { price: 0, stock: e.stock };
    });

    const keys = Object.keys(values).sort();

    for (let i = 0; i < keys.length; i++) {
      const v = values[keys[i]];
      if (v.price == 0 && i > 0)
        v.price = values[keys[i - 1]].price
      if (v.stock == 0 && i > 0)
        v.stock = values[keys[i - 1]].stock
      labels.push(keys[i])
      priceSeries.push(v.price)
      stockSeries.push(v.stock)
    }



    chartOptions.series![0].data = priceSeries;
    chartOptions.series![1].data = stockSeries;
    chartOptions.xaxis!.categories = labels.sort();
    console.log(this.chartOptions)
    this.chartOptions = chartOptions
  }
}
