import { Component, OnInit } from "@angular/core";
import { Participant } from "src/app/models/Participants/participant.type";
import { ParticipantsService } from "src/app/services/participants.service";
import { Role } from "src/app/models/database-entities/role.type";
import { Locale } from "src/app/models/database-entities/locale.type";
import { LanguageService } from "src/app/services/languages.service";
import { Selectable } from "src/app/shared/models/selectable.model";
import { RolesService } from "src/app/services/roles.service";
import { Guid } from "guid-typescript";
import { ProjectsService } from "src/app//services/projects.service";

@Component({
  selector: "app-participants-list",
  templateUrl: "./participants-list.component.html",
  styleUrls: ["./participants-list.component.css"]
})
export class ParticipantsListComponent implements OnInit {
  selectedRoleId: Guid | null = null;
  localeIds: Guid[] = [];
  search: string = "";
  sortBy: string[] = [];
  lastSortColumnName: string = "";
  isSortingAscending: boolean = true;
  participants: Participant[] = [];
  roles: Role[] = [];
  locales: Selectable<Locale>[] = [];
  totalParticipantsCount: number = 0;
  currentOffset: number = 0;
  pageSize: number = 10;

  projectId: Guid;

  constructor(
    private projectService: ProjectsService,
    private participantsService: ParticipantsService,
    private localesService: LanguageService,
    private rolesService: RolesService
  ) {}

  ngOnInit() {
    this.loadParticipants();
    this.loadLocales();
    this.loadRoles();
  }

  loadParticipants(offset = 0) {
    let roleIds = new Array<Guid>();
    if (this.selectedRoleId != null) {
      roleIds.push(this.selectedRoleId);
    }
    this.projectId = this.projectService.currentProjectId;
    this.participantsService
      .getParticipantsByProjectId(
        this.projectService.currentProjectId,
        this.search,
        roleIds,
        this.localeIds,
        this.pageSize,
        this.currentOffset,
        this.sortBy,
        this.isSortingAscending,
        null
      )
      .subscribe(
        response => {
          this.totalParticipantsCount = +response.headers.get("totalCount");
          this.participants = response.body;
          this.currentOffset = offset;
        },
        error => console.log(error)
      );
  }

  loadLocales() {
    this.localesService
      .getLanguageList()
      .subscribe(
        locales =>
          (this.locales = locales.map(
            locale => new Selectable<Locale>(locale, false)
          )),
        error => console.log(error)
      );
  }

  loadRoles() {
    this.rolesService.getAllRoles().subscribe(
      roles => {
        this.roles = roles;
        console.log(roles);
      },
      error => console.log(error)
    );
  }

  onPageChanged(newOffset: number) {
    this.loadParticipants(newOffset);
  }

  setSelectedLocales(locales: Locale[]) {
    this.localeIds = locales.map(locale => locale.id);
    this.loadParticipants();
  }

  sortByColumn(columnName: string) {
    if (columnName != this.lastSortColumnName) {
      this.isSortingAscending = true;
    }
    this.lastSortColumnName = columnName;

    if (columnName) {
      this.sortBy = [columnName];
    }

    this.loadParticipants();

    this.isSortingAscending = !this.isSortingAscending;
  }
}
