import { Guid } from "guid-typescript";

export class Participant {
  constructor(
    public localization_Project_Id?: Guid,
    public user_Id?: Guid,
    public role_Id?: Guid,
    public active?: boolean,
    public user_Name?: string,
    public role_Name?: string,
    public role_Short?: string
  ) {}
}
