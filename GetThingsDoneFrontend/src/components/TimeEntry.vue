<script setup>
  import TimeEntryModel from '@/models/TimeEntryModel.js'

  const props = defineProps({
    id: Number,
    date: String,
    issueId: Number,
    description: String,
    hours: Number
  });

</script>

<template>
  <h1>
    {{ id }} - {{ date }} - {{ issueId }} - {{ description }} - {{ hours }}
  </h1>
</template>