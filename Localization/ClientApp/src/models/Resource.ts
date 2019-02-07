// Модель данных
export class Resource {
  constructor(
    public id: number,
    public parentId: number,
    public name: string,
    public size: number,
    public created: Date,
    public updated: Date,
    public extension: string,
    public type: string,
    public folder: boolean,
    public outerAccess: boolean,
    public deleted: boolean,
    public favorite: boolean) {
  }

  public clone(): Resource {
    return new Resource(this.id,
      this.parentId,
      this.name,
      this.size,
      this.created,
      this.updated,
      this.extension,
      this.type,
      this.folder,
      this.outerAccess,
      this.deleted,
      this.favorite);
  }

}

