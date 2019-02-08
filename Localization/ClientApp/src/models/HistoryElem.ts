/**
 * Класс события с файлом
 */
export class HistoryElem {
  constructor(
    public id: number,
    public date: Date,
    public operation: string) {
  }

}
