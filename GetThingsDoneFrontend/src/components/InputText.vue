<script setup>
  const props = defineProps({
    placeholder: String,
    textValue: String
  });

</script>

<template>
  <input 
    type="text" 
    :placeholder="placeholder"
    :value="textValue"
    @input="$emit('update:textValue', $event.target.value)"
    class="py-2 rounded-lg focus:outline-none border-2
      border-slate-200 focus:border-green-300 px-3
      placeholder:text-slate-400 focus:bg-white"
  >
</template>