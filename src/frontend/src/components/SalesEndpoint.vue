<script setup lang="ts">
import useSignalR from "../composables/useSignalR";
import { ref } from "vue";
import EndpointHeader from "./EndpointHeader.vue";

var { connection, state } = useSignalR("http://localhost:5001/salesHub");

const processedCount = ref(0);
const erroredCount = ref(0);
const messages = ref<string[]>([]);

connection.on("ProcessingMessage", (order) => {
  if (order) {
    var date = new Date();
    messages.value = [
      `${date.toLocaleDateString()} ${date.toLocaleTimeString()} Received order request ${
        order.orderId
      } for ${order.contents}`,
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});
connection.on("OrderPlaced", (order) => {
  if (order) {
    var date = new Date();
    messages.value = [
      `${date.toLocaleDateString()} ${date.toLocaleTimeString()} Order ${
        order.orderId
      } placed`,
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});

connection.on("MessagesProcessed", (processed, errored) => {
  processedCount.value = processed;
  erroredCount.value = errored;
});
</script>

<template>
  <EndpointHeader label="Sales Endpoint" :state="state" />
  <div class="counter-info">
    <span>
      {{ processedCount }} messages processed /
      <span class="red"> {{ erroredCount }} errored</span>
    </span>
  </div>
  <textarea rows="3" readonly v-text="messages.join('\n')" />
</template>

<style scoped>
.valueChangeControl {
  display: flex;
  gap: 0.25em;
}

.withCount {
  display: flex;
  gap: 0.5em;
}

textarea {
  margin-top: 0.5em;
  width: 100%;
}
</style>
