<script setup>
  import Task from '@/components/Task.vue'
  import InputText from '@/components/InputText.vue'
  import ProjectModel from '@/models/ProjectModel.js'
  import TaskModel from '@/models/TaskModel.js'
  import ProjectService from '@/services/ProjectService.js'
  import TaskService from '@/services/TaskService.js'

  import { ref } from 'vue'

  const props = defineProps({
    id: Number,
    name: String,
    code: String,
    active: Boolean
  });

  const emits = defineEmits(['onDelete', 'onEdit', 'onComplete']);

  const projectService = new ProjectService();
  const taskService = new TaskService();

  const isOpen = ref(false);
  const addTaskVisible = ref(false);
  const taskName = ref('');
  const tasks = ref([]);

  const updateTasks = () => {
    projectService.getTasksForProject(props.id)
      .then(res => tasks.value = res.map(task => new TaskModel(task)))
      .catch(err => console.log(err));
  };
  
  const completeAllTasks = () => {
    tasks.value
      .filter(task => task.projectId === props.id)
      .forEach(task => {
        task.active = false;
        taskService.editTask(task.id, task)
          .then(() => updateTasks())
          .catch(err => console.log(err));
      });
  };

  const onAddTask = () => {
    if (taskName.value.length === 0) {
      alert("Task name can't be empty");
      return;
    }
    const newTask = new TaskModel(null, props.id, taskName.value);
    taskService.addTask(newTask)
      .then(() => updateTasks())
      .catch(err => console.log(err));
    taskName.value = '';
  };
  
  const onDeleteTask = (id) => {
    taskService.deleteTask(id)
      .then(() => updateTasks())
      .catch(err => console.log(err));
  };

  const onCompleteTask = (id) => {
    if (!props.active) emits('onComplete', props.id);

    const toComplete = {...tasks.value.find(t => t.id === id)};
    toComplete.active = !toComplete.active;
    taskService.editTask(id, toComplete)
      .then(() => updateTasks())
      .catch(err => console.log(err));
  };

  const onEditTask = (id, name) => {
    const oldTask = {...tasks.value.find(t => t.id === id)};
    if (oldTask.name === name) return;
    
    if (!name.length) {
      alert("Task name can't be empty");
      return
    }


    oldTask.name = name;
    taskService.editTask(id, oldTask)
      .then(() => updateTasks())
      .catch(err => console.log(err));
  };

  updateTasks();
</script>

<template>
  <div class="border-slate-200 border-2 
    text-left py-3 px-4 mt-2 rounded-lg
    flex items-center justify-between"
    :class="[{ 'rounded-b-none border-b-0': isOpen, 'bg-slate-200': !active }]"

  >
    <div class="flex items-center grow">
      <i class="fas ml-3 mr-4 text-xl 
        hover:cursor-pointer hover:text-green-400"
        :class="isOpen ? 'fa-chevron-up' : 'fa-chevron-down'"
        @click="isOpen = !isOpen"
      ></i>

      <div class="flex flex-col grow">
        <InputText class="py-0 w-11/12 px-4"
          v-model:textValue="name" 
          :class="active ? 'bg-white border-white' : 
            'line-through bg-slate-200 border-slate-200'"
          :disabled="!active"
          @blur="active && $emit('onEdit', id, new ProjectModel(null, name, code))"
        />
  
        <InputText
          class="text-slate-400 w-11/12 py-0 px-4"
          v-model:textValue="code"
          :class="active ? 'bg-white border-white' : 
            'line-through bg-slate-200 border-slate-200'"
          :disabled="!active"
          @blur="active && $emit('onEdit', id, new ProjectModel(null, name, code))"
        />
      </div>
    </div>

    <div class="flex items-center justify-center">
      <!-- <button class="bg-green-300 rounded-lg py-1 
        w-24 mr-3 hover:bg-green-400"
        v-if="active"
        @click="handleAdd"
      >
        Add task
      </button> -->

      <i class="fas fa-check text-xl mr-3 hover:cursor-pointer 
        hover:text-green-400"
        :class="{'text-green-500': !active}"
        @click="() => {
          $emit('onComplete', id);
          completeAllTasks();
        }"
      ></i>
      <i class="far fa-trash-can text-xl hover:cursor-pointer 
        hover:text-green-400"
        @click="$emit('onDelete', id)"
      ></i>
    </div>
    
  </div> 

  <div v-if="isOpen && active">
    <form @submit.prevent="" class="flex">
      <InputText 
        placeholder="Task name"
        class="pl-20 bg-slate-100 rounded-none grow"
        v-model:textValue="taskName"
      />
      <button class="bg-green-300 py-2 w-36 hover:bg-green-400"
        @click="onAddTask"
      >
        Add task
      </button>
    </form>
  </div>
  
  <div v-if="isOpen && tasks.length" 
    class="bg-slate-100 rounded-b-lg border-2 border-t-0 border-slate-200"
  >
    <Task v-for="task in tasks" :key="task.id" :id="task.id"
      :name="task.name" :active="task.active"
      @onDelete="(id) => onDeleteTask(id)"
      @onComplete="(id) => onCompleteTask(id)"
      @onEdit="(id, name) => onEditTask(id, name)"
    />
  </div>

</template>