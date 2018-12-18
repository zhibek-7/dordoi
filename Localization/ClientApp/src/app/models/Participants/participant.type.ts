export class Participant {
  constructor(
    public id_LocalizationProject?: number,
    public id_User?: number,
    public id_Role?: number,
    public active?: boolean,
    public userName?: string,
    public roleName?: string,
  ) { }
}
