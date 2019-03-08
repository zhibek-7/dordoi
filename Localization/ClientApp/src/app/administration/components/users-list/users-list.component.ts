import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import {
  MatSort,
  MatTableDataSource,
  MatPaginator,
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material';
//import { ApiService } from '../../../services/api.service';
import { isError } from 'util';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  //ELEMENT_DATA: Users[] = this.apiService.getAllUsers();
  displayedColumns: string[] = [
    'name',
    'role',
    'creationDate',
    'foldersWeight',
    'maxFoldersWeight',
    'condition',
    'actions'
  ];
  //dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  value = '';
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(public dialog: MatDialog,
    //private apiService: ApiService
  ) {
                //console.log(this.apiService.getAllUsers(), 555);
              }

  openDialog(userId): void {
/*    const dialogRef =  this.dialog.open(DialogOverview, {
      width: '500px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed', result);

      if (result) {
        //this.ELEMENT_DATA.map(function(item) {
        //  if (item.id === userId) {
        //    if (result.selectedStatus) {
        //      item.condition = result.selectedStatus;
        //    }
        //    if (result.selectedRate) {
        //      item.maxFoldersWeight = result.selectedRate;
        //    }
        //  }
        //});
      }
    });*/
  }

  ngOnInit() {
    //this.dataSource.sort = this.sort;
    //this.dataSource.paginator = this.paginator;
  }

  applyFilter(filterValue: string) {
    //this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
