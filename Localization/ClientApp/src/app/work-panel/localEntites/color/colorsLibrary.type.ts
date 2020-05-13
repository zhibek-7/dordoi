import { Color } from "./color.type";

export class ColorsLibrary {
    
    colorsList: Array<Color>;
    
    constructor(){
        this.colorsList = new Array<Color>();
        this.colorsList.push(new Color("red","../../../../../assets/icons/main/redIcon.png"));
        this.colorsList.push(new Color("yellow","../../../../../assets/icons/main/yellowIcon.png"));
        this.colorsList.push(new Color("green","../../../../../assets/icons/main/greenIcon.jpg"));
    }
}