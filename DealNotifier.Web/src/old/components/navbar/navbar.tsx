import { AppBar, Button, IconButton, Toolbar, Typography } from '@mui/material';
import React from 'react';

export interface NavbarInterface {}

const Navbar: React.FC<NavbarInterface> = () => {
  return (
    <nav>
      <AppBar position='static'>
        <Toolbar>
        
          <Typography
            variant='h6'
            component='div'
            sx={{ flexGrow: 1 }}
          >
            News
          </Typography>
          
        </Toolbar>
      </AppBar>
    </nav>
  );
};

export default Navbar;
