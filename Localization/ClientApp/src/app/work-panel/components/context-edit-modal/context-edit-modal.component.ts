import { Component, OnInit, Output, EventEmitter } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

@Component({
    selector: 'context-edit-modal-component',
    templateUrl: './context-edit-modal.component.html',
    styleUrls: ['./context-edit-modal.component.css']
})
export class ContextEditModalComponent extends ModalComponent implements OnInit {

    enteredContext: string = null;

    @Output() onEnterContext = new EventEmitter<string>();
    
    constructor() {
        super();
     }

    ngOnInit(): void {

    }

    confirmContext(){
        this.onEnterContext.emit(this.enteredContext);
        super.hide();
    }

    show(){
        super.show();

        this.enteredContext = null
    }
}
