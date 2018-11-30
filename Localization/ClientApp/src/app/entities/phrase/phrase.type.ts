import { Color } from "./color.type";

export class Phrase {

    id: number;
    color: Color;
    text: string;

    constructor(id: number, text: string, color:Color){
        this.id = id;
        this.text = text;
        this.color = color;
    }

}