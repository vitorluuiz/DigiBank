import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import eslint from 'vite-plugin-eslint';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [eslint({ cache: false }), react()],
  server: {
    host: '192.168.0.7',
    port: 3000,
  },
});
