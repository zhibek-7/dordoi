import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnInit {

  private _visible: boolean = false;

  @Input()
  get visible() { return this._visible }

  set visible(value: boolean) {
    this._visible = value;
    this.visibleChange.emit(this._visible);
  }

  @Output()
  visibleChange = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit() {
  }

  show() {
    this.visible = true;
  }

  hide() {
    this.visible = false;
  }

  public onContainerClicked(event: MouseEvent): void {
    if ((<HTMLElement>event.target).classList.contains('modal')) {
      this.hide();
    }
  }

}
