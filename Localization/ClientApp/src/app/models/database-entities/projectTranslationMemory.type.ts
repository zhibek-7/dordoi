import { Guid } from 'guid-typescript';

export class ProjectTranslationMemory {
  public constructor(
    public project_id: Guid,
    public project_name_text: string,

    public translationMemory_id: Guid,
    public translationMemory_name_text: string
  ) { }
}
