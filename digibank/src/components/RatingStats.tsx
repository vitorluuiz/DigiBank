import React from 'react';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement } from 'chart.js';
import { Bar } from 'react-chartjs-2';
import { RatingHistograma } from '../@types/RatingHistogram';

ChartJS.register(CategoryScale, LinearScale, BarElement);

export default function Histograma({ histograma }: { histograma: RatingHistograma[] }) {
  const options = {
    indexAxis: 'y' as const,
    elements: {
      bar: {
        borderWidth: 1,
      },
    },
    responsive: true,
  };

  // const labels = ['5😁', '4😃', '3🙂', '2🤨', '1😐'];
  const labels = ['5⭐', '4⭐', '3⭐', '2⭐', '1⭐'];

  const data = {
    labels,
    datasets: [
      {
        data: histograma.map((rating) => rating.count),
        borderColor: '#d9d9d9',
        backgroundColor: '#d9d9d9',
      },
    ],
  };

  return <Bar options={options} data={data} />;
}
