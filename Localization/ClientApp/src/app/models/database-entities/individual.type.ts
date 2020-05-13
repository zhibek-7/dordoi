import { Guid } from "guid-typescript";
export class individual {
  public id: Guid;
  public ID_User: string;

  public name_text_first: string;
  public description_first: string;

  public name_text_second: string;
  public description_second: string;

  public name_text_third: string;
  public description_third: string;

  public name_text_fourth: string;
  public description_fourth: string;

  public name_text_fifth: string;
  public description_fifth: string;



  public date_time_added: Date;


  public constructor(name_text_first: string, description_first: string,
    name_text_second: string, description_second: string,
    name_text_third: string, description_third: string,
    name_text_fourth: string, description_fourth: string,
    name_text_fifth: string, description_fifth: string
  ) {
    this.ID_User = null; //Guid.createEmpty()
    this.name_text_first = name_text_first;
    this.description_first = description_first;

    this.name_text_second = name_text_second;
    this.description_second = description_second;

    this.name_text_third = name_text_third;
    this.description_third = description_third;

    this.name_text_fourth = name_text_fourth;
    this.description_fourth = description_fourth;

    this.name_text_fifth = name_text_fifth;
    this.description_fifth = description_fifth;

    this.date_time_added = null;
  }
}
