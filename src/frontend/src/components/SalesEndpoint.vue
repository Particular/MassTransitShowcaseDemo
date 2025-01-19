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
import OnOffSwitch from "./OnOffSwitch.vue";
import { GA4 } from "../utils/analytics";

var { connection, state } = useSignalR("http://localhost:5001/salesHub");

const processedCount = ref(0);
const erroredCount = ref(0);
const shouldFailRetries = ref(false);
const messages = ref<MessageOrError[]>([]);

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
      { timestamp: new Date(), message: new OrderPlaced(order) },
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
    GA4.showcaseRetryAttempted();
  } catch (e) {
    console.error(e);
  }
});

function toggleFailOnRetries() {
  connection.invoke("SetFailRetries", !shouldFailRetries.value);
}
</script>

<template>
  <div class="endpoint-header">
    <div>
      <EndpointHeader label="Sales" :state="state" />
      <div class="counter-info">
        <span>
          {{ processedCount }} messages processed /
          <span class="red"> {{ erroredCount }} errored</span>
        </span>
      </div>
    </div>
    <div>
      <OnOffSwitch
        id="failOnRetriesSales"
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
      <span>containing</span>
      <span class="coloured">{{ message.message.contents.join(", ") }}</span>
      <span>failed.</span>
      <a
        target="_blank"
        :href="`http://localhost:5173/#/failed-messages/message/${message.messageViewId}`"
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
