<script setup>
  import InputText from '@/components/InputText.vue'

  const props = defineProps({
    id: Number,
    name: String,
    active: Boolean
  });

  const emits = defineEmits(['onComplete', 'onDelete', 'onEdit']);

</script>

<template>
  <!--  -->
  <div class="flex items-center justify-between px-4 py-2
    border-b-2 border-slate-200 last:border-b-0
  ">

    <div class="pl-0.5 grow text-left">
      <InputText class="ml-11 px-4 py-0 w-11/12 bg-slate-100 border-slate-100"
      v-model:textValue="name" 
      :class="{'line-through': !active }"
      :disabled="!active"
      @blur="active && $emit('onEdit', id, name)"
    />
    </div>
    


    <div>
      <i class="fas fa-check text-xl mr-3 hover:cursor-pointer 
        hover:text-green-400"
        :class="{'text-green-500': !active}"
        @click="$emit('onComplete', id)"
      ></i>
      <i class="far fa-trash-can text-xl hover:cursor-pointer 
        hover:text-green-400"
        @click="$emit('onDelete', id)"
      ></i>
    </div>
    
  </div>
</template>