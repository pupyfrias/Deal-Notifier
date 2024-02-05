import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import Slide from '@mui/material/Slide';
import { TransitionProps } from '@mui/material/transitions';
import { useState, forwardRef, ReactElement, Ref, Fragment, FC } from 'react';

const Transition = forwardRef(function Transition(
  props: TransitionProps & {
    children: ReactElement<any, any>;
  },
  ref: Ref<unknown>
) {
  return (
    <Slide
      direction='up'
      ref={ref}
      {...props}
    />
  );
});

type AlertDialogSlideProps = {
  title: string;
  contentText: string;
  show: boolean;
  setShow: (show: boolean)=> void;
  onAgree: VoidFunction;
};

const AlertDialogSlide: FC<AlertDialogSlideProps> = ({ title, contentText, show, setShow, onAgree: agree }) => {
  const [open, setOpen] = useState(show);

  React.useEffect(() => {
    setOpen(show);
  }, [show]);

  const handleClose = () => {
    console.log('dialog closed');
    setShow(false);
  };

  const handleAgree = () => {
    agree();
    setShow(false);
  };


  return (
    <Fragment>
      <Dialog
        open={open}
        TransitionComponent={Transition}
        keepMounted
        onClose={handleClose}
        aria-describedby='alert-dialog-slide-description'
      >
        <DialogTitle>{title}</DialogTitle>
        <DialogContent>
          <DialogContentText id='alert-dialog-slide-description'>{contentText}</DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Disagree</Button>
          <Button onClick={handleAgree}>Agree</Button>
        </DialogActions>
      </Dialog>
    </Fragment>
  );
};

export default AlertDialogSlide;
