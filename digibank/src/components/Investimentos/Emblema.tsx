import React from 'react';

export default function Emblema({ name, valor }: { name: string; valor: number }) {
  function EmblemaColor() {
    let emblemaColor: string;
    switch (valor) {
      case 1:
        emblemaColor = '#90086B';
        break;
      case 2:
        emblemaColor = '00802E';
        break;
      case 3:
        emblemaColor = '#1ed760';
        break;
      default:
        emblemaColor = '#000';
        break;
    }

    return emblemaColor;
  }
  return (
    <div style={{ backgroundColor: `${EmblemaColor()}20` }}>
      <span style={{ color: `${EmblemaColor()}99` }}>{name}</span>
    </div>
  );
}
