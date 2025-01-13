import './assets/main.css'
import { initializeAnalytics } from "./utils/analytics";

import { createApp } from 'vue'
import App from './App.vue'


export default {
    mounted() {
      initializeAnalytics();
    },
};
  
createApp(App).mount('#app')
