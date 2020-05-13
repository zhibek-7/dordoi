import { Guid } from 'guid-typescript';

export class StringEdit {
  id: Guid;
  value: string;
  description: string;
  part_Of_Sentence: string;
  context: string;
  context_Max_Length: number;
}
