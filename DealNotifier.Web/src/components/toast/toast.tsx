import { useToast } from "@/hooks";
import { Alert, Snackbar } from "@mui/material";
import { FC, SyntheticEvent } from "react";




const Toast: FC = ()=> {

    const {isOpen, severity, autoHideDuration, setIsOpen, message, setSeverity} = useToast();


    const handleClose = (event?: SyntheticEvent | Event, reason?: string) => {
      if (reason === 'clickaway') {
        return;
      }
  
      setIsOpen(false);
      setSeverity('success');
    };



    return (
        <Snackbar
        autoHideDuration={autoHideDuration}
        onClose={handleClose}
        open={isOpen}
        sx={{ width: '90%'}}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      >
        <Alert
          onClose={handleClose}
          severity={severity}
          variant='filled'
          sx={{ width: '90%',}}
        >
          {message}
        </Alert>
      </Snackbar>
    )
}

export default Toast;