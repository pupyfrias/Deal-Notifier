import { AccountCircle } from '@mui/icons-material';
import { Menu, MenuItem, IconButton } from '@mui/material';
import { FC } from 'react';

type MenuMobileProps = {
  mobileMoreAnchorEl: HTMLElement | null;
  mobileMenuId: string;
  isMobileMenuOpen: boolean;
  handleMobileMenuClose: VoidFunction;
  handleProfileMenuOpen: (event: React.MouseEvent<HTMLElement>) => void;
};

const MenuMobile: FC<MenuMobileProps> = ({ mobileMoreAnchorEl, mobileMenuId, isMobileMenuOpen, handleMobileMenuClose, handleProfileMenuOpen }) => {
  return (
    <Menu
      anchorEl={mobileMoreAnchorEl}
      anchorOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      id={mobileMenuId}
      keepMounted
      transformOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      open={isMobileMenuOpen}
      onClose={handleMobileMenuClose}
    >
      <MenuItem onClick={handleProfileMenuOpen}>
        <IconButton
          size='large'
          aria-label='account of current user'
          aria-controls='primary-search-account-menu'
          aria-haspopup='true'
          color='inherit'
        >
          <AccountCircle />
        </IconButton>
        <p>Profile</p>
      </MenuItem>
    </Menu>
  );
};

export default MenuMobile;
