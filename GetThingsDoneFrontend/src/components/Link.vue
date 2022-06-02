<script setup>
  const props = defineProps({
    to: String,
    text: String,
    icon: String,
    active: Boolean,
  });
</script>

<template>
  <router-link :to="props.to" @click.prevent="" v-slot="{isActive}">
    <button
      class="rounded-lg py-2 w-64 mt-3 text-left" 
      :class="[isActive ? 'bg-green-300' : 'hover:bg-slate-200']"
    >
      <i :class="props.icon" class="pl-6 pr-3"></i>
      {{ props.text }}
    </button>
  </router-link>
</template>