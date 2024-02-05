import { useAuth, useToast } from '@/hooks';
import { login } from '@/services';
import { AccountCircle, Visibility, VisibilityOff } from '@mui/icons-material';
import { Box, Button, FormControl, FormHelperText, IconButton, InputAdornment, InputLabel, OutlinedInput } from '@mui/material';
import { ChangeEvent, FC, FormEvent, MouseEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const LoginPage: FC = () => {
  const [showPassword, setShowPassword] = useState(false);
  const [formValues, setFormValues] = useState({
    userName: '',
    password: '',
  });

  const [formErrors, setFormErrors] = useState({
    userName: '',
    password: '',
  });

  const navigate = useNavigate();
  const { showToast } = useToast();
  const { setLogin } = useAuth();

  const handleClickShowPassword = () => setShowPassword((show) => !show);

  const handleMouseDownPassword = (event: MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
  };

  const validateUserName = (userName: string) => {
    return userName.length > 3;
  };

  const validatePassword = (password: string) => {
    return password.length >= 6;
  };

  const hasFormError = Object.values(formErrors).some((error) => error !== '');
  const isFormEmpty = Object.values(formValues).some((prop) => prop === '');

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault();

    login(formValues).then((response) => {
      setLogin(response.data);
      showToast('success operation');
      navigate('/');
    });
  };

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const target = event.target as HTMLInputElement;
    const { id, value } = target;
    setFormValues({ ...formValues, [id]: value });

    switch (id) {
      case 'userName':
        setFormErrors({ ...formErrors, userName: validateUserName(value) ? '' : 'Invalid User' });
        break;
      case 'password':
        setFormErrors({ ...formErrors, password: validatePassword(value) ? '' : 'The password must be at least 6 characters' });
        break;
      default:
        break;
    }
  };

  return (
    <>
      <Box
        component='form'
        onSubmit={handleSubmit}
        sx={{
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          height: '80vh',
        }}
      >
        <FormControl
          sx={{ m: 1, width: '25ch' }}
          variant='outlined'
          error={!!formErrors.userName}
          required
        >
          <InputLabel htmlFor='user'>User</InputLabel>
          <OutlinedInput
            id='userName'
            type='text'
            value={formValues.userName}
            onChange={handleChange}
            endAdornment={
              <InputAdornment position='end'>
                <AccountCircle />
              </InputAdornment>
            }
            label='user'
          />

          <FormHelperText>{formErrors.userName}</FormHelperText>
        </FormControl>
        <FormControl
          sx={{ m: 1, width: '25ch' }}
          variant='outlined'
          error={!!formErrors.password}
          required
        >
          <InputLabel htmlFor='password'>Password</InputLabel>
          <OutlinedInput
            id='password'
            type={showPassword ? 'text' : 'password'}
            onChange={handleChange}
            value={formValues.password}
            endAdornment={
              <InputAdornment position='end'>
                <IconButton
                  aria-label='toggle password visibility'
                  onClick={handleClickShowPassword}
                  onMouseDown={handleMouseDownPassword}
                  edge='end'
                >
                  {showPassword ? <VisibilityOff /> : <Visibility />}
                </IconButton>
              </InputAdornment>
            }
            label='password'
          />
          <FormHelperText>{formErrors.password}</FormHelperText>
        </FormControl>
        <Button
          variant='contained'
          type='submit'
          disabled={isFormEmpty || hasFormError}
        >
          Log In
        </Button>
      </Box>
    </>
  );
};

export default LoginPage;
