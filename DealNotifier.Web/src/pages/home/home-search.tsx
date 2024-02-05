/* eslint-disable react-hooks/exhaustive-deps */
import { Search, SearchIconWrapper, StyledInputBase } from '@/components';
import { useDebounce, useFirstRender } from '@/hooks';
import { FC, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import SearchIcon from '@mui/icons-material/Search';

const HomeSearch: FC = () => {
  const isFirstRender = useFirstRender();

  const [search, setSearch] = useState<string>('');
  const debouncedSearchTerm = useDebounce(search, 500);
  const navigate = useNavigate();

  useEffect(() => {
    if (!isFirstRender) {
      let route = '/';
      if (debouncedSearchTerm) {
        route = `?search=${debouncedSearchTerm}`;
      }
      navigate(route);
      console.log('search');
    }
  }, [debouncedSearchTerm]);

  const handleKeyUp = (event: React.KeyboardEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const target = event.target as HTMLInputElement;
    const value = target.value;
    setSearch(value);
  };

  return (
    <Search>
      <SearchIconWrapper>
        <SearchIcon />
      </SearchIconWrapper>
      <StyledInputBase
        placeholder='Search…'
        inputProps={{ 'aria-label': 'search' }}
        onKeyUp={(event) => handleKeyUp(event)}
      />
    </Search>
  );
};

export default HomeSearch;
