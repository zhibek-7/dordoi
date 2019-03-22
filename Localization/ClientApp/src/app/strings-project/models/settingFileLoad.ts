import { Guid } from 'guid-typescript';

export enum fileType {
  csv = 0,
  tmx = 1
}

export class settingFileLoad {
  typeFile: number;
  isAllLocale: boolean;
  localesIds: Guid[];
  baseLocaleId: Guid;
  translationLocaleId: Guid;
}
