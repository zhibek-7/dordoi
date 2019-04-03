import { Guid } from "guid-typescript";

export class Invitation {
  constructor(
    public id: string,
    public id_project: string,
    public id_role: Guid,
    public email: string,
    public message: string
  ) {}
}
