import { Navbar } from '@/components';
import { NotificationCriteriaPage } from '@/pages';
import { FC } from 'react';

const NotificationCriteriaRoute: FC = () => {
  return (
    <>
      <Navbar />
      <NotificationCriteriaPage />
    </>
  );
};

export default NotificationCriteriaRoute;
