import { Guid } from "guid-typescript";
export class fund {
  public id: Guid;
  public ID_User: string;
  public Fund_text: string;
  public Fund_description: string;
  public DateTime: Date;


   public constructor(Fund_text: string, Fund_description: string) {
    this.ID_User = null; //Guid.createEmpty()
    this.Fund_text = Fund_text;
    this.Fund_description = Fund_description;
    this.DateTime = null;
  }
}
