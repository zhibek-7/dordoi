import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {
  MatSort,
  MatTableDataSource,
  MatPaginator,
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material';
import { PARAMETERS } from '@angular/core/src/util/decorators';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {
  displayedColumns: string[] = [
    'date',
    'initialize',
    'type',
    'text',
    'details',
  ];
  dataSource = new MatTableDataSource(EVENTS_DATA);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  initializer: string = '';
  eventType: string = '';
  txt: string = '';

  constructor() { }

  ngOnInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  applyFilter(pickerStart, pickerEnd, initializer, eventType, txt) {
    console.log(arguments);
    // this.dataSource.filter = filterValue.trim().toLowerCase();
  }

}

export interface Event {
  date: string;
  initialize: string;
  type: string;
  text: string;
}

const EVENTS_DATA: Event[] = [
  {
    date: '12.05.2018 10:10:10',
    initialize: 'Peter',
    type: 'Создание пользователя',
    text: 'Успешно',
  },
  {
    date: '12.08.2018 12:20:10',
    initialize: 'Henry',
    type: 'Добавление файла',
    text: 'Успешно',
  },
  {
    date: '09.10.2018 13:13:10',
    initialize: 'server1',
    type: 'Ошибка сервера',
    text: 'Нехватка памяти',
  },
];
