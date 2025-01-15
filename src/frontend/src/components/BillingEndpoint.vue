<script setup lang="ts">
import { ref } from "vue";
import useSignalR from "../composables/useSignalR";
import EndpointHeader from "./EndpointHeader.vue";
import {
  isError,
  isOrderBilled,
  OrderBilled,
  OrderPlaced,
  type MessageOrError,
  type Order,
} from "./types";
import { store } from "./shared";
import MessageContainer from "./MessageContainer.vue";
import OnOffSwitch from "./OnOffSwitch.vue";

var { connection, state } = useSignalR("http://localhost:5002/billingHub");

const processedCount = ref(0);
const erroredCount = ref(0);
const shouldFailRetries = ref(false);
const messages = ref<MessageOrError[]>([]);

connection.on("ProcessingMessage", (order: Order) => {
  if (order) {
    messages.value = [
      { timestamp: new Date(), message: new OrderPlaced(order) },
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});
connection.on("MessageError", (order: Order, messageId: string) => {
  if (order) {
    messages.value = [
      {
        timestamp: new Date(),
        message: new OrderPlaced(order),
        isError: true,
        messageId,
      },
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});
connection.on("OrderBilled", (order: Order) => {
  if (order) {
    messages.value = [
      { timestamp: new Date(), message: new OrderBilled(order) },
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});

connection.on("SyncValues", (processed, errored, failRetries) => {
  processedCount.value = processed;
  erroredCount.value = errored;
  shouldFailRetries.value = failRetries;
});

function toggleFailOnRetries() {
  connection.invoke("SetFailRetries", !shouldFailRetries.value);
}
</script>

<template>
  <div class="endpoint-header">
    <div>
      <EndpointHeader label="Billing" :state="state" />
      <div class="counter-info">
        <span>
          {{ processedCount }} messages processed /
          <span class="red"> {{ erroredCount }} errored</span>
        </span>
      </div>
    </div>
    <div>
      <OnOffSwitch
        id="failOnRetries"
        label="Fail Retries"
        @toggle="toggleFailOnRetries"
        :value="shouldFailRetries"
      />
    </div>
  </div>
  <MessageContainer :messages="messages" v-slot="{ message }">
    <span>{{ message.timestamp.toLocaleTimeString() }}</span>
    <template v-if="isError(message)">
      <span>Order</span>
      <span
        class="coloured error"
        :class="store.selectedMessage === message.message.orderId && 'selected'"
      >
        {{ message.message.orderId }}
      </span>
      <span>failed.</span>
      <a
        target="_blank"
        href="http://localhost:5173/#/failed-messages/all-failed-messages"
      >
        View failure in ServicePulse
      </a>
    </template>
    <template v-else-if="isOrderBilled(message.message)">
      <span>Order</span>
      <span
        class="coloured"
        :class="store.selectedMessage === message.message.orderId && 'selected'"
      >
        {{ message.message.orderId }}
      </span>
      <span>billed</span>
    </template>
    <template v-else>
      <span>Received order placed</span>
      <span
        class="coloured"
        :class="store.selectedMessage === message.message.orderId && 'selected'"
      >
        {{ message.message.orderId }}
      </span>
    </template>
  </MessageContainer>
</template>

<style scoped>
.endpoint-header {
  margin-top: 1em;
  display: flex;
  justify-content: space-between;
}
</style>
