export enum fileType {
  csv = 0,
  tmx = 1
}

export class settingFileLoad {
  typeFile: number;
  isAllLocale: boolean;
  localesIds: number[];
  baseLocaleId: number;
  translationLocaleId: number;
}
