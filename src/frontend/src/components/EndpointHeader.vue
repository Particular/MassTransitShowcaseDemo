<script setup lang="ts">
import type { HubConnectionState } from "@microsoft/signalr";

const props = defineProps<{
  label: string;
  state: HubConnectionState;
}>();
</script>

<template>
  <div class="header">
    <div :title="props.state" class="status" :class="props.state"></div>
    <h3>{{ props.label }}</h3>
  </div>
</template>

<style scoped>
.header {
  display: flex;
  align-items: baseline;
  gap: 0.5em;
}

.status {
  width: 1em;
  height: 1em;
  border-radius: 0.5em;
}

.Connected {
  background-color: green;
}

.Disconnected,
.Disconnecting {
  background-color: red;
}

.Connecting,
.Reconnecting {
  background-color: orange;
}
</style>
