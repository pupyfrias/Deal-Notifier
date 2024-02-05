import { LocalStorageType } from '@/enums';
import { Person } from '@/models';
import { getLocalStorage, setLocalStorage } from '@/utilities';
import { createSlice } from '@reduxjs/toolkit';

const initialState: Person[] = [];

export const peopleSlice = createSlice({
  name: LocalStorageType.PEOPLE,
  initialState: getLocalStorage(LocalStorageType.PEOPLE) ? JSON.parse(getLocalStorage(LocalStorageType.PEOPLE) as string) : initialState,
  reducers: {
    addPeople: (state, action) => {
      setLocalStorage(LocalStorageType.PEOPLE, JSON.stringify(state));
      return action.payload;
    }
  },
});

export const {addPeople} = peopleSlice.actions;