import React from 'react';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Bar } from 'react-chartjs-2';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

export const options = {
  indexAxis: 'y' as const,
  responsive: true,
};

const labels = ['5', '4', '3', '2', '1'];

export const data = {
  labels,
  datasets: [
    {
      label: 'Classificação dos usuários',
      data: labels.map(() => [1]),
      backgroundColor: 'grey',
    },
  ],
};

export function RatingGraph() {
  return <Bar options={options} data={data} />;
}
