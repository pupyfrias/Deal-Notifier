import Pagination, { PaginationRenderItemParams } from '@mui/material/Pagination';
import PaginationItem from '@mui/material/PaginationItem';
import { FC } from 'react';
import { Link, useLocation } from 'react-router-dom';

type HomePaginationProps = {
  count: number;
};

const HomePagination: FC<HomePaginationProps> = ({count}) => {
  const location = useLocation();
  const query = new URLSearchParams(location.search);
  const page = parseInt(query.get('page') || '1', 10);

  const renderItem = (item: PaginationRenderItemParams) => {
    if (item.page && item.page > 1) {
      query.set('page', item.page.toString());
    } else {
      query.delete('page');
    }

    return (
      <PaginationItem
        component={Link}
        to={`${query.size === 0 ? '' : `?${query}`}`}
        {...item}
      />
    );
  };

  return (
    <Pagination
      sx={{ margin: 'auto' }}
      page={page}
      count={count}
      renderItem={renderItem}
    />
  );
};

export default HomePagination;
