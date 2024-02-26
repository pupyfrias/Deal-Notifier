import { Person } from "./person";

export interface AppStore {
    people: Person[];
    favorite: Person[];
  }
  