import { createTheme } from '@mui/material/styles';

// eslint-disable-next-line import/prefer-default-export
export const ThemeToggleButtonProvider = (color: string) =>
  createTheme({
    palette: {
      primary: {
        main: `#${color}`,
      },
    },
  });

export const ThemeTabsProvider = (color: string) =>
  createTheme({
    palette: {
      primary: {
        main: `#${color}`,
      },
    },
  });
