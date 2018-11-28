// Модель проектов
export interface Project {
    name: string;
    id: string;
    url: string;
    owner: string;
    created: any;
    changed: any;
    hasErr: boolean;
    type: string;
    sourceLanguage: string;
  }
  