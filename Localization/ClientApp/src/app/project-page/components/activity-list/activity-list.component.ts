import { Component, OnInit, Input } from "@angular/core";
import { MatTableDataSource, PageEvent, Sort } from "@angular/material";

import { WorkTypeService } from "src/app/services/workType.service";
import { UserActionsService } from "src/app/services/userActions.service";

import { Locale } from "src/app/models/database-entities/locale.type";
import { User } from "src/app/models/database-entities/user.type";
import { WorkType } from "src/app/models/database-entities/workType.type";
import { UserAction } from "src/app/models/database-entities/userAction.type";
import { ProjectsService } from "src/app/services/projects.service";

@Component({
  selector: 'app-activity-list',
  templateUrl: './activity-list.component.html',
  styleUrls: ['./activity-list.component.css']
})
export class ActivityListComponent implements OnInit {

  displayedColumns: string[] = ['user_name', 'work_type_name', 'translation_substring_name', 'file_name', 'locale_name', 'datetime'];

  @Input()
  langList: Array<Locale>;

  @Input()
  userList = new Array<User>();

  workTypeList: Array<WorkType>;

  userActionsList: Array<UserAction>;

  userActionsDataSource = new MatTableDataSource(this.userActionsList);

  selectedWorkTypeId = -1;
  selectedLocaleId =  null;
  selectedUserId = null;

  userActionsTotalLength = 0;
  userActionsPageSize = 10;
  userActionsPageSizeOptions = [10, 25, 50];

  userActionsSortBy = '';
  userActionsSortAscending = true;

  constructor(
    private workTypeService: WorkTypeService,
    private userActionsService: UserActionsService,
    private projectsService: ProjectsService,
  ) { }

  ngOnInit() {
    this.loadWorkTypes();
    this.loadUserActions();
  }

  loadWorkTypes() {
    this.workTypeService.getWorkTypes().subscribe(
      workTypes => {
        this.workTypeList = workTypes;
      },
      error => console.error(error)
    );
  }

  onPageChanged(args: PageEvent) {
    this.userActionsPageSize = args.pageSize;
    const currentOffset = args.pageSize * args.pageIndex;
    this.loadUserActions(currentOffset);
  }

  loadUserActions(offset?: number) {
    if (!offset) {
      offset = 0;
    }
    this.userActionsService.getUserActionsByProjectId(
      this.projectsService.currentProjectId,
      this.selectedWorkTypeId,
      this.selectedUserId,
      this.selectedLocaleId,
      this.userActionsPageSize,
      offset,
      [this.userActionsSortBy],
      this.userActionsSortAscending
    ).subscribe(
      response => {
        this.userActionsTotalLength = +response.headers.get("totalCount");
        this.userActionsList = response.body;
        this.userActionsDataSource = new MatTableDataSource(this.userActionsList);
      },
      error => console.error(error)
    );
  }

  onSortingChanged(args: Sort) {
    if (args.direction === '') {
      this.userActionsSortBy = null;
      this.userActionsSortAscending = true;
    }
    else {
      this.userActionsSortBy = args.active;
      this.userActionsSortAscending = (args.direction == 'asc');
    }
    this.loadUserActions();
  }

}
