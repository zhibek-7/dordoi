import { Guid } from 'guid-typescript';

export class UserProfile {
  id: Guid;
  /**
   * Имя пользователя (логин/ник)
   */
  name_text: string;

  photo: any;

  email: string;

  //joined: boolean;

  /**
   * ФИО
   */
  full_name: string;

  id_time_zones?: number;

  about_me: string;

  gender?: boolean;

  /**
   * Выбранные языки перевода (item1: идентификатор языка перевода, item2: язык перевода является родным)
   */
  locales_id_is_native: Array<{ item1: Guid; item2: boolean }>;
}
