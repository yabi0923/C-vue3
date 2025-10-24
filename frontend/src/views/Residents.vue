<template>
  <div class="residents-page">
    <!-- 返回上一頁 -->
    <button class="back-btn" @click="goBack">⬅ 返回上一頁</button>

    <h1>🏠 住戶專屬頁面</h1>

    <!-- 上方 Tab 切換列 -->
    <div class="tab-menu">
      <button
        v-for="tab in tabs"
        :key="tab.id"
        :class="{ active: activeTab === tab.id }"
        @click="activeTab = tab.id"
      >
        {{ tab.name }}
      </button>
    </div>

    <!-- 內容區塊 -->
    <div class="tab-content">
      <!-- 個人訊息中心 -->
      <div v-if="activeTab === 'dashboard'">
        <h2>個人訊息中心</h2>
        <h3>待領取信件/包裹</h3>
        <ul>
          <li v-for="item in parcels" :key="item.id">{{ item.type }} - {{ item.status }}</li>
        </ul>

        <h3>帳單管理</h3>
        <ul>
          <li v-for="bill in bills" :key="bill.id">
            {{ bill.name }} - {{ bill.amount }}元 - {{ bill.paid ? '已繳' : '未繳' }}
            <button v-if="!bill.paid" @click="payBill(bill.id)">繳費</button>
          </li>
        </ul>

        <h3>報修進度</h3>
        <ul>
          <li v-for="repair in repairs" :key="repair.id">{{ repair.title }} - {{ repair.status }}</li>
        </ul>
      </div>

      <!-- 公共設施預約 -->
      <div v-if="activeTab === 'booking'">
        <h2>公共設施預約</h2>
        <ul>
          <li v-for="facility in facilities" :key="facility.id">
            {{ facility.name }}
            <button @click="bookFacility(facility.id)">預約</button>
          </li>
        </ul>

        <h3>我的預約</h3>
        <ul>
          <li v-for="my in myBookings" :key="my.id">
            {{ my.name }} - {{ my.date }}
            <button @click="cancelBooking(my.id)">取消</button>
          </li>
        </ul>
      </div>

      <!-- 社區活動 -->
      <div v-if="activeTab === 'events'">
        <h2>社區活動</h2>
        <ul>
          <li v-for="event in events" :key="event.id">
            {{ event.name }} - 已報名 {{ event.registered.length }} 人
            <button @click="registerEvent(event.id)">報名</button>
          </li>
        </ul>

        <h3>活動照片牆</h3>
        <div class="photos">
          <img v-for="photo in photos" :src="photo" :key="photo" class="event-photo" />
        </div>
      </div>

      <!-- 社區生活小幫手 -->
      <div v-if="activeTab === 'lifeHelper'">
        <h2>生活小幫手</h2>
        <h3>鄰近店家優惠</h3>
        <ul>
          <li v-for="offer in offers" :key="offer.id">{{ offer.shop }} - {{ offer.discount }}</li>
        </ul>

        <h3>住戶二手交易</h3>
        <ul>
          <li v-for="item in marketplace" :key="item.id">{{ item.name }} - {{ item.status }}</li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
function goBack() {
  router.go(-1)
}

// Tab 列
const tabs = [
  { id: 'dashboard', name: '訊息中心' },
  { id: 'booking', name: '設施預約' },
  { id: 'events', name: '社區活動' },
  { id: 'lifeHelper', name: '生活小幫手' },
]
const activeTab = ref('dashboard')

// 個人訊息中心
const parcels = ref([
  { id: 1, type: '掛號信', status: '待領取' },
  { id: 2, type: '包裹', status: '已通知' },
])
const bills = ref([
  { id: 1, name: '管理費', amount: 2000, paid: false },
  { id: 2, name: '水電費', amount: 1500, paid: true },
])
const repairs = ref([
  { id: 1, title: '水管漏水', status: '處理中' },
  { id: 2, title: '燈泡更換', status: '已完成' },
])
function payBill(id) {
  const bill = bills.value.find(b => b.id === id)
  if (bill) bill.paid = true
  alert('繳費完成！')
}

// 公共設施預約
const facilities = ref([
  { id: 1, name: '健身房' },
  { id: 2, name: '撞球間' },
])
const myBookings = ref([])
function bookFacility(id) {
  const facility = facilities.value.find(f => f.id === id)
  myBookings.value.push({ id: Date.now(), name: facility.name, date: new Date().toLocaleDateString() })
}
function cancelBooking(id) {
  myBookings.value = myBookings.value.filter(b => b.id !== id)
}

// 社區活動
const events = ref([
  { id: 1, name: '社區瑜伽', registered: [] },
  { id: 2, name: '園遊會', registered: [] },
])
const photos = ref([
  'https://via.placeholder.com/150',
  'https://via.placeholder.com/150',
])
function registerEvent(id) {
  const event = events.value.find(e => e.id === id)
  event.registered.push('住戶A')
}

// 生活小幫手
const offers = ref([
  { id: 1, shop: '咖啡店', discount: '買一送一' },
])
const marketplace = ref([
  { id: 1, name: '二手書', status: '待售' },
])
</script>

<style scoped>
.residents-page { padding: 20px; font-family: Arial, sans-serif; }
.back-btn { margin-bottom: 20px; cursor: pointer; }

.tab-menu {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
}
.tab-menu button {
  padding: 8px 16px;
  border: 1px solid #2c3e50;
  background: #fff;
  cursor: pointer;
  border-radius: 4px;
}
.tab-menu button.active {
  background: #2c3e50;
  color: #fff;
}

.tab-content h2 { color: #2c3e50; margin-top: 0; }
.event-photo { width: 100px; height: 100px; margin-right: 10px; margin-bottom: 10px; }
button { margin-left: 10px; cursor: pointer; }
</style>
