/* eslint-disable @typescript-eslint/no-unused-vars */
import { AlertColor } from '@mui/material';
import { createContext } from 'react';

type ToastContextProps = {
  isOpen: boolean;
  severity: AlertColor;
  autoHideDuration: number;
  message: string;
  setIsOpen: (_isOpen: boolean) => void;
  setSeverity: (_severity: AlertColor) => void;
  setAutoHideDuration: (_autoHideDuration: number) => void;
  setMessage: (_message: string) => void;
  showToast: (_message: string, _severity?: AlertColor, _autoHideDuration?: number) => void;
};

const ToastContext = createContext<ToastContextProps>({
  autoHideDuration: 600,
  isOpen: false,
  message: '',
  severity: 'success',
  setAutoHideDuration: () => {},
  setIsOpen: () => {},
  setMessage: () => {},
  setSeverity: () => {},
  showToast: () => {},
});

export default ToastContext;
