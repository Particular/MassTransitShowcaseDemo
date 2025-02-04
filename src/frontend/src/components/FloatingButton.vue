<script setup lang="ts">
withDefaults(
  defineProps<{
    text: string;
    location?: string;
    color?: string;
  }>(),
  { color: "#00a3c4" }
);
</script>

<template>
  <a
    class="floating-button"
    v-if="location"
    target="_blank"
    :style="{ 'background-color': color }"
    :href="location"
  >
    <span @click="$emit('click')">
      {{ text }}
    </span>
  </a>
</template>

<style scoped>
/* Floating Button Styling */
a.floating-button {
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
a.floating-button:hover {
  background-image: linear-gradient(rgb(0 0 0/15%) 0 0);
  box-shadow: 0 6px 8px rgba(0, 0, 0, 0.2);
  transform: scale(1.05);
}

/* Active (Pressed) Effect */
a.floating-button:active {
  background-image: linear-gradient(rgb(0 0 0/25%) 0 0);
  transform: scale(0.95);
}
</style>
