import { AlertColor } from '@mui/material';

export interface MessageProps {
  message: string;
  severity: AlertColor;
  timeSpan: number;
  open?: boolean;
}
