<script setup>
  import { DatePicker } from 'v-calendar'
  import { ref, computed } from 'vue'
  
  import InputText from '@/components/InputText.vue'
  import TimeEntry from '@/components/TimeEntry.vue'
  import TaskService from '@/services/TaskService.js'
  import TimeEntryService from '@/services/TimeEntryService.js'
  import TaskModel from '@/models/TaskModel.js'
  import TimeEntryModel from '@/models/TimeEntryModel.js'
  

  const tasks = ref([]);
  const entries = ref([]);
  const taskService = new TaskService();
  const timeEntryService = new TimeEntryService();

  

  const dateISO = ref(new Date().toISOString().split('T')[0]);
  const hours = ref('');
  const description = ref('');
  const taskId = ref(0);
  const selectedName = ref('Pick a task');

  const isDateOpen = ref(false);
  const isTaskOpen = ref(false);

  

  const dateSlashes = computed(() => {
    const selected = dateISO.value.length ? 
      dateISO.value : new Date().toISOString().split('T')[0];
    return selected.split('-').join(' / ');
  });

  const config = ref({
    type: 'string',
    mask: 'YYYY-MM-DD',
  });

  const attributes = ref([
    // {
    //   key: 'today',
    //   highlight: true, 
    //   dates: new Date()
    // }
  ]);

  const getTaskNames = () => {
    taskService.getActiveTasks()
      .then(res => tasks.value = res.map(task => new TaskModel(task)))
      .catch(err => console.log(err));
  };

  const updateTimeEntries = () => {
    timeEntryService.getAllEntries()
      .then(res => entries.value = res.map(e => new TimeEntryModel(e)))
      .catch(err => console.log(err));
  };

  const dayclick = (date) => {
    console.log(dateISO.value)
    // console.log(date);
  };

  const pickTask = (task) => {
    isTaskOpen.value = false;
    selectedName.value = task.name;
    taskId.value = task.id;
  };

  const handleHours = (elem) => {
    if (!/^([1-9]|(1[0-9]|2[0-4]))$/.test(hours.value)) {
      hours.value = hours.value.slice(0, -1);
    }
  };

  const onAddTimeEntry = () => {
    if (!taskId.value) {
      alert("Task is required");
      return;
    }

    if (!hours.value.length) {
      alert("Hours can't be empty");
      return;
    }

    const newEntry = new TimeEntryModel(null, dateISO.value, taskId.value, 
      parseInt(hours.value), description.value);

    timeEntryService.addTimeEntry(newEntry)
      .then(() => updateTimeEntries())
      .catch(err => console.log(err));
  };

  getTaskNames();
  updateTimeEntries();
</script>

<template>
  <div class="h-fit">
    <h1 class="mt-8 text-2xl">Timesheet</h1>

    <form class="mt-8 flex" @submit.prevent="">
      <div class="relative">
        <div class="border-2 rounded-lg py-2 px-4  hover:bg-white 
          hover:cursor-pointer flex justify-center items-center w-64"
          :class="isDateOpen ? 'bg-white rounded-b-none' : 'bg-slate-100'"
          @click="isDateOpen = !isDateOpen"
        >
          <i class="fas text-xl absolute left-6"
            :class="isDateOpen ? 'fa-chevron-up' : 'fa-chevron-down'"
          ></i>

          <h1 class="grow bg-inherit" >
            {{ dateSlashes }}
          </h1>
        </div>

        <DatePicker v-if="isDateOpen"
          class="border-slate-200 w-64 absolute left-0 
          border-2  rounded-lg rounded-t-none border-t-0"
          v-model="dateISO" :model-config="config" required
          @dayclick="(date) => dayclick(date)"
        />
      </div>

      <div class="relative ml-2">
        <div class="border-2 rounded-lg py-2 px-4  hover:bg-white 
          hover:cursor-pointer flex justify-center items-center w-72"
          :class="isTaskOpen ? 'bg-white rounded-b-none' : 'bg-slate-100'"
          @click="isTaskOpen = !isTaskOpen"
        >
          <i class="fas text-xl absolute left-6"
            :class="isTaskOpen ? 'fa-chevron-up' : 'fa-chevron-down'"
          ></i>
        
          <h1 class="w-full">
            {{ selectedName.length > 20 ? 
              `${selectedName.slice(0, 20)}...` : selectedName
            }}
          </h1>
        </div>

        <div v-if="isTaskOpen" class="border-2 rounded-b-lg border-t-0 
        overflow-x-hidden overflow-y-auto max-h-96 absolute w-full">
          <h1 v-for="task in tasks" :key="task.id" 
            class="bg-slate-100 py-1 px-4 border-b-2 last:border-b-0 last:rounded-b-md 
            hover:cursor-pointer hover:bg-white"
            @click="pickTask(task)"
          >
            {{task.name}}
            <!-- {{ task.name.length > 20 ? 
              `${task.name.slice(0, 20)}...` : task.name
            }} -->
          </h1>
        </div>
      </div>

      <div>
        <input placeholder="Hours"
          class="bg-slate-100 focus:bg-white w-20 text-center ml-2
          py-2 rounded-lg focus:outline-none border-2
        border-slate-200 focus:border-green-300 px-3
        placeholder:text-slate-400"
          v-model="hours"
          @input="(elem) => handleHours(elem)"
        />
      </div>

      <div>
        <InputText 
          placeholder="Decsription"
          class="ml-2 w-60 bg-slate-100"
          v-model:textValue="description"
        />
      </div>

      <div>
        <button class="bg-green-300 rounded-lg py-2 h-11 px-2 w-28 ml-2 hover:bg-green-400"
        @click="onAddTimeEntry">
          Track
        </button>
      </div>
      
    </form>

    <div v-if="entries.length">
      <TimeEntry v-for="entry in entries" :key="entry.id" 
        :id="entry.id" :date="entry.date" :description="entry.description" 
        :hours="entry.hours" :issueId="entry.issueId"
      />
    </div>

  </div>

  
</template>