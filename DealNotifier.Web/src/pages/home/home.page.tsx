/* eslint-disable react-hooks/exhaustive-deps */
import { HomePagination } from '@/components';
import { Item as ItemModel } from '@/models';
import { getItems } from '@/services';
import { FormControl, Grid, MenuItem, Select, SelectChangeEvent, Typography } from '@mui/material';
import { FC, useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import { HomeMain, Sidebar } from '.';
import { useFirstRender } from '@/hooks';

const HomePage: FC = () => {
  const [items, setItems] = useState<ItemModel[]>([]);
  const [limit, setLimit] = useState<number>(30);
  const [totalItem, setTotalItem] = useState<number>(0);
  const [count, setCount] = useState<number>(0);
  const [queryParams, setQueryParams] = useState<URLSearchParams>(new URLSearchParams());
  const location = useLocation();
  const limits = [30, 60, 120];
  const isFirstRender = useFirstRender();

  useEffect(() => {
    if (!isFirstRender) {
      updateQueryParams();
      console.log('location.search changed');
    }
  }, [location.search]);

  useEffect(() => {
    const pageString = queryParams.get('page');

    if (pageString) {
      const page = Number.parseInt(pageString, 10);
      const offset = (limit * page).toString();
      queryParams.set('offset', offset);
    }

    queryParams.set('limit', limit.toString());

    getItems(queryParams).then((response) => {
      const data = response.data;
      const totalPage = Math.floor(data.total / data.limit);
      setItems(data.items);
      setCount(totalPage);
      setTotalItem(data.total);
      console.log('queryParams or limit changed');
    });

   
  }, [queryParams, limit]);

  const updateQueryParams = ()=>{
    const query = new URLSearchParams(location.search);
    setQueryParams(query);
  }

  function handleChange(event: SelectChangeEvent<number>): void {
    const value = event.target.value as number;
    queryParams.delete('page');
    queryParams.delete('offset');
    queryParams.delete('limit');
    const route = queryParams.size > 0 ? `?${queryParams.toString()}` : '/';
    window.history.pushState({}, '', route);
    setLimit(value);

    console.log('limit changed', route);
  }

  return (
    <>
      <Grid
        container
        spacing={2}
        sx={{ m: 0, width: '100%' }}
      >
        <Grid
          item
          md={3}
        >
          <Sidebar />
        </Grid>
        <Grid
          item
          md={9}
        >
          {totalItem > 0 && <Typography>{totalItem.toLocaleString()} Items </Typography>}
          <HomeMain items={items} updateQueryParams= {updateQueryParams}/>
        </Grid>
        {count > 0 && (
          <Grid
            item
            md={12}
            sx={{ display: 'flex' }}
          >
            <FormControl sx={{ m: 1 }}>
              <Select
                defaultValue={30}
                onChange={handleChange}
              >
                {limits.map((i) => {
                  return (
                    <MenuItem
                      key={i}
                      value={i}
                    >
                      {i}
                    </MenuItem>
                  );
                })}
              </Select>
            </FormControl>
            <HomePagination count={count} />
          </Grid>
        )}
      </Grid>
    </>
  );
};

export default HomePage;
