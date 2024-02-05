import { LoginResponse } from '@/models';
import { createContext } from 'react';

type AuthContextProps = {
  user: null | LoginResponse;
  setLogin: (userData: LoginResponse) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextProps>(null!);

export default AuthContext;
