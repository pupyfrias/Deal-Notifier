
import { Navbar } from '@/components';
import { HomePage, HomeSearch } from '@/pages';
import { FC } from 'react';

const Home: FC = () => {
  return (
    <>
      <Navbar>
        <HomeSearch />
      </Navbar>
      <HomePage />
    </>
  );
};

export default Home;
