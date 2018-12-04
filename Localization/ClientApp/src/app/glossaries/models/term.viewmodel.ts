import { String } from "src/app/models/database-entities/string.type";

export class TermViewModel {

  constructor(
    public term: String,
    public isSelected: boolean
  ) { }

}
