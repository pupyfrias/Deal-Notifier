import { Navbar } from '@/components';
import { LoginPage } from '@/pages';
import { FC } from 'react';

const LoginRoute: FC = () => {
  return (
    <>
      <Navbar />
      <LoginPage />
    </>
  );
};

export default LoginRoute;
