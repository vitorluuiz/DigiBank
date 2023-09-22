import { SelectChangeEvent } from '@mui/material';
import React, { createContext, useContext, useState } from 'react';

interface FilterContextType {
  areaInvestimentos: number[];
  valorDeAcaoMin: number;
  valorDeAcaoMax: number;
  marketCapMin: number;
  marketCapMax: number;
  percentualDividendosMin: number;
  percentualDividendosMax: number;
  handleOptionArea: (event: SelectChangeEvent<number[]>) => void;
  handleValorChange: (event: Event, newValue: number | number[]) => void;
  handleMarketCapChange: (event: Event, newValue: number | number[]) => void;
  handleDividendosChange: (event: Event, newValue: number | number[]) => void;
}

const FilterContext = createContext<FilterContextType | undefined>(undefined);

export function FiltersProvider({ children }: { children: React.ReactNode }) {
  const [areaInvestimentos, setAreaInvestimentos] = useState<number[]>([]);
  const [valorDeAcaoMin, setValorDeAcaoMin] = useState<number>(0);
  const [valorDeAcaoMax, setValorDeAcaoMax] = useState<number>(100);
  const [marketCapMin, setMarketCapMin] = useState<number>(0);
  const [marketCapMax, setMarketCapMax] = useState<number>(20000000);
  const [percentualDividendosMin, setPercentualDividendosMin] = useState<number>(0);
  const [percentualDividendosMax, setPercentualDividendosMax] = useState<number>(100);

  const handleOptionArea = (event: SelectChangeEvent<number[]>) => {
    const {
      target: { value },
    } = event;
    setAreaInvestimentos(value as number[]);
  };

  const handleValorChange = (event: Event, newValue: number | number[]) => {
    if (typeof newValue !== 'number') {
      setValorDeAcaoMin(newValue[0]);
      setValorDeAcaoMax(newValue[1]);
    }
  };

  const handleMarketCapChange = (event: Event, newValue: number | number[]) => {
    if (typeof newValue !== 'number') {
      setMarketCapMin(newValue[0]);
      setMarketCapMax(newValue[1]);
    }
  };

  const handleDividendosChange = (event: Event, newValue: number | number[]) => {
    if (typeof newValue !== 'number') {
      setPercentualDividendosMin(newValue[0]);
      setPercentualDividendosMax(newValue[1]);
    }
  };

  return (
    <FilterContext.Provider
      // eslint-disable-next-line react/jsx-no-constructed-context-values
      value={{
        areaInvestimentos,
        valorDeAcaoMin,
        valorDeAcaoMax,
        marketCapMin,
        marketCapMax,
        percentualDividendosMin,
        percentualDividendosMax,
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
