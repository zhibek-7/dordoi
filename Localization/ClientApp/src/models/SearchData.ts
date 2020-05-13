// Модель данных для расширенного поиска
export interface SearchData {
  types: string[];
  inTrash;
  inLabel;
  startDate: Date;
  endDate: Date;
  name: string;
}
