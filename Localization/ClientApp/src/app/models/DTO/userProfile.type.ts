export class UserProfile {
  id: number;
  name: string;
  //password: string;
  photo: Blob;
  email: string;
  //joined: boolean;
  fullName: string;
  timeZone?: number;
  aboutMe: string;
  gender?: boolean;
  //localeId?: number;
  //localeIsNative?: boolean;

  localesIds: number[];
  localesIdIsNative: Array<[number, boolean]>;
}
