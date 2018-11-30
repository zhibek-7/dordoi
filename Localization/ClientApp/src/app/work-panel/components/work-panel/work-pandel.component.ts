import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'work-panel-component',
    templateUrl: './work-panel.component.html',
    styleUrls: ['./work-panel.component.css']
})
export class WorkPanelComponent implements OnInit {

    showLeftBlock: boolean = true;
    showRightBlock: boolean = true;

    left3: boolean = true;
    left0: boolean = false;
    middleCol6: boolean = true;
    middleCol9: boolean = false;
    middleCol12: boolean = false;
    right3: boolean = true;
    right0: boolean = false;
    middleAndRight: boolean = false;  
    
    constructor() { }

    ngOnInit(): void { }

    hideLeftBlockFunction(showLeftBlock: boolean){
        this.showLeftBlock = showLeftBlock;

        if(this.showLeftBlock){
            this.left3 = true;
            this.left0 = false;
            if(this.showRightBlock == false){
                this.middleCol9 = true;
                this.middleCol6 = false;
            } else {
                this.middleCol9 = false;
                this.middleCol6 = true;
            }
            this.middleCol12 = false;
            this.middleAndRight = false;
        }
        else {
            this.left3 = false;
            this.left0 = true;
            if(!this.showRightBlock == false){
                this.middleCol9 = true;
                this.middleCol6 = false;
            } else {
                this.middleCol9 = false;
                this.middleCol6 = true;
            }
            this.middleCol12 = false;
            this.middleAndRight = true;
        }

        if(!this.showLeftBlock && !this.showRightBlock) {
            this.middleCol9 = false;
            this.middleCol6 = false;
            this.middleCol12 = true;
        }
    }

    hideRightBlockFunction(showRightBlock: boolean){
        this.showRightBlock = showRightBlock;

        if(this.showRightBlock){
        this.right3 = true;
        this.right0 = false;
        if(this.showLeftBlock == false){
            this.middleCol9 = true;
            this.middleCol6 = false;
        } else {
            this.middleCol9 = false;
            this.middleCol6 = true;
        }
        this.middleCol12 = false;
        }
        else {
        this.right3 = false;
        this.right0 = true;
        if(!this.showLeftBlock == false){
            this.middleCol9 = true;
            this.middleCol6 = false;
        } else {
            this.middleCol9 = false;
            this.middleCol6 = true;
        }
        this.middleCol12 = false;
        }

        if(!this.showLeftBlock && !this.showRightBlock) {
        this.middleCol9 = false;
        this.middleCol6 = false;
        this.middleCol12 = true;
        }
    }

}
