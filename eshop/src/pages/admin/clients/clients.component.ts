import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MaterialModule } from '../../../app/material/material.module';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { DatePipe, DecimalPipe } from '@angular/common';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { WebApiService } from '../../../services/web-api.service';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { Client } from '../../../domain/product';
import { ClientsDialogComponent } from './clients-dialog/clients-dialog.component';

@Component({
  selector: 'app-clients',
  standalone: true,
  imports: [MaterialModule, MatIconModule, HttpClientModule, DecimalPipe, DatePipe],

  templateUrl: './clients.component.html',
  styleUrl: './clients.component.scss'
})
export class ClientsComponent implements OnInit, OnDestroy, AfterViewInit {
  subs: Subscription[] = [];
  displayedColumns: string[] = ['actions', 'firstName', 'lastName', 'personalCode', 'idNumber', 'dateOfBirth', 'ammountOfPoints'];
  dataSource = new MatTableDataSource<Client>([]);
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
    this.dataSource.data = await this.api.getClients()
  }

  insert() {
    this.showDialog({
      id: -1,
      firstName: "",
      lastName: "",
      personalCode: "",
      idNumber: "",
      dateOfBirth: new Date().toISOString(),
      ammountOfPoints: 0,

    }, (c) => {
      this.api.insertClient(c).then((r) => { this.refresh().then(e => this.dataSource.paginator?.lastPage()) })
    })
  }
  edit(client: Client) {
    this.showDialog(client, (c) => {
      this.api.updateClient(c).then((r) => { })
    })
  }
  showDialog(client: Client, callback: (client: Client) => void) {
    const ref = this.dialog.open(ClientsDialogComponent, {
      data: client
    })
    this.subs.push(ref.afterClosed().subscribe((val: any) => {
      if (val != null)
        callback(val);
    }))
  }
  delete(client: Client) {
    this.api.deleteClient(client).then(e => this.refresh())
  }

}
