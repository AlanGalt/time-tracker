<script setup>
  import { DatePicker } from 'v-calendar'
  import { ref, computed } from 'vue'
  
  import InputText from '@/components/InputText.vue'
  import TimeEntry from '@/components/TimeEntry.vue'
  import TaskService from '@/services/TaskService.js'
  import TimeEntryService from '@/services/TimeEntryService.js'
  import TaskModel from '@/models/TaskModel.js'
  import TimeEntryModel from '@/models/TimeEntryModel.js'
  import { useToast } from "vue-toastification"

  const toast = useToast();
  const emits = defineEmits(['onUpdatePerfomance']);

  const tasks = ref([]);
  const entries = ref([]);
  const taskService = new TaskService();
  const timeEntryService = new TimeEntryService();

  const dateISO = ref(new Date().toISOString().split('T')[0]);
  const hours = ref('');
  const description = ref('');
  const taskId = ref(0);
  const selectedName = ref('Pick a task');
  const filterValue = ref('All time');

  const isDateOpen = ref(false);
  const isTaskOpen = ref(false);
  const isFilterOpen = ref(false);

  const dateSlashes = computed(() => {
    const selected = dateISO.value.length ? 
      dateISO.value : new Date().toISOString().split('T')[0];
    return selected.split('-').reverse().join(' / ');
  });

  const config = ref({
    type: 'string',
    mask: 'YYYY-MM-DD',
  });

  const getTaskNames = () => {
    taskService.getActiveTasks()
      .then(res => tasks.value = res.map(task => new TaskModel(task)))
      .catch(err => console.log(err));
  };

  const updateTimeEntries = () => {
    let monthOnly = false;
    let date = dateISO.value;

    if (filterValue.value === 'All time') {
      date = null;
      monthOnly = null;
    }
    
    if (filterValue.value === 'Month') monthOnly = true;

    timeEntryService.getAllEntries(date, monthOnly)
      .then(res => entries.value = res.map(e => new TimeEntryModel(e)))
      .then(() => emits('onUpdatePerfomance'))
      .catch(err => console.log(err));
  };

  const dayclick = (e) => {
    isDateOpen.value = false;
    if (filterValue.value !== 'All time') updateTimeEntries();
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
      toast.error('Task is required!');
      return;
    }

    if (!hours.value.length) {
      toast.error('Hours are required!');
      return;
    }

    const newEntry = new TimeEntryModel(null, dateISO.value, taskId.value, 
      parseInt(hours.value), description.value);

    timeEntryService.addTimeEntry(newEntry)
      .then(() => {
        updateTimeEntries();
        toast.success("Time entry added!");
      })
      .catch(err => {
        console.log(err);
        if (err.response.status === 422) {
          toast.error("Can't track more than 24 hours per day!");
        }
      });

    selectedName.value = 'Pick a task';
    hours.value = '';
    description.value = '';
  };

  const onEdit = (id, newEntry) => {
    const oldEntry = {...entries.value.find(e => e.id === id)};
    if (oldEntry.taskId === newEntry.issueId &&
        oldEntry.date === newEntry.date &&
        oldEntry.hours === newEntry.hours &&
        oldEntry.description === newEntry.description) return;

    timeEntryService.editTimeEntry(id, newEntry)
      .then(() => {
        updateTimeEntries();
        toast.success("Changes saved!")
      })
      .catch(err => {
        console.log(err);
        if (err.response.status === 422) {
          toast.error("Can't track more than 24 hours per day!");
        }
      });
  };

  const onDelete = (id) => {
    timeEntryService.deleteTimeEntry(id)
      .then(() => updateTimeEntries())
      .catch(err => console.log(err));
  };

  getTaskNames();
  updateTimeEntries();
</script>

