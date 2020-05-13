export class User {
  constructor(
    public id?: number,
    public name?: string,
    public password?: string,
    public photo?: ImageBitmap,
    public email?: string,
    public joined?: boolean
  ) { }
}
