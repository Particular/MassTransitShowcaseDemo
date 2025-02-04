<script setup lang="ts"></script>

<template>
  <div class="floating-container">
    <slot></slot>
  </div>
</template>

<style scoped>
.floating-container {
  position: fixed;
  bottom: 1.5em;
  right: 1.5em;
  max-width: 15em;
  display: flex;
  flex-direction: column;
  gap: 0.5em;
}
</style>
