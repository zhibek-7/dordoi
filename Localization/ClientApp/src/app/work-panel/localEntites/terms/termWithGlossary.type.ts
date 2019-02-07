import { Term } from "src/app/models/Glossaries/term.type";
import { Glossary } from "src/app/models/database-entities/glossary.type";

export class TermWithGlossary {
  get id(): number {
    return this.iD;
  }

  public constructor(
    public iD?: number,
    public substring_to_translate?: string,
    public description?: string,
    public context?: string,
    public iD_File_Owner?: number,
    public translation_Max_Length?: number,
    public value?: string,
    public position_In_Text?: number,
    public part_Of_Speech_Id?: number,
    public glossary_Id?: number,
    public glossary_Name?: string,
    public glossary_Description?: string,
    public term?: Term,
    public glossary?: Glossary
  ) {}
}
