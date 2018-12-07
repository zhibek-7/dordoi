import { Component, OnInit, Output, EventEmitter, Input, OnChanges } from '@angular/core';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit, OnChanges {

  @Input() currentPage: number = 1;
  @Input() pageSize: number = 1;
  @Input() totalCount: number = 1;
  @Input() displayedPagesRange: number = 2;

  @Output() pageChanged: EventEmitter<number> = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges() {
  }

  get totalPages() {
    return Math.ceil(Math.max(this.totalCount, 1) / Math.max(this.pageSize, 1));
  }

  get pagesArray(): number[] {
    let possiblePageNumbers = new Array<number>();
    for (let i = 0; i < this.displayedPagesRange * 2 + 1; i++) {
      possiblePageNumbers.push(this.currentPage - this.displayedPagesRange + i);
    }
    while (possiblePageNumbers.some(possiblePageNumber => possiblePageNumber <= 0)) {
      possiblePageNumbers = possiblePageNumbers.map(possiblePageNumber => possiblePageNumber + 1);
    }
    while (possiblePageNumbers.some(possiblePageNumber => possiblePageNumber > this.totalPages)) {
      possiblePageNumbers = possiblePageNumbers.map(possiblePageNumber => possiblePageNumber - 1);
    }
    return possiblePageNumbers.filter(
        pageNumber => this.isValidPageNumber(pageNumber));
  }

  get pages(): Observable<number[]> {
    return of(this.pagesArray);
  }

  get firstDisplayedPage() {
    return this.pagesArray[0];
  }

  get lastDisplayedPage() {
    return this.pagesArray[this.pagesArray.length - 1];
  }

  isValidPageNumber(pageNumber: number): boolean {
    return pageNumber > 0 && pageNumber <= this.totalPages;
  }

  selectPage(pageNumber: number) {
    if (this.isValidPageNumber(pageNumber)) {
      this.pageChanged.emit(pageNumber);
    }
    else {
      console.log('ERROR: Page number is not valid!');
    }
  }

}