<template>
  <div class="h-fit">
    <h1 class="mt-9 text-2xl">Timesheet</h1>
    
    <button class="relative w-fit mt-9 z-30 block" @blur="isFilterOpen = false">
      <div class="border-2 rounded-lg py-2 px-4 w-32 hover:bg-white
        hover:cursor-pointer flex justify-center items-center"
        :class="isFilterOpen ? 'bg-white rounded-b-none' : 'bg-slate-100'"
        @click="isFilterOpen = !isFilterOpen"
      >
        <i class="fas text-xl"
          :class="isFilterOpen ? 'fa-chevron-up' : 'fa-chevron-down'"
        ></i>

        <h1 class="grow bg-inherit ml-3" >
          {{ filterValue }}
        </h1>
      </div>

      <div v-if="isFilterOpen" 
        class="absolute left-0 z-20 bg-slate-100 w-full border-slate-200 
        border-2 rounded-lg rounded-t-none border-t-0">
        <h1 class="border-b-2 hover:bg-white hover:cursor-pointer"
          @click="() => {
            filterValue = 'All time';
            isFilterOpen = false;
            updateTimeEntries();
          }">All time</h1>
        <h1 class="border-b-2 hover:bg-white hover:cursor-pointer"
          @click="() => {
            filterValue = 'Day';
            isFilterOpen = false;
            updateTimeEntries();
          }">Day</h1>
        <h1 class="hover:bg-white hover:cursor-pointer rounded-b-lg"
          @click="() => {
            filterValue = 'Month';
            isFilterOpen = false;
            updateTimeEntries();
          }">Month</h1>
      </div>
    </button>
    
    <form class="flex w-full mt-2" @submit.prevent="">
      <button class="relative grow">
        <div class="border-2 rounded-lg py-2 px-4 hover:bg-white 
          hover:cursor-pointer flex justify-center items-center w-[17.125rem]"
          :class="isDateOpen ? 'bg-white rounded-b-none' : 'bg-slate-100'"
          @click="isDateOpen = !isDateOpen"
        >
          <i class="fas text-xl"
            :class="isDateOpen ? 'fa-chevron-up' : 'fa-chevron-down'"
          ></i>

          <h1 class="grow bg-inherit" >
            {{ dateSlashes }}
          </h1>
        </div>

        <DatePicker v-if="isDateOpen" trim-weeks :firstDayOfWeek="2"
          class="border-slate-200 w-full absolute left-0 
          border-2  rounded-lg rounded-t-none border-t-0 z-20"
          v-model="dateISO" :model-config="config" is-required
          @dayclick="dayclick"
        />
      </button>

      <button class="relative ml-2" @blur="isTaskOpen = false">
        <div class="border-2 rounded-lg py-2 px-4 h-full hover:bg-white 
          hover:cursor-pointer flex justify-center items-center w-72"
          :class="isTaskOpen ? 'bg-white rounded-b-none' : 'bg-slate-100'"
          @click="isTaskOpen = !isTaskOpen"
        >
          <i class="fas text-xl absolute left-5"
            :class="isTaskOpen ? 'fa-chevron-up' : 'fa-chevron-down'"
          ></i>
        
          <h1 class="w-full">
            {{ selectedName.length > 16 ? 
              `${selectedName.slice(0, 16)}...` : selectedName
            }}
          </h1>
        </div>

        <div v-if="isTaskOpen" class="border-2 rounded-b-lg border-t-0 
          overflow-x-hidden overflow-y-auto max-h-96 absolute w-full z-20">
          <h1 v-for="task in tasks" :key="task.id" 
            class="bg-slate-100 py-1 px-4 border-b-2 last:border-b-0 last:rounded-b-md 
            hover:cursor-pointer hover:bg-white"
            @click="pickTask(task)"
          >
            {{task.name}}
          </h1>
        </div>
      </button>

      <div>
        <input placeholder="Hours"
          class="bg-slate-100 focus:bg-white w-20 h-full text-center ml-2
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
          class="ml-2 bg-slate-100 h-full"
          v-model:textValue="description"
        />
      </div>

      <div>
        <button class="bg-green-300 rounded-lg py-2 h-full 
          px-2 w-24 ml-2 hover:bg-green-400"
        @click="onAddTimeEntry">
          Track
        </button>
      </div>
    </form>

    <div v-if="entries.length">
      <TimeEntry v-for="entry in entries" :key="entry.id" 
        :id="entry.id" :date="entry.date" :description="entry.description" 
        :hours="entry.hours" :taskId="entry.taskId" :tasks="tasks"
        @onEdit="(id, newEntry) => onEdit(id, newEntry)"
        @onDelete="(id) => onDelete(id)"
      />
    </div>

  </div>

  
</template>