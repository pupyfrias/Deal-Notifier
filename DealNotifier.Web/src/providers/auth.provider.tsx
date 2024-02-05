import { AuthContext } from '@/contexts';
import { LoginResponse } from '@/models';
import { FC, ReactNode, useEffect, useState } from 'react';

type AuthProviderProps = {
  children: ReactNode;
};

const AuthProvider: FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<null | LoginResponse>(null);

  useEffect(() => {
    const storedUserData = localStorage.getItem('user');
    if (storedUserData) {
      setUser(JSON.parse(storedUserData));
    }
  }, []);

  const setLogin = (userData: LoginResponse) => {
    setUser(userData);
    localStorage.setItem('user', JSON.stringify(userData));
  };

  const logout = () => {
    setUser(null);
    localStorage.removeItem('user');
  };

  return <AuthContext.Provider value={{ user, setLogin: setLogin, logout }}>{children}</AuthContext.Provider>;
};

export default AuthProvider;
