<template>
  <div class="login-container">
    <h2>系統登入</h2>
    <input v-model="username" placeholder="帳號" />
    <input v-model="password" placeholder="密碼" type="password" />
    <button @click="login">登入</button>
    <p v-if="message" :class="['message', messageType]">{{ message }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

// --- Axios instance ---
const api = axios.create({
  baseURL: 'http://localhost:5000/api',
  timeout: 5000,
})

const username = ref('')
const password = ref('')
const message = ref('')
const messageType = ref('info') // success / error
const router = useRouter()

async function login() {
  // 前端簡單驗證
  if (!username.value || !password.value) {
    message.value = '帳號與密碼不得為空'
    messageType.value = 'error'
    return
  }

  try {
    const res = await api.post('/auth/login', {
      username: username.value,
      password: password.value
    })

    // 存 token 與使用者資訊
    localStorage.setItem('token', res.data.token)
    localStorage.setItem('username', res.data.username)
    window.dispatchEvent(new Event('storage'))

    message.value = '登入成功'
    messageType.value = 'success'

    // 導頁
    router.push('/')
  } catch (err) {
    message.value = err.response?.data?.message || '登入失敗'
    messageType.value = 'error'
  }
}
</script>

<style scoped>
.login-container {
  display: flex;
  flex-direction: column;
  width: 90%;
  max-width: 300px;
  margin: 100px auto;
  gap: 10px;
}

input {
  padding: 8px;
  border-radius: 5px;
  border: 1px solid #ccc;
}

button {
  padding: 8px;
  border-radius: 5px;
  border: none;
  background-color: #3b82f6;
  color: white;
  cursor: pointer;
  transition: 0.2s;
}

button:hover {
  background-color: #2563eb;
}

.message.success {
  color: green;
}

.message.error {
  color: red;
}

.message.info {
  color: #475569;
}

/* RWD 調整 */
@media (max-width: 400px) {
  .login-container {
    margin: 50px auto;
  }
}
</style>
