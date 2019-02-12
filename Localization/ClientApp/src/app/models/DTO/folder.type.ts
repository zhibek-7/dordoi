export class FolderModel {
  constructor(
    public Name_text: string,
    public Project_Id: number,
    public Parent_Id: number | null,
  ) { }
}
