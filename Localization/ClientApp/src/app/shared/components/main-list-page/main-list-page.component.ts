import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-main-list-page',
  templateUrl: './main-list-page.component.html',
  styleUrls: ['./main-list-page.component.css']
})
export class MainListPageComponent implements OnInit {

  @Output()
  changed = new EventEmitter();

  protected dataSource: any[];

  private _pageSize: number = 10;
  set pageSize(pageSize: number) {
    this._pageSize = pageSize;
  }
  get pageSize(): number {
    return this._pageSize;
  }
  
  private _currentOffset: number = 0;
  set currentOffset(currentOffset: number) {
    this._currentOffset = currentOffset;
  }
  get currentOffset(): number {
    return this._currentOffset;
  }

  private _totalCount: number = 0;
  set totalCount(totalCount: number) {
    this._totalCount = totalCount;
  }
  get totalCount(): number {
    return this._totalCount;
  }

  private _sortByColumnName: string = null;
  set sortByColumnName(columnName: string) {
    this._sortByColumnName = columnName;
  }
  get sortByColumnName(): string {
    return this._sortByColumnName;
  }
  
  private _searchString: string = "";
  set searchString(string: string) {
    this._searchString = string;
  }
  get searchString(): string {
    return this._searchString;
  }
  
  private _lastSortColumnName: string = '';
  set lastSortColumnName(columnName: string) {
    this._lastSortColumnName = columnName;
  }
  get lastSortColumnName(): string {
    return this._lastSortColumnName;
  }
  
  private _isSortingAscending: boolean = true;
  set isSortingAscending(isSortingAscending: boolean) {
    this._isSortingAscending = isSortingAscending;
  }
  get isSortingAscending(): boolean {
    return this._isSortingAscending;
  }
  
  private _filtersViewModels: any[];// = [];
  set filters(items: any) {
    this._filtersViewModels = items;
  }
  get filters(): any {
    return this._filtersViewModels;
  }
  private _selectedFilterId: Guid | null = null;
  set filterId(id: Guid) {
    this._selectedFilterId = id;
  }
  get filterId(): Guid {
    return this._selectedFilterId;
  }


  constructor() { }

  ngOnInit() {
    //this.loadDropdown();
  }

  loadDropdown() {
    //this.loadData();
  }
  
  loadData(offset = 0) {
  }

  reloadData() {
    this.currentOffset = 0;
    this.changed.emit();
  }

  requestSortBy(columnName: string) {
    if (columnName != this.lastSortColumnName) {
      this.isSortingAscending = true;
    } else {
      this.isSortingAscending = !this.isSortingAscending;
    }

    this.sortByColumnName = columnName;
    this.loadData();

    this.lastSortColumnName = columnName;
  }

  onPageChanged(newOffset: number) {
    //this.loadData(newOffset);
    this.currentOffset = newOffset;
    this.changed.emit();
  }
  
}
