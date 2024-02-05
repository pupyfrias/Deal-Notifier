/* eslint-disable react-hooks/exhaustive-deps */
import { People } from '@/data';
import { addPeople } from '@/redux/states';
import { FC, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { PeopleTable } from '@/pages/home/components';



export interface HomeInterface {}

const Home: FC<HomeInterface> = () => {

  const dispatch = useDispatch();
  
  useEffect(() => {
    dispatch(addPeople(People));
  }, []);

  return (
    <PeopleTable/>
  );
};

export default Home;
