export class Participant {
  constructor(
    public localizationProjectId?: number,
    public userId?: number,
    public roleId?: number,
    public active?: boolean,
    public userName?: string,
    public roleName?: string,
  ) { }
}
