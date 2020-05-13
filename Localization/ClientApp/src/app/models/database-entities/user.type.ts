import { Guid } from 'guid-typescript';

export class User {
  id: Guid;
  name_text: string;
  password_text: string;
  photo: Blob;
  email: string;
  joined: boolean;
}
