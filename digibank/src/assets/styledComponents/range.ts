import { createTheme } from '@mui/material/styles';

// eslint-disable-next-line import/prefer-default-export
export const CustomSilder = createTheme({
  components: {
    MuiSlider: {
      styleOverrides: {
        root: {
          colorPrimary: '#c00414',
          color: '#c00414',
        },
      },
    },
  },
});
