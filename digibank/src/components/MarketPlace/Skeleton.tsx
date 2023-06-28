import React from 'react';
import { Skeleton } from '@mui/material';

export default function SkeletonComponent() {
  return (
    <div
      style={{ display: 'flex', justifyContent: 'space-between', height: '100%', width: '100%' }}
    >
      {/* Renderizar o Skeleton do tamanho e estilo desejados */}
      <div style={{ width: '20%', height: '100%' }}>
        <Skeleton variant="rectangular" width="80%" height={225} />
        <Skeleton variant="text" width="75%" sx={{ fontSize: '3rem' }} />
        <Skeleton variant="text" width="25%" sx={{ fontSize: '2rem' }} />
      </div>
      <div style={{ width: '20%', height: '100%' }}>
        <Skeleton variant="rectangular" width="80%" height={225} />
        <Skeleton variant="text" width="75%" sx={{ fontSize: '3rem' }} />
        <Skeleton variant="text" width="25%" sx={{ fontSize: '2rem' }} />
      </div>
      <div style={{ width: '20%', height: '100%' }}>
        <Skeleton variant="rectangular" width="80%" height={225} />
        <Skeleton variant="text" width="75%" sx={{ fontSize: '3rem' }} />
        <Skeleton variant="text" width="25%" sx={{ fontSize: '2rem' }} />
      </div>
      <div style={{ width: '20%', height: '100%' }}>
        <Skeleton variant="rectangular" width="80%" height={225} />
        <Skeleton variant="text" width="75%" sx={{ fontSize: '3rem' }} />
        <Skeleton variant="text" width="25%" sx={{ fontSize: '2rem' }} />
      </div>
    </div>
  );
}
