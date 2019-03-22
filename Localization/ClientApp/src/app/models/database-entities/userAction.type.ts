import { Guid } from 'guid-typescript';

export class UserAction {
  public id: Guid;
  public id_user: Guid;
  public user_name: string;
  public id_work_type: Guid;
  public work_type_name: string;
  public datetime: Date;
  public description: string;
  public id_locale: Guid;
  public locale_name: string;
  public id_file: Guid;
  public file_name: string;
  public id_string: Guid;
  public translation_substring_name: string;
  public id_translation: Guid;
  public translation: string;
  public id_project: Guid;
  public project_name: string;
}
