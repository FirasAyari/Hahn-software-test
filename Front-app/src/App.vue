<script setup lang="ts">
import ApiEntriesGrid from './components/api/ApiEntriesGrid.vue';
</script>

<template>
  <main>
    <ApiEntriesGrid />
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
  text-align: center; 
  padding: 1rem 0;
  background-color: #34495e;
  color: #fff;
}

.wrapper {
  display: flex;
  justify-content: center; 
  align-items: center; 
}

main {
  display: flex;
  justify-content: center;
  align-items: flex-start;
  min-height: 100vh;
  margin: 0 auto;
  padding-top: 2rem;
}
</style>