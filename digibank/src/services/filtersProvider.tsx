import { SelectChangeEvent } from '@mui/material';
import React, { SyntheticEvent, createContext, useContext, useState } from 'react';

interface FilterContextType {
  areas: number[];
  minValorAcao: number;
  maxValorAcao: number;
  minMarketCap: number;
  maxMarketCap: number;
  minDividendo: number;
  handleOptionArea: (event: SelectChangeEvent<number[]> | null) => void;
  handleValorChange: (
    event: Event | SyntheticEvent<Element, Event>,
    newValue: number | number[],
  ) => void;
  handleMarketCapChange: (
    event: Event | SyntheticEvent<Element, Event>,
    newValue: number | number[],
  ) => void;
  handleDividendosChange: (event: Event | SyntheticEvent<Element, Event>, newValue: number) => void;
}

const FilterContext = createContext<FilterContextType | undefined>(undefined);

export function FiltersProvider({ children }: { children: React.ReactNode }) {
  const [areas, setAreaInvestimentos] = useState<number[]>([]);
  const [minValorAcao, setValorDeAcaoMin] = useState<number>(0);
  const [maxValorAcao, setValorDeAcaoMax] = useState<number>(100);
  const [minMarketCap, setMarketCapMin] = useState<number>(0);
  const [maxMarketCap, setMarketCapMax] = useState<number>(100);
  const [minDividendo, setPercentualDividendosMin] = useState<number>(0);

  const handleOptionArea = (event: SelectChangeEvent<number[]> | null) => {
    if (event !== null) {
      const {
        target: { value },
      } = event;
      setAreaInvestimentos(value as number[]);
    } else {
      setAreaInvestimentos([]);
    }
  };

  const handleValorChange = (
    event: Event | SyntheticEvent<Element, Event>,
    newValue: number | number[],
  ) => {
    if (typeof newValue !== 'number') {
      setValorDeAcaoMin(newValue[0]);
      setValorDeAcaoMax(newValue[1]);
    }
  };

  const handleMarketCapChange = (
    event: Event | SyntheticEvent<Element, Event>,
    newValue: number | number[],
  ) => {
    if (typeof newValue !== 'number') {
      setMarketCapMin(newValue[0]);
      setMarketCapMax(newValue[1]);
    }
  };

  const handleDividendosChange = (
    event: Event | SyntheticEvent<Element, Event>,
    newValue: number,
  ) => {
    setPercentualDividendosMin(newValue);
  };

  return (
    <FilterContext.Provider
      // eslint-disable-next-line react/jsx-no-constructed-context-values
      value={{
        areas,
        minValorAcao,
        maxValorAcao,
        minMarketCap,
        maxMarketCap,
        minDividendo,
        handleOptionArea,
        handleValorChange,
        handleMarketCapChange,
        handleDividendosChange,
      }}
    >
      {children}
    </FilterContext.Provider>
  );
}

export const useFilterBar = (): FilterContextType => {
  const context = useContext(FilterContext);
  if (!context) {
    throw new Error('useFilterBar must be used within a FilterBarProvider');
  }
  return context;
};
