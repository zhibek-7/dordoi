import { Guid } from 'guid-typescript';

export class Image {
  public constructor(
    public body?: any,
    public id?: Guid,
    public name_text?: string,
    public date_Time_Added?: Date,
    public iD_User?: Guid,
    public url?: string
  ) { }
}
