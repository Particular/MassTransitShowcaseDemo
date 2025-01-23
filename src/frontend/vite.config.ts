import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

// https://vitejs.dev/config/
export default defineConfig(() => {


  return {
    plugins: [plugin()],
    server: {
      port: process.env.PORT ? parseInt(process.env.PORT) : 61335,
    },
    define: {
      BILLING_SIGNALR: process.env.BILLING_SIGNALR,
      CLIENT_SIGNALR: process.env.CLIENT_SIGNALR,
      SALES_SIGNALR: process.env.SALES_SIGNALR,
      SHIPPING_SIGNALR: process.env.SHIPPING_SIGNALR,
    }
  }
}
);
