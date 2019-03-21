import { Term } from "src/app/models/Glossaries/term.type";
import { Glossary } from "src/app/models/database-entities/glossary.type";
import { Guid } from 'guid-typescript';
export class TermWithGlossary {
  /*
  get id(): number {
    return this.id;
  }
*/
  public constructor(
    public id?: Guid,
    public substring_to_translate?: string,
    public description?: string,
    public context?: string,
    public id_file_owner?: Guid,
    public translation_max_length?: number,
    public value?: string,
    public position_in_text?: number,
    public part_of_speech_id?: Guid,
    public glossary_id?: Guid,
    public glossary_name?: string,
    public glossary_description?: string,
    public term?: Term,
    public glossary?: Glossary
  ) {}
}
