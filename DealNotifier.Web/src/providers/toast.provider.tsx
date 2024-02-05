import { ToastContext } from '@/contexts';
import { AlertColor } from '@mui/material';
import { FC, ReactNode, useState } from 'react';

type ToastProviderProps = {
  children: ReactNode;
};
const ToastProvider: FC<ToastProviderProps> = ({ children }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [severity, setSeverity] = useState('success' as AlertColor);
  const [autoHideDuration, setAutoHideDuration] = useState(6000);
  const [message, setMessage] = useState('');

  const showToast = (_message: string, _severity: AlertColor = severity, _autoHideDuration: number = autoHideDuration) => {
    setMessage(_message);
    setSeverity(_severity);
    setAutoHideDuration(_autoHideDuration);
    setIsOpen(true);
  };

  return (
    <ToastContext.Provider
      value={{
        isOpen: isOpen,
        setIsOpen: setIsOpen,
        severity: severity,
        setSeverity: setSeverity,
        autoHideDuration: autoHideDuration,
        setAutoHideDuration: setAutoHideDuration,
        message: message,
        setMessage: setMessage,
        showToast: showToast,
      }}
    >
      {children}
    </ToastContext.Provider>
  );
};

export default ToastProvider;
