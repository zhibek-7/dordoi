import { Guid } from "guid-typescript";

export class FolderModel {
  public name: string;
  public projectId: string;
  public parentId: Guid;

  constructor(name: string, projectId: Guid, parentId?: Guid) {
    this.name = name;
    this.projectId = projectId + "";
    this.parentId = parentId;
  }
}
