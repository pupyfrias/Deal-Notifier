import { Navbar } from '@/components';
import ItemPage from '@/pages/item/item.page';
import { FC } from 'react';

const ItemRoute: FC = () => {
  return (
    <>
      <Navbar />
      <ItemPage />
    </>
  );
};

export default ItemRoute;
