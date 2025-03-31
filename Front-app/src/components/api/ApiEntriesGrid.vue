<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';
import './ApiEntriesGrid.css'; 

interface ApiEntry {
  id: number;
  api: string;
  description: string;
  auth: string;
  https: boolean;
  cors: string;
  link: string;
  category: string;
}

const apiEntries = ref<ApiEntry[]>([]);
const filterText = ref("");

const filteredEntries = computed(() => {
  return apiEntries.value.filter(entry =>
    entry.api.toLowerCase().includes(filterText.value.toLowerCase()) ||
    entry.description.toLowerCase().includes(filterText.value.toLowerCase())
  );
});

const fetchApiEntries = async () => {
  try {
    const response = await axios.get<ApiEntry[]>('http://localhost:5299/api/ApiEntries');
    console.log("API Response:", response.data); 
    apiEntries.value = response.data;
  } catch (error) {
    console.error("Error fetching API entries:", error);
  }
};

onMounted(fetchApiEntries);
</script>

<template>
  <div>
    <input v-model="filterText" placeholder="Filter APIs..." class="filter-input" />

    <table class="api-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Breed</th>
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
          <td>{{ entry.https ? 'Yes' : 'No' }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>