<template>
  <div class="login-container">
    <h2>系統登入</h2>
    <input v-model="username" placeholder="帳號" />
    <input v-model="password" placeholder="密碼" type="password" />
    <button @click="login">登入</button>
    <p v-if="error" style="color:red">{{ error }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const username = ref('')
const password = ref('')
const error = ref('')
const router = useRouter()

async function login() {
  try {
    const res = await axios.post('http://localhost:5000/api/auth/login', {
      username: username.value,
      password: password.value
    })
    localStorage.setItem('token', res.data.token)
    localStorage.setItem('username', res.data.username)
    localStorage.setItem('token', res.data.token)
    window.dispatchEvent(new Event('storage'))
    router.push('/')
  } catch (err) {
    error.value = err.response?.data?.message || '登入失敗'
  }
}
</script>

<style scoped>
.login-container {
  display: flex;
  flex-direction: column;
  width: 250px;
  margin: 100px auto;
  gap: 10px;
}
</style>
