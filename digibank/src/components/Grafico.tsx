import React from 'react';
import ReactApexChart from 'react-apexcharts';

interface SemiCircleGaugeData {
  series: number[];
  options: ApexCharts.ApexOptions;
}

const SemiCircleGaugeChart: React.FC<SemiCircleGaugeData> = ({ series, options }) => (
  <ReactApexChart options={options} series={series} type="radialBar" height={250} />
);

const Graphic: React.FC = () => {
  const data: SemiCircleGaugeData = {
    series: [76],
    options: {
      plotOptions: {
        radialBar: {
          startAngle: -90,
          endAngle: 90,
          hollow: {
            margin: 15,
            size: '70%',
          },
          dataLabels: {
            name: {
              show: false,
            },
            value: {
              fontSize: '30px',
            },
          },
        },
      },
      fill: {
        colors: ['#c20004', '#f20519'],
        type: 'gradient',
        gradient: {
          shade: 'light',
          shadeIntensity: 0.4,
          inverseColors: false,
          opacityFrom: 1,
          opacityTo: 1,
          stops: [0, 50, 53, 91],
        },
      },
    },
  };

  return (
    <div>
      {/* eslint-disable-next-line react/jsx-props-no-spreading */}
      <SemiCircleGaugeChart {...data} />
    </div>
  );
};

export default Graphic;
