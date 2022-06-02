import { createRouter, createWebHistory} from 'vue-router'
import Timesheet from '@/views/Timesheet.vue'
import Projects from '@/views/Projects.vue'

const routes = [
  {
    path: '/',
    redirect: '/projects'
  },
  {
    path: '/timesheet',
    name: 'timesheet',
    component: Timesheet
  },
  {
    path: '/projects',
    name: 'projects',
    component: Projects
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
});

export default router