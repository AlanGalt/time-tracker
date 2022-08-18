<script setup>
  import { SetupCalendar } from 'v-calendar'
  import '../node_modules/v-calendar/dist/style.css'
  import { onUpdated, ref } from 'vue'
  import router from '@/router'
  import Sidebar from '@/components/Sidebar.vue'
  import TimeEntryModel from '@/models/TimeEntryModel.js'
  import TimeEntryService from '@/services/TimeEntryService.js'
  
  const timeEntryService = new TimeEntryService();

  const monthEntries = ref([]);
  const attributes = ref([]);
  const curDate = ref('');
  
  const updatePerfomance = (month, year) => {
    let monthISO;
    if (month && year) {
      monthISO = month.toString();
      if (monthISO.length === 1) monthISO = `0${monthISO}`;
      curDate.value = `${year}-${monthISO}-01`;
    }
    
    let perfomance = {};

    timeEntryService.getAllEntries(curDate.value, true)
      .then(res => monthEntries.value = res.map(e => new TimeEntryModel(e)))
      .then(() => monthEntries.value.forEach(e => {
        perfomance[e.date] = (perfomance[e.date] || 0) + e.hours;
      }))
      .then(() => attributes.value = 
        Object.entries(perfomance).reduce((acc, [key, value]) => {
          if (value < 8) acc[0].dates.push(new Date(key));
          if (value === 8) acc[1].dates.push(new Date(key));
          if (value > 8) acc[2].dates.push(new Date(key));
          return acc;
        }, 
        [
          { 
            highlight: {
              class: 'bg-yellow-500', 
              contentClass: 'text-white font-bold'
            }, 
            dates: [] 
          },
          { 
            highlight: { 
              class: 'bg-green-400', 
              contentClass: 'text-white font-bold'
            }, 
            dates: [] 
          },
          { 
            highlight: {
              class: 'bg-red-400', 
              contentClass: 'text-white font-bold'
            }, 
            dates: [] 
          }
        ])
      )
      .then(() => attributes.value.push({ bar: 'blue', dates: new Date()}))
      .catch(err => console.log(err));
  };
</script>

<template>
  <Sidebar :monthEntries="monthEntries" :attributes="attributes"
    @onMonthChanged="(month, year) => updatePerfomance(month, year)"/>

  <main class="h-screen w-full flex-initial text-center
    flex justify-center bg-white overflow-y-scroll scroll-smooth overflow-x-hidden">
    <router-view @onUpdatePerfomance="updatePerfomance()"></router-view>
  </main>
</template>

