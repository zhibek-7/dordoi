export class File {
  id: number;
  name_text: string;
  description: string;
  date_of_change: Date;
  strings_count: number;
  version: number;
  priority: number;
  path: string;
  output_name: string;
  is_folder: boolean;
  folder: string;
  id_localization_project: number;
  id_folder_owner: number;
  is_last_version: boolean;
  id_previous_version: number;
  translator_name: string;
  download_name: string;
  percent_of_translation?: number;
  percent_of_confirmed?: number;
}
