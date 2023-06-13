import { styled } from '@mui/material/styles';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
// import { borderBottom } from '@mui/system';

export const CustomTabs = styled(Tabs)({
  // borderBottom: '3px solid #e8e8e8',
  '& .MuiTabs-indicator': {
    backgroundColor: '#000000',
  },
  '& .MuiTab-root': {
    height: 30,
  },
  '& .MuiTab-textColorPrimary ': {
    color: '#000000',
  },
  '& .Mui-selected': {
    color: '#000000',
    textColor: '#000000',
  },
});

export const CustomTab = styled(Tab)({
  textTransform: 'none',
  minWidth: 0,
  color: 'rgba(0, 0, 0, 0.85)',
});
