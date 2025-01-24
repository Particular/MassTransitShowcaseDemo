import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

// https://vitejs.dev/config/
export default defineConfig(() => {
  return {
    plugins: [plugin()],
    server: {
      port: process.env.PORT ? parseInt(process.env.PORT) : 61335,
    }
  }
}
);
