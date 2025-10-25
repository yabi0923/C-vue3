<template>
  <div class="p-6">
    <h1 class="text-2xl font-bold mb-4">🏠 住戶管理系統</h1>

    <!-- 操作訊息 -->
    <p v-if="message" :class="['message', messageType]">{{ message }}</p>

    <!-- 搜尋框 -->
    <div class="mb-4">
      <input
        v-model="searchQuery"
        placeholder="🔍 搜尋姓名、房號"
        class="border p-2 w-full rounded"
      />
    </div>

    <!-- 新增住戶區塊 -->
    <div class="mb-4 flex flex-wrap gap-2">
      <input
        v-model="newResident.name"
        placeholder="姓名"
        class="border p-2 rounded"
      />
      <input
        v-model="newResident.roomNumber"
        placeholder="房號"
        class="border p-2 rounded"
      />
      <input
        v-model.number="newResident.managementFee"
        type="number"
        placeholder="管理費 (NT$)"
        class="border p-2 rounded w-40"
      />
      <button
        @click="createResident"
        class="bg-blue-600 hover:bg-blue-700 text-white px-3 py-2 rounded"
      >
        ➕ 新增
      </button>
    </div>

    <!-- 住戶資料表 -->
    <table class="table-auto border-collapse border border-gray-400 w-full text-left">
      <thead>
        <tr class="bg-gray-100">
          <th class="border p-2">ID</th>
          <th class="border p-2">姓名</th>
          <th class="border p-2">房號</th>
          <th class="border p-2">管理費 (NT$)</th>
          <th class="border p-2">操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="r in filteredResidents" :key="r.id">
          <td class="border p-2">{{ r.id }}</td>

          <td class="border p-2">
            <input v-model="r.name" class="border p-1 w-full rounded" />
          </td>

          <td class="border p-2">
            <input v-model="r.roomNumber" class="border p-1 w-full rounded" />
          </td>

          <td class="border p-2">
            <input
              v-model.number="r.managementFee"
              type="number"
              class="border p-1 w-full rounded"
            />
          </td>

          <td class="border p-2 flex gap-2">
            <button
              @click="updateResident(r)"
              class="bg-green-600 hover:bg-green-700 text-white px-2 py-1 rounded"
            >
              💾 更新
            </button>
            <button
              @click="deleteResident(r.id)"
              class="bg-red-600 hover:bg-red-700 text-white px-2 py-1 rounded"
            >
              🗑 刪除
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import axios from "axios";

const apiBase = "http://localhost:5000/api/residents";
const residents = ref([]);
const newResident = ref({ name: "", roomNumber: "", managementFee: 0 });
const searchQuery = ref("");

// 操作訊息
const message = ref("");
const messageType = ref("info"); // info / success / error

// 搜尋條件
const filteredResidents = computed(() =>
  residents.value.filter((r) =>
    r.name.includes(searchQuery.value) ||
    r.roomNumber.includes(searchQuery.value)
  )
);

// 讀取住戶資料
async function loadResidents() {
  try {
    const res = await axios.get(apiBase);
    residents.value = res.data;
  } catch (err) {
    message.value = "載入住戶資料失敗";
    messageType.value = "error";
  }
}

// 新增住戶
async function createResident() {
  if (!newResident.value.name || !newResident.value.roomNumber) {
    message.value = "請輸入完整的姓名與房號";
    messageType.value = "error";
    return;
  }

  // 防止重複房號
  if (residents.value.some(r => r.roomNumber === newResident.value.roomNumber)) {
    message.value = "房號已存在，請確認";
    messageType.value = "error";
    return;
  }

  try {
    await axios.post(apiBase, newResident.value);
    newResident.value = { name: "", roomNumber: "", managementFee: 0 };
    await loadResidents();
    message.value = "新增住戶成功";
    messageType.value = "success";
  } catch (err) {
    message.value = "新增住戶失敗";
    messageType.value = "error";
  }
}

// 更新住戶
async function updateResident(r) {
  try {
    await axios.put(`${apiBase}/${r.id}`, r);
    await loadResidents();
    message.value = "更新成功";
    messageType.value = "success";
  } catch (err) {
    message.value = "更新失敗";
    messageType.value = "error";
  }
}

// 刪除住戶
async function deleteResident(id) {
  if (!confirm("確定要刪除此住戶嗎？")) return;

  try {
    await axios.delete(`${apiBase}/${id}`);
    await loadResidents();
    message.value = "刪除成功";
    messageType.value = "success";
  } catch (err) {
    message.value = "刪除失敗";
    messageType.value = "error";
  }
}

onMounted(loadResidents);
</script>

<style scoped>
table {
  border-radius: 8px;
  overflow: hidden;
}
input {
  outline: none;
}
.message {
  margin-bottom: 15px;
  padding: 8px;
  border-radius: 4px;
}
.message.info { background-color: #cce5ff; color: #004085; }
.message.success { background-color: #d4edda; color: #155724; }
.message.error { background-color: #f8d7da; color: #721c24; }
</style>
