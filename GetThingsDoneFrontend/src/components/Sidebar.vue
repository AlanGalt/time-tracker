<script setup>
  import Link from '@/components/Link.vue'
  import { Calendar } from 'v-calendar'
  import { ref, computed } from 'vue'
  import TimeEntryModel from '@/models/TimeEntryModel.js'
  import TimeEntryService from '@/services/TimeEntryService.js'
  
  const timeEntryService = new TimeEntryService();

  const emits = defineEmits(['onMonthChanged']);

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

  const props = defineProps({
    monthEntries: Array, 
    attributes: Array
  });

  const hidePerfomance = ref(false);
  const hideState = computed(() => hidePerfomance.value ? 'Show' : 'Hide');
  const highlightToday = ref([{
    highlight: 'blue',
    dates: new Date()
  }]);
</script>

<template>
  <nav class="h-screen w-96 flex-initial text-center bg-white">
    <div class="mt-8">
      <i class="far fa-calendar-check text-3xl pr-3"></i>
      <h1 class="text-2xl font-bold inline">Get It Done</h1>
    </div>

    <Calendar class="w-64 mt-8 border-slate-200 rounded-lg border-2"
      is-required :firstDayOfWeek="2"
      @update:from-page="(e) => $emit('onMonthChanged', e.month, e.year)" 
      :attributes="hidePerfomance ? highlightToday : attributes"
    />
    
    <button class="text-slate-50 bg-slate-700 hover:bg-slate-800 
      py-2 w-64 rounded-lg mt-2"
      @click="hidePerfomance = !hidePerfomance"
    >
      {{ `${hideState} perfomance` }}
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