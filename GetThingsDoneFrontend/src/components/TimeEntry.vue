<script setup>
  import { ref, computed } from 'vue'
  import { DatePicker } from 'v-calendar'
  import InputText from '@/components/InputText.vue'
  import TaskService from '@/services/TaskService.js'
  import TaskModel from '@/models/TaskModel.js'
  import TimeEntryModel from '@/models/TimeEntryModel.js'

  const props = defineProps({
    id: Number,
    date: String,
    description: String,
    hours: Number,
    taskId: Number,
    tasks: Array
  });

  const emits = defineEmits(['onEdit', 'onDelete']);

  const taskService = new TaskService();

  const currentTask = ref({});

  const getTask = () => {
    const task = taskService.getTaskById(props.taskId)
      .then(res => {
        currentTask.value = new TaskModel(res);
      })
      .catch(err => console.log(err));
  };

  const isDateOpen = ref(false);
  const isTaskOpen = ref(false);
  const dateISO = ref(props.date);
  const dateSlashes = computed(() => {
    return dateISO.value.split('-').reverse().join(' / ');
  });

  const hoursString = ref(props.hours.toString());

  const config = ref({
    type: 'string',
    mask: 'YYYY-MM-DD',
  });

  const handleHours = (elem) => {
    if (!/^([1-9]|(1[0-9]|2[0-4]))$/.test(hoursString.value)) {
      hoursString.value = hoursString.value.slice(0, -1);
    }
  };

  const editEntry = () => {
    if (!hoursString.value.length) {
      hoursString.value = props.hours.toString();
      return;
    }

    const newEntry = new TimeEntryModel(
      null, 
      dateISO.value, 
      props.taskId, 
      parseInt(hoursString.value), 
      props.description
    );

    emits('onEdit', props.id, newEntry);
    getTask();
  }

  const editTask = () => {
    if (props.taskId === currentTask.value.id) return;
    editEntry();
  }

  getTask();
</script>

<template>
  <div class="rounded-lg border-2 border-slate-200 py-3 px-4 mt-2 last:mb-8 
    flex justify-center items-center">
    <button class="relative w-fit">
      <div class="border-2 rounded-lg py-2 px-4  bg-white 
        hover:cursor-pointer flex justify-center items-center w-64"
        :class="isDateOpen ? 'bg-white rounded-b-none border-slate-200' : 'border-white'"
        @click="isDateOpen = !isDateOpen"
      >
        <h1 class="grow bg-inherit">
          {{ dateSlashes }}
        </h1>
      </div>

      <DatePicker v-if="isDateOpen" trim-weeks :firstDayOfWeek="2"
        class="border-slate-200 w-64 absolute left-0 
        border-2  rounded-lg rounded-t-none border-t-0 z-10"
        v-model="dateISO" :model-config="config" is-required
        @dayclick="() => {
          isDateOpen = false;
          editEntry();
        }"
      />
    </button>

    <button class="relative ml-2" @blur="isTaskOpen = false">
      <div class="border-2 rounded-lg py-2 px-4
         flex justify-center items-center w-72"
        :class="{
          'rounded-b-none border-slate-200': isTaskOpen,
          'border-white': !isTaskOpen,
          'hover:cursor-pointer': currentTask.active
        }"
        @click="() => {
          if(currentTask.active) isTaskOpen = !isTaskOpen
        }"
      >
      
        <h1 class="w-full">
          {{ currentTask.name?.length > 20 ? 
            `${currentTask.name?.slice(0, 20)}...` : currentTask.name
          }}
        </h1>
      </div>

      <div v-if="isTaskOpen" class="border-2 rounded-b-lg border-t-0 
        overflow-x-hidden overflow-y-auto max-h-96 absolute w-full z-10">
        <h1 v-for="task in tasks" :key="task.id" 
          class="bg-slate-100 py-1 px-4 border-b-2 last:border-b-0 last:rounded-b-md 
          hover:cursor-pointer hover:bg-white"
          @click="() => {
            isTaskOpen = false;
            taskId = task.id;
            editTask();
          }"
        >
          {{task.name}}
        </h1>
      </div>
    </button>
    
    <input placeholder="Hours"
      class="bg-white w-20 h-full text-center ml-2
      py-2 rounded-lg focus:outline-none border-2
    border-white focus:border-green-300 px-3
    placeholder:text-slate-400"
      v-model="hoursString"
      @input="(elem) => handleHours(elem)"
      @blur="editEntry"
    />

    <InputText class="py-0 px-3 w-full border-white ml-2"
      v-model:textValue="description"
      @blur="editEntry"
    />

    <i class="far fa-trash-can text-xl hover:cursor-pointer 
    hover:text-green-400 ml-4"
      @click="$emit('onDelete', id)"
    ></i>
  </div>
</template>