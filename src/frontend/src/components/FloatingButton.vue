<script setup lang="ts">
import { GA4 } from "../utils/analytics";
defineProps<{
  text: string;
  location?: string;
}>();

async function pingUs() {
  try {
    GA4.pingUsButton();
  } catch (e) {
    console.error(e);
  }
}
</script>

<template>
  <a v-if="location" target="_blank" :href="location">
    <button class="floating-button" @click="$emit('click')">
      <span>{{ text }}</span>
    </button>
  </a>
  <button v-else class="floating-button" @click="$emit('click')">
    <span>{{ text }}</span>
  </button>
</template>

<style scoped>
/* Floating Button Styling */
.floating-button {
  position: fixed;
  bottom: 20px;
  right: 20px;
  background-color: #00a3c4; /* Nice blue color */
  color: white; /* Text color */
  padding: 12px 24px;
  border-radius: 30px;
  border: none;
  font-size: 16px;
  font-weight: bold;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  cursor: pointer;
  display: flex;
  justify-content: center;
  align-items: center;
  transition: all 0.3s ease-in-out;
}

/* Hover Effect */
.floating-button:hover {
  background-color: #00729c; /* Slightly darker blue */
  box-shadow: 0 6px 8px rgba(0, 0, 0, 0.2);
  transform: scale(1.05);
}

/* Active (Pressed) Effect */
.floating-button:active {
  background-color: #01445c; /* Even darker blue */
  transform: scale(0.95);
}
</style>
