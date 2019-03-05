export class Invitation {
  constructor(
    public id: string,
    public id_project: number,
    public id_role: number,
    public email: string,
    public message: string,
  ) { }
}
