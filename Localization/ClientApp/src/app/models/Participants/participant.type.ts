export class Participant {
  constructor(
    public localization_Project_Id?: number,
    public user_Id?: number,
    public role_Id?: number,
    public active?: boolean,
    public user_Name?: string,
    public role_Name?: string,
    public role_Short?: string
  ) { }
}
