<script setup lang="ts">
import useSignalR from "../composables/useSignalR";
import { ref } from "vue";
import EndpointHeader from "./EndpointHeader.vue";
import {
  isError,
  isOrderPlaced,
  OrderPlaced,
  PlaceOrder,
  type MessageOrError,
  type Order,
} from "./types";
import MessageContainer from "./MessageContainer.vue";
import { store } from "./shared";
import { GA4 } from "../utils/analytics";

const { connection, state } = useSignalR(
  `http://${import.meta.env.VITE_SALES_SIGNALR ?? "localhost:5001"}/salesHub`
);

const processedCount = ref(0);
const erroredCount = ref(0);
const shouldFailRetries = ref(false);
const messages = ref<MessageOrError[]>([]);
let retrySuccessful: string[] = [];

connection.on("ProcessingMessage", (order: Order) => {
  if (order) {
    messages.value = [
      { timestamp: new Date(), message: new PlaceOrder(order) },
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});
connection.on(
  "MessageError",
  (order: Order, messageId: string, messageViewId: string) => {
    if (order) {
      console.log(messageViewId);
      messages.value = [
        {
          timestamp: new Date(),
          message: order,
          isError: true,
          messageId,
          messageViewId,
        },
        ...messages.value,
      ].slice(0, Math.max(messages.value.length, 100));
    }
  }
);
connection.on("OrderPlaced", (order: Order) => {
  if (order) {
    messages.value = [
      {
        timestamp: new Date(),
        message: new OrderPlaced(
          order,
          retrySuccessful.includes(order.orderId)
        ),
      },
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});

connection.on("SyncValues", (processed, errored, failRetries) => {
  processedCount.value = processed;
  erroredCount.value = errored;
  shouldFailRetries.value = failRetries;
});
connection.on("RetryAttempted", () => {
  try {
    store.setMessageRetried();
    GA4.showcaseRetryAttempted();
  } catch (e) {
    console.error(e);
  }
});
connection.on("RetrySuccessful", (orderId: string) => {
  retrySuccessful = [orderId, ...retrySuccessful].slice(
    0,
    Math.max(messages.value.length, 100)
  );
});

function toggleFailOnRetries() {
  connection.invoke("SetFailRetries", !shouldFailRetries.value);
}
</script>

<template>
  <div class="endpoint-header">
    <div>
      <EndpointHeader label="Sales" :state="state" />
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
      <span>containing</span>
      <span class="coloured">{{ message.message.contents.join(", ") }}</span>
      <span>failed.</span>
      <a
        target="_blank"
        :href="`http://localhost:9090/#/failed-messages/message/${message.messageViewId}`"
        >View failure in ServicePulse</a
      >
    </template>
    <template v-else-if="isOrderPlaced(message.message)">
      <span>Order</span>
      <span
        class="coloured"
        :class="store.selectedMessage === message.message.orderId && 'selected'"
      >
        {{ message.message.orderId }}
      </span>
      <span>placed</span>
      <span v-if="message.message.retrySuccessful" class="success">
        (After successful retry of PlaceOrder message)
      </span>
    </template>
    <template v-else>
      <span>Received order request</span>
      <span
        class="coloured"
        :class="store.selectedMessage === message.message.orderId && 'selected'"
      >
        {{ message.message.orderId }}
      </span>
      <span>containing</span>
      <span class="coloured">{{ message.message.contents.join(", ") }}</span>
    </template>
  </MessageContainer>
</template>

<style scoped>
.endpoint-header {
  margin-top: 0.5em;
  display: flex;
  justify-content: space-between;
}
</style>
