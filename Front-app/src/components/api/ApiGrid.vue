<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { fetchApiEntries } from "@/services/apiService";
import type { ApiEntry } from "@/services/apiService";

const apiEntries = ref<ApiEntry[]>([]);
const filterText = ref("");

const filteredEntries = computed(() =>
  apiEntries.value.filter((entry) =>
    entry.api.toLowerCase().includes(filterText.value.toLowerCase())
  )
);

onMounted(async () => {
  apiEntries.value = await fetchApiEntries();
});
</script>

<template>
  <div class="container">
    <input v-model="filterText" placeholder="Filter APIs..." class="filter-input" />

    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>API</th>
          <th>Description</th>
          <th>Auth</th>
          <th>HTTPS</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="entry in filteredEntries" :key="entry.id">
          <td>{{ entry.id }}</td>
          <td>{{ entry.api }}</td>
          <td>{{ entry.description }}</td>
          <td>{{ entry.auth }}</td>
          <td>{{ entry.https ? "Yes" : "No" }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.container {
  max-width: 800px;
  margin: auto;
  padding: 20px;
}

.filter-input {
  width: 100%;
  padding: 8px;
  margin-bottom: 10px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}
</style>
