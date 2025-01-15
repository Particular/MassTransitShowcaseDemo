<script setup lang="ts">
import { store } from "./shared";
import type { Message } from "./types";

const props = defineProps<{
  messages: Message[];
}>();
</script>

<template>
  <div class="message-container">
    <div
      v-for="message in messages"
      @mouseover="store.setSelectedMessage(message.message.orderId)"
      @mouseout="store.clearSelectedMessage()"
      class="message"
    >
      <slot :message="message" />
    </div>
  </div>
</template>

<style scoped>
.message-container {
  margin: 0.5em 0;
  width: 100%;
  height: 6em;
  border-radius: 0.5em;
  box-shadow: 0 2px 10px rgba(24, 10, 113, 0.749);
  overflow: auto;
  resize: vertical;
}

.message {
  display: flex;
  gap: 0.25em;
  flex-wrap: wrap;
  padding: 0.15rem 0.5rem;
  font-size: 0.9em;
}

.message:nth-of-type(even) {
  background-color: rgba(60, 52, 114, 0.062);
}

:slotted(.message .coloured) {
  color: rgb(19, 19, 179);
  font-weight: bold;
}

:slotted(.message .coloured.error) {
  color: rgb(179, 19, 19);
  font-weight: bold;
}

:slotted(.message .selected) {
  background-color: rgb(242, 152, 33);
}
</style>
