import { LocalStorageType } from '@/enums';
import { Person } from '@/models';
import { getLocalStorage, setLocalStorage } from '@/utilities';
import { createSlice } from '@reduxjs/toolkit';

const initialState: Person[] = [];

export const favoriteSlice = createSlice({
  name: LocalStorageType.FAVORITES,
  initialState: getLocalStorage(LocalStorageType.FAVORITES) ? JSON.parse(getLocalStorage(LocalStorageType.FAVORITES) as string) : initialState,
  reducers: {
    addFavorites: (state, action) => {
      setLocalStorage(LocalStorageType.FAVORITES, action.payload);
      return action.payload;
    },
  },
});

export const { addFavorites } = favoriteSlice.actions;
