import React from 'react';

interface DataPoint {
  key: string;
  value: number;
}

interface RadarChartProps {
  data: DataPoint[];
}

const RadarChart: React.FC<RadarChartProps> = ({ data }) => {
  const radius = 100; // Raio do gráfico
  const centerX = radius + 10; // Coordenada X do centro do gráfico
  const centerY = radius + 10; // Coordenada Y do centro do gráfico
  const total = data.reduce((sum, point) => sum + point.value, 0); // Total das frações

  let angle = -90; // Ângulo inicial

  return (
    <div style={{ width: `${radius * 2 + 20}px`, height: `${radius * 2 + 20}px` }}>
      <svg width={`${radius * 2 + 20}`} height={`${radius * 2 + 20}`}>
        {data.map((point) => {
          const fraction = point.value / total;
          const endX = centerX + radius * Math.cos((angle * Math.PI) / 180) * fraction;
          const endY = centerY + radius * Math.sin((angle * Math.PI) / 180) * fraction;

          const largeArcFlag = fraction > 0.5 ? 1 : 0;
          const pathData = `M${centerX},${centerY} L${endX},${endY} A${radius},${radius} 0 ${largeArcFlag} 1 ${centerX},${centerY}`;

          angle += 360 / data.length;

          return <path key={point.key} d={pathData} fill="blue" />;
        })}
      </svg>
    </div>
  );
};

export default RadarChart;
