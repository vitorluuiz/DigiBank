import Alert from '@mui/material/Alert/Alert';
import Snackbar from '@mui/material/Snackbar/Snackbar';
import React from 'react';
import { MessageProps } from '../../@types/snackBar';

function CustomSnackbar({ message, onClose }: { message: MessageProps; onClose: () => void }) {
  return (
    <Snackbar
      anchorOrigin={{ horizontal: 'center', vertical: 'top' }}
      open={message.open}
      autoHideDuration={message.timeSpan}
      onClose={onClose}
    >
      <Alert variant="filled" onClose={onClose} severity={message.severity} sx={{ width: '100%' }}>
        {message.message}
      </Alert>
    </Snackbar>
  );
}

export default CustomSnackbar;
