import { createApp } from 'vue'
import App from './App.vue'
import './index.css'
import router from '@/router'

// import '../node_modules/vue-toastification/dist/index.css'
import "@/customToast.scss";
import Toast, { TYPE } from "vue-toastification";

const app = createApp(App)

const toastOptions = {
  toastDefaults: {
    [TYPE.ERROR]: {
      timeout: 4000,
      pauseOnHover: false
    },
    [TYPE.SUCCESS]: {
      timeout: 1500,
      pauseOnHover: false,
      hideProgressBar: true,
    }    
}
};

app.use(router);
app.use(Toast, toastOptions);

app.mount('#app');
