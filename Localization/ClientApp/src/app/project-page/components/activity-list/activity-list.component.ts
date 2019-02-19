import { Component, OnInit, Input } from "@angular/core";
import { MatTableDataSource } from "@angular/material";

import { WorkTypeService } from "src/app/services/workType.service";
import { UserActionsService } from "src/app/services/userActions.service";

import { Locale } from "src/app/models/database-entities/locale.type";
import { User } from "src/app/models/database-entities/user.type";
import { WorkType } from "src/app/models/database-entities/workType.type";
import { UserAction } from "src/app/models/database-entities/userAction.type";

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

  selectedWorkType = -1;
  selectedLang = -1;
  selectedUser = -1;

  constructor(
    private workTypeService: WorkTypeService,
    private userActionsService: UserActionsService,
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

  loadUserActions() {
    this.userActionsService.getActionsList().subscribe(
      actions => {
        this.userActionsList = actions;
        this.userActionsDataSource = new MatTableDataSource(this.userActionsList);
      },
      error => console.error(error)
    );
  }

  chageAndApplyUserActionsFilter() {
    this.userActionsDataSource.filterPredicate = (userAction: UserAction) => {
      let matchFilters = true;
      if (this.selectedLang != -1) {
        matchFilters = matchFilters && userAction.id_locale == this.selectedLang;
      }
      if (this.selectedUser != -1) {
        matchFilters = matchFilters && userAction.id_user == this.selectedUser;
      }
      if (this.selectedWorkType != -1) {
        matchFilters = matchFilters && userAction.id_work_type == this.selectedWorkType;
      }
      return matchFilters;
    };
    this.applyUserActionsFiltering();
  }

  applyUserActionsFiltering() {
    this.userActionsDataSource.filter = '1';
  }

}
