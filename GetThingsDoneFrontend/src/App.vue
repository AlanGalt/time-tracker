<script setup>
  import { SetupCalendar } from 'v-calendar'
  import '../node_modules/v-calendar/dist/style.css'
  import Sidebar from '@/components/Sidebar.vue'
  import router from '@/router'
</script>

<template>
  <Sidebar />

  <main class="h-screen w-full flex-initial text-center
    flex justify-center bg-white overflow-y-auto overflow-x-hidden">
    <router-view></router-view>
  </main>
</template>

