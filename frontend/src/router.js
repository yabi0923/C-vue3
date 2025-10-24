import { createRouter, createWebHistory } from 'vue-router'
import Home from './views/Home.vue'
import Login from './views/Login.vue'
import Residents from './views/Residents.vue'
import ResidentsCRUD from './views/ResidentsCRUD.vue'
const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  { path: '/login',name: 'Login',component: Login },
  {
    path: '/residentsCRUD',
    name: 'ResidentsCRUD',
    component: ResidentsCRUD
  },
  {
    path: '/residents',
    name: 'Residents',
    component: Residents
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})
router.beforeEach((to, from, next) => {
  const publicPages = ['/login']
  const authRequired = !publicPages.includes(to.path)
  const token = localStorage.getItem('token')

  if (authRequired && !token) {
    return next('/login')
  }
  next()
})

export default router
