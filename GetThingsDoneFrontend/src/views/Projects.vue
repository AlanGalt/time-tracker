<script setup>
  import { ref, computed } from 'vue'
  
  import InputText from '@/components/InputText.vue'
  import Project from '@/components/Project.vue'
  import ProjectService from '@/services/ProjectService.js'
  import TaskService from '@/services/TaskService.js'
  import ProjectModel from '@/models/ProjectModel.js'
  import TaskModel from '@/models/TaskModel.js'
  import { useToast } from "vue-toastification"

  const toast = useToast();

  const projectService = new ProjectService();
  const taskService = new TaskService();

  const projects = ref([]);
  const name = ref('');
  const code = ref('');

  const updateProjects = () => {
    projectService.getAllProjects()
      .then(res => {
          projects.value = res.map(project => new ProjectModel(project));
        }
      )
      .catch(err => console.log(err));
  };

  const onAddProject = () => {
    if (name.value.length === 0) {
      toast.error("Project name is required!");
      return;
    }
    
    if (!code.value.length) code.value = "#"
    const newProject = new ProjectModel(null, name.value, code.value);
    
    projectService.addProject(newProject)
      .then(() => updateProjects())
      .then(() => toast.success("Project created!"));

    name.value = '';
    code.value = '';
  };

  const onDelete = (id) => {
    projectService.deleteProject(id)
      .then(() => updateProjects())
      .catch(err => console.log(err));
  };

  const onEdit = (id, newProject) => {
    const oldProject = projects.value.find(p => p.id === id);
    if (oldProject.name === newProject.name && 
      oldProject.code === newProject.code) return;

    if (!newProject.name.length) {
      toast.error("Project name is required!");
      return;
    }

    projectService.editProject(id, newProject)
      .then(() => updateProjects())
      .then(() => toast.success("Changes saved!"))
      .catch(err => console.log(err));
  };

  const onComplete = (id) => {
    const toComplete = {...projects.value.find(p => p.id === id)};
    toComplete.active = !toComplete.active;
    projectService.editProject(id, toComplete)
      .then(() => updateProjects())
      .catch(err => console.log(err));
  };

  updateProjects();
</script>

<template>
  <div class="w-fit h-fit">
    <h1 class="mt-9 text-2xl">Projects</h1>
    <form @submit.prevent="" class="mt-9">
      <InputText 
        placeholder="Project name"
        class="mr-2 w-96 bg-slate-100"
        v-model:textValue="name"
      />

      <InputText 
        placeholder="Project code"
        class="mr-2 w-52 bg-slate-100"
        v-model:textValue="code"
      />
  
      <button class="bg-green-300 rounded-lg py-2 w-36 h-11 px-2 hover:bg-green-400"
        @click="onAddProject"
      >
        Create project
      </button>
    </form>

    <div v-if="projects.length">
      <Project v-for="project in projects" :key="project.id"
        :id="project.id" :name="project.name" :code="project.code" 
        :active="project.active"
        @onDelete="(id) => onDelete(id)"
        @onEdit="(id, newProject) => onEdit(id, newProject)"
        @onComplete="(id) => onComplete(id)"
      />
    </div>

  </div>
</template>