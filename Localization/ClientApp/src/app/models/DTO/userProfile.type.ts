export class UserProfile {
  name_text: string;
  photo: Blob;
  email: string;
  //joined: boolean;
  full_name: string;
  id_time_zones?: number;
  about_me: string;
  gender?: boolean;

  locales_ids: number[];
  locales_id_is_native: Array<[number, boolean]>;
}
