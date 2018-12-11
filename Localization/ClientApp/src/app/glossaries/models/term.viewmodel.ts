import { String } from "src/app/models/database-entities/string.type";
import { Selectable } from "src/app/glossaries/models/selectable.model";

export class TermViewModel extends Selectable<String> {

  constructor(
    public term: String,
    isSelected: boolean
  ) {
      super(term, isSelected);
  }

}
