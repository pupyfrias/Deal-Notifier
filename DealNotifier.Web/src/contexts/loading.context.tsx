/* eslint-disable @typescript-eslint/no-unused-vars */

import { createContext } from 'react';

const LoadingContext = createContext({
  isLoading: false,

  setIsLoading: (_loading: boolean) => {},
});

export default LoadingContext;


