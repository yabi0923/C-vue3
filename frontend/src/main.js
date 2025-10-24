import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import router from './router' 
import axios from "axios";

axios.interceptors.response.use(
  res => res,
  err => {
    alert(err.response?.data?.message || "伺服器發生未知錯誤");
    return Promise.reject(err);
  }
);

createApp(App)
  .use(router)                
  .mount('#app')
