import React, { createContext, useContext, useState } from 'react';
import { MessageProps } from '../@types/snackBar';

interface SnackBarContextType {
  currentMessage: MessageProps;
  postMessage: (messageProps: MessageProps) => void;
  handleCloseSnackBar: () => void;
}

const SnackBarContext = createContext<SnackBarContextType | undefined>(undefined);

export function SnackBarProvider({ children }: { children: React.ReactNode }) {
  const [currentMessage, setMessage] = useState<MessageProps>({
    message: 'Indefinido',
    severity: 'warning',
    timeSpan: 2000,
    open: false,
  });

  const handleOpenSnackBar = () => {
    setMessage((prevMessage) => ({
      ...prevMessage,
      open: true,
    }));
  };

  const handleCloseSnackBar = () => {
    setMessage((prevMessage) => ({
      ...prevMessage,
      open: false,
    }));
  };

  const postMessage = (messageProps: MessageProps) => {
    setMessage(messageProps);
    handleOpenSnackBar();
  };

  return (
    // eslint-disable-next-line react/jsx-no-constructed-context-values
    <SnackBarContext.Provider value={{ currentMessage, postMessage, handleCloseSnackBar }}>
      {children}
    </SnackBarContext.Provider>
  );
}

export const useSnackBar = (): SnackBarContextType => {
  const context = useContext(SnackBarContext);
  if (!context) {
    throw new Error('useSnackBar must be used within a SnackBarProvider');
  }
  return context;
};
