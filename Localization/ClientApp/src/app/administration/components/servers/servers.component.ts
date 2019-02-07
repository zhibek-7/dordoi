import { Component, OnInit, ViewChild, NgZone, Inject } from '@angular/core';
import {
  MatSort,
  MatTableDataSource,
  MatPaginator,
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material';

@Component({
  selector: 'app-servers',
  templateUrl: './servers.component.html',
  styleUrls: ['./servers.component.css']
})
export class ServersComponent implements OnInit {
  displayedColumns: string[] = [
    'name',
    'role',
    'status',
    'address',
    'actions'
  ];
  dataSource = new MatTableDataSource(SERVERS_DATA);
  value = '';
  selectedServerId = '';
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    public dialog: MatDialog,
    private zone: NgZone) {
    this.zone.runOutsideAngular(() => {
      document.addEventListener('contextmenu', (e: MouseEvent) => {
        e.preventDefault();
      });
    });
  }

  ngOnInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  changeStatus(curretStatus) {
    const id = this.selectedServerId;
    SERVERS_DATA.map(function(item) {
      if (item.id === id) {
          item.status = curretStatus;
      }
    });
  }

  openDialog(server): void {
    const dialogRef = this.dialog.open(ServerSettingsDialogComponent, {
      width: '700px',
      data: server
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed', result);
    });
  }
}

@Component({
  selector: 'app-server-settings-dialog',
  templateUrl: 'server-settings.html',
  styleUrls: ['./servers.component.css']
})
export class ServerSettingsDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ServerSettingsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Servers
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}

export interface DialogData {
  address: string;
  selectedStatus: string;
  selectedAuthMethod: string;
}

export interface Servers {
  id: string;
  name: string;
  role: string;
  status: string;
  address: string;
  port: string;
  minSpace: string;
  maxSpace: string;
}

const SERVERS_DATA: Servers[] = [
  {
    id: '123',
    name: 'Admin1',
    role: 'Сервер администрирования',
    status: 'Работает',
    address: 'http://localhost/',
    port: '3310',
    minSpace: '3Гб',
    maxSpace: '10Гб',
  },
  {
    id: '456',
    name: 'App1',
    role: 'Сервер приложений',
    status: 'Работает',
    address: '',
    port: '',
    minSpace: '3Гб',
    maxSpace: '10Гб',
  },
  {
    id: '789',
    name: 'App2',
    role: 'Сервер приложений',
    status: 'Работает',
    address: '',
    port: '',
    minSpace: '3Гб',
    maxSpace: '10Гб',
  },
  {
    id: '089',
    name: 'Client1',
    role: 'Пользовательский',
    status: 'Работает',
    address: '',
    port: '',
    minSpace: '3Гб',
    maxSpace: '10Гб',
  },
  {
    id: '129',
    name: 'Client2',
    role: 'Пользовательский',
    status: 'Остановлен',
    address: '',
    port: '',
    minSpace: '3Гб',
    maxSpace: '10Гб',
  },
  {
    id: '156',
    name: 'Scheduler',
    role: 'Сервер заданий',
    status: 'Остановлен',
    address: '',
    port: '',
    minSpace: '3Гб',
    maxSpace: '10Гб',
  },
  {
    id: '895',
    name: 'База данных',
    role: 'Сервер базы данных',
    status: 'Остановлен',
    address: '',
    port: '',
    minSpace: '3Гб',
    maxSpace: '10Гб',
  },
];
