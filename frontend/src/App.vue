<template>
  <div>
    <nav class="navbar">
      <div class="nav-links">
        <router-link to="/">首頁</router-link>
        <router-link to="/residents">住戶</router-link>
        <router-link to="/residentsCRUD">CRUD 管理</router-link>
      </div>
      <button v-if="token" @click="logout" class="logout-btn">登出</button>
    </nav>

    <!-- 加上上方空白，避免被固定導覽列遮住 -->
    <div class="content">
      <router-view />
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()
const token = ref(localStorage.getItem('token'))

// 每次路由變化時都重新檢查 token
watch(
  () => route.fullPath,
  () => {
    token.value = localStorage.getItem('token')
  }
)

window.addEventListener('storage', () => {
  token.value = localStorage.getItem('token')
})

function logout() {
  localStorage.removeItem('token')
  localStorage.removeItem('username')
  token.value = null
  router.push('/login')
}
</script>

<style scoped>
.navbar {
  position: fixed; /* 固定在畫面上方 */
  top: 0;
  left: 0;
  width: 100%;
  z-index: 1000;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #3b82f6;
  padding: 10px 20px;
  color: white;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
}

.nav-links a {
  color: white;
  text-decoration: none;
  margin-right: 15px;
  font-weight: 500;
  transition: color 0.2s;
}

.nav-links a:hover {
  color: #dbeafe;
}

.logout-btn {
  background-color: #ef4444;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 6px;
  margin-right: 30px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.logout-btn:hover {
  background-color: #b91c1c;
}

/* 為內容預留空間，避免被固定導覽列遮住 */
.content {
  padding-top: 60px;
}
</style>
