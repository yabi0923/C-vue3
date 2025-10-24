<template>
  <div class="home">
    <h1>🏠 首頁總覽</h1>

    <!-- 儀表板統計卡片 -->
    <div class="stats">
      <div class="card">
        <h2>{{ stats.totalResidents }}</h2>
        <p>住戶總數</p>
      </div>
      <div class="card">
        <h2>{{ stats.totalAnnouncements }}</h2>
        <p>公告數</p>
      </div>
      <div class="card">
        <h2>{{ stats.pendingMaintenance }}</h2>
        <p>待處理報修</p>
      </div>
      <div class="card">
        <h2>NT$ {{ stats.totalManagementFee.toLocaleString() }}</h2>
        <p>管理費總額</p>
      </div>
    </div>

    <!-- 公告 -->
    <div class="announcement-list">
      <h2 class="title">社區最新公告</h2>
      <div v-if="announcements.length === 0" class="no-data">
        目前沒有任何公告。
      </div>
      <div v-else>
        <div v-for="item in announcements" :key="item.id" class="announcement-item">
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
          <tr v-for="(a, i) in recentActivities" :key="i">
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
    <p class="message">🔗 {{ message }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";

// --- 預設假資料（當後端掛掉時會用這些顯示） ---
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
    content: "本週六上午 9:00 至下午 1:00 將進行電梯年度維護，屆時請住戶改用樓梯。",
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
  { name: "王小明", room: "A101", status: "新入住" },
  { name: "陳美玲", room: "B302", status: "搬出" },
  { name: "張志豪", room: "C205", status: "更新資料" },
  { name: "李育誠", room: "D401", status: "新入住" },
  { name: "林采蓁", room: "E105", status: "更新聯絡方式" },
];

// --- Vue 狀態 ---
const stats = ref(defaultStats);
const announcements = ref(defaultAnnouncements);
const recentActivities = ref(defaultActivities);
const message = ref("等待後端訊息...");

// --- 撈取後端儀表板資料 ---
async function getSummary() {
  try {
    const res = await axios.get("http://localhost:5000/api/dashboard/summary");
    if (res.data && res.data.data) {
      const data = res.data.data;
      stats.value = {
        totalResidents: data.totalResidents,
        totalAnnouncements: data.totalAnnouncements,
        pendingMaintenance: data.pendingMaintenance,
        totalManagementFee: data.totalManagementFee,
      };
      message.value = res.data.message || "成功取得後端資料";
    } else {
      message.value = "⚠️ 後端回傳資料格式異常";
    }
  } catch (error) {
    console.warn("⚠️ 無法連線後端，使用假資料顯示：", error.message);
    message.value = "無法連線後端，顯示預設資料";
  }
}

// --- 測試後端連線 ---
async function getMessage() {
  try {
    const res = await axios.get("http://localhost:5000/api/test/hello");
    message.value = res.data.message;
  } catch (error) {
    message.value = "無法連線後端：" + error.message;
  }
}

// --- 頁面載入時自動載入摘要 ---
onMounted(() => {
  getSummary();
});
</script>

<style scoped>
/* 保留你的原本樣式 */
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
.message {
  margin-top: 20px;
  font-size: 1em;
  color: #475569;
}
</style>
