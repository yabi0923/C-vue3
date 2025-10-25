<template>
  <div>
    <nav class="navbar">
      <div class="nav-links">
        <router-link to="/">首頁</router-link>
        <router-link to="/residents">住戶</router-link>
        <router-link to="/residentsCRUD">CRUD 管理</router-link>
      </div>

      <!-- 登出按鈕：登入時顯示 -->
      <button
        v-if="isLoggedIn"
        @click="logout"
        class="logout-btn"
        type="button"
        aria-label="登出"
      >
        登出
      </button>
    </nav>
    <div class="content">
      <router-view />
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted, onUnmounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()

// ===== 登入狀態處理 =====
const token = ref(localStorage.getItem('token'))
const isLoggedIn = computed(() => !!token.value)

// 每次路由變化時重新檢查 token
watch(
  () => route.fullPath,
  () => {
    token.value = localStorage.getItem('token')
  }
)

// storage 事件監聽（跨分頁同步登入狀態）
function onStorage(e) {
  if (e.key === 'token') {
    token.value = e.newValue
  }
}

onMounted(() => {
  window.addEventListener('storage', onStorage)
})

onUnmounted(() => {
  window.removeEventListener('storage', onStorage)
})

// ===== 登出邏輯 =====
function logout() {
  localStorage.removeItem('token')
  localStorage.removeItem('username')
  token.value = null
  router.replace('/login') // 使用 replace 避免返回受保護頁面
}
</script>

<style scoped>
:root {
  --navbar-height: 60px;
}

.navbar {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: var(--navbar-height);
  z-index: 1000;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #3b82f6;
  padding: 0 20px;
  color: white;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
}

.nav-links {
  display: flex;
  gap: 15px;
}

.nav-links a {
  color: white;
  text-decoration: none;
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
  margin-right: 25px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.logout-btn:hover {
  background-color: #b91c1c;
}

.logout-btn:focus-visible {
  outline: 2px solid #dbeafe;
  outline-offset: 2px;
}

.content {
  padding-top: var(--navbar-height);
}
</style>
