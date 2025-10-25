<template>
  <div class="home">
    <h1>🏠 首頁總覽</h1>

    <!-- 儀表板統計卡片 -->
    <div class="stats">
      <div class="card" v-for="(value, key) in statsDisplay" :key="key">
        <h2>{{ value.display }}</h2>
        <p>{{ value.label }}</p>
      </div>
    </div>

    <!-- 公告 -->
    <div class="announcement-list">
      <h2 class="title">社區最新公告</h2>
      <div v-if="announcements.length === 0" class="no-data">
        目前沒有任何公告。
      </div>
      <div v-else>
        <div
          v-for="item in announcements"
          :key="item.id"
          class="announcement-item"
        >
          <h3 class="announcement-title">{{ item.title }}</h3>
          <p class="announcement-date">{{ item.date }}</p>
          <p class="announcement-content">{{ item.content }}</p>
        </div>
      </div>
    </div>

    <!-- 近期活動 -->
    <div class="activities">
      <h3>📋 近期住戶動態</h3>
      <table>
        <thead>
          <tr>
            <th>住戶姓名</th>
            <th>房號</th>
            <th>狀態</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="a in recentActivities" :key="a.id">
            <td>{{ a.name }}</td>
            <td>{{ a.room }}</td>
            <td>{{ a.status }}</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- 快捷操作 -->
    <div class="actions">
      <h3>⚙️ 系統操作</h3>
      <router-link to="/residents" class="btn">查看住戶列表</router-link>
      <router-link to="/residentsCRUD" class="btn">CRUD 管理</router-link>
      <button class="btn" @click="getSummary">重新載入資料</button>
      <button class="btn" @click="getMessage">測試後端連線</button>
    </div>

    <!-- 後端訊息 -->
    <p :class="['message', messageType]">🔗 {{ message }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import axios from "axios";

// --- Axios instance 統一設定 ---
const api = axios.create({
  baseURL: "http://localhost:5000/api",
  timeout: 5000,
});

// --- 假資料 fallback ---
const defaultStats = {
  totalResidents: 120,
  totalAnnouncements: 5,
  pendingMaintenance: 3,
  totalManagementFee: 150000,
};
const defaultAnnouncements = [
  {
    id: 1,
    title: "電梯維護通知",
    content:
      "本週六上午 9:00 至下午 1:00 將進行電梯年度維護，屆時請住戶改用樓梯。",
    date: "2025-10-15",
  },
  {
    id: 2,
    title: "社區烤肉大會報名",
    content: "歡迎住戶踴躍報名，一同共襄盛舉。",
    date: "2025-10-10",
  },
];
const defaultActivities = [
  { id: 1, name: "王小明", room: "A101", status: "新入住" },
  { id: 2, name: "陳美玲", room: "B302", status: "搬出" },
  { id: 3, name: "張志豪", room: "C205", status: "更新資料" },
  { id: 4, name: "李育誠", room: "D401", status: "新入住" },
  { id: 5, name: "林采蓁", room: "E105", status: "更新聯絡方式" },
];

// --- Vue 狀態 ---
const stats = ref({ ...defaultStats });
const announcements = ref([...defaultAnnouncements]);
const recentActivities = ref([...defaultActivities]);
const message = ref("等待後端訊息...");
const messageType = ref("info"); // success / error / info

// --- 顯示格式化資料 ---
const statsDisplay = computed(() => ({
  totalResidents: { display: stats.value.totalResidents, label: "住戶總數" },
  totalAnnouncements: {
    display: stats.value.totalAnnouncements,
    label: "公告數",
  },
  pendingMaintenance: {
    display: stats.value.pendingMaintenance,
    label: "待處理報修",
  },
  totalManagementFee: {
    display: `NT$ ${stats.value.totalManagementFee.toLocaleString()}`,
    label: "管理費總額",
  },
}));

// --- 撈取後端儀表板資料 ---
async function getSummary() {
  try {
    const res = await api.get("/dashboard/summary");
    if (res.data?.data) {
      const data = res.data.data;
      stats.value = {
        totalResidents: data.totalResidents,
        totalAnnouncements: data.totalAnnouncements,
        pendingMaintenance: data.pendingMaintenance,
        totalManagementFee: data.totalManagementFee,
      };
      message.value = res.data.message || "成功取得後端資料";
      messageType.value = "success";
    } else {
      message.value = "⚠️ 後端回傳資料格式異常";
      messageType.value = "error";
    }
  } catch (error) {
    console.warn("⚠️ 無法連線後端，使用假資料顯示：", error.message);
    message.value = "無法連線後端，顯示預設資料";
    messageType.value = "error";
    stats.value = { ...defaultStats };
    announcements.value = [...defaultAnnouncements];
    recentActivities.value = [...defaultActivities];
  }
}

// --- 測試後端連線 ---
async function getMessage() {
  try {
    const res = await api.get("/test/hello");
    message.value = res.data.message;
    messageType.value = "success";
  } catch (error) {
    message.value = "無法連線後端：" + error.message;
    messageType.value = "error";
  }
}

// --- 頁面載入時自動載入摘要 ---
onMounted(() => {
  getSummary();
});
</script>

<style scoped>
.home {
  padding: 20px;
  font-family: "Noto Sans TC", sans-serif;
}

.stats {
  display: grid;
  grid-template-columns: repeat(2, minmax(180px, 1fr));
  gap: 15px;
  margin-bottom: 30px;
}

.card {
  background: #f8fafc;
  border-radius: 10px;
  padding: 20px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  text-align: center;
}

.card h2 {
  margin: 0;
  color: #2563eb;
  font-size: 1.8em;
}

.announcement-list {
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 8px;
}

.title {
  color: #333;
  border-bottom: 2px solid #ccc;
  padding-bottom: 10px;
}

.announcement-item {
  border-bottom: 1px solid #eee;
  padding: 15px 0;
}

.announcement-item:last-child {
  border-bottom: none;
}

.announcement-title {
  color: #0056b3;
  margin-bottom: 5px;
}

.announcement-date {
  font-size: 0.8em;
  color: #666;
  margin-bottom: 10px;
}

.no-data {
  text-align: center;
  color: #999;
  font-style: italic;
  padding: 20px;
}

.activities {
  margin-bottom: 30px;
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 10px;
  border-bottom: 1px solid #ddd;
  text-align: left;
}

.actions {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.btn {
  background-color: #3b82f6;
  color: white;
  padding: 8px 15px;
  border-radius: 6px;
  text-decoration: none;
  transition: 0.2s;
  border: none;
  cursor: pointer;
}

.btn:hover {
  background-color: #2563eb;
}

/* 訊息分類 */
.message.success {
  color: green;
}
.message.error {
  color: red;
}
.message.info {
  color: #475569;
}

/* RWD 手機版 */
@media (max-width: 600px) {
  .stats {
    grid-template-columns: 1fr;
  }
}
</style>
