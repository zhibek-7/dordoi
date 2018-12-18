import { Selectable } from "src/app/shared/models/selectable.model";
import { Term } from "src/app/models/Glossaries/term.type";

export class TermViewModel extends Selectable<Term> {

  constructor(
    public term: Term,
    isSelected: boolean
  ) {
      super(term, isSelected);
  }

}
