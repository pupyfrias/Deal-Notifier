import { AppStore } from '@/models';
import { favoriteSlice, peopleSlice } from './states';
import { configureStore } from '@reduxjs/toolkit';

const store = configureStore<AppStore>({
  reducer: {
    people: peopleSlice.reducer,
    favorite: favoriteSlice.reducer,
  },
});

export default store;
