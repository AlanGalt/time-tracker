<script setup>
  import Link from './Link.vue'
  import { DatePicker } from 'v-calendar'
  import { ref } from 'vue'

  const links = ref([
    {   
      to: "/timesheet", 
      text: "Timesheet", 
      icon: "far fa-clock"
    },
    {
      to: "/projects", 
      text: "Projects", 
      icon: "far fa-folder"
    }
  ]);
</script>

<template>
  <nav class="h-screen w-96 flex-initial text-center bg-white">
    <div class="mt-8">
      <i class="far fa-calendar-check text-3xl pr-3"></i>
      <h1 class="text-2xl font-bold inline">Get It Done</h1>
    </div>
   
    <DatePicker class="w-64 mt-8 border-slate-200 rounded-lg border-2"/>
    
    <button class="text-slate-50 bg-slate-800 hover:bg-slate-900 
      py-2 w-64 rounded-lg mt-2"
    >
      Hide perfomance
    </button>

    <div class="mt-8">
        <Link 
          v-for="(link, idx) in links" :key="idx"
          :to="link.to" 
          :text="link.text" 
          :icon="link.icon"
        />
    </div>
  </nav>
</template>