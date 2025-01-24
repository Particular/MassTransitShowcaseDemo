<script setup lang="ts">
import useSignalR from "../composables/useSignalR";
import { ref } from "vue";
import EndpointHeader from "./EndpointHeader.vue";
import {
  isError,
  isOrderPlaced,
  OrderBilled,
  OrderPlaced,
  type MessageOrError,
  type Order,
} from "./types";
import MessageContainer from "./MessageContainer.vue";
import { store } from "./shared";
import { GA4 } from "../utils/analytics";

const { connection, state } = useSignalR(`http://${import.meta.env.VITE_SHIPPING_SIGNALR ?? "localhost:5003"}/shippingHub`);

const processedOrderPlacedCount = ref(0);
const processedOrderBilledCount = ref(0);
const erroredOrderPlacedCount = ref(0);
const erroredOrderBilledCount = ref(0);
const shouldFailRetries = ref(false);
const messages = ref<MessageOrError[]>([]);

connection.on("ProcessingOrderPlacedMessage", (order: Order) => {
  if (order) {
    messages.value = [
      { timestamp: new Date(), message: new OrderPlaced(order) },
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});
connection.on("ProcessingOrderBilledMessage", (order: Order) => {
  if (order) {
    messages.value = [
      { timestamp: new Date(), message: new OrderBilled(order) },
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
connection.on(
  "SyncValues",
  (
    processedOrderPlaced,
    erroredOrderPlaced,
    processedOrderBilled,
    erroredOrderBilled,
    failRetries
  ) => {
    processedOrderPlacedCount.value = processedOrderPlaced;
    erroredOrderPlacedCount.value = erroredOrderPlaced;
    processedOrderBilledCount.value = processedOrderBilled;
    erroredOrderBilledCount.value = erroredOrderBilled;
    shouldFailRetries.value = failRetries;
  }
);
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
      <EndpointHeader label="Shipping" :state="state" />
      <!-- <div class="counter-info">
        <span>
          {{ processedOrderPlacedCount }} order placed messages processed /
          <span class="red"> {{ erroredOrderPlacedCount }} errored</span>
        </span>
      </div>
      <div class="counter-info">
        <span>
          {{ processedOrderBilledCount }} order billed messages processed /
          <span class="red"> {{ erroredOrderBilledCount }} errored</span>
        </span>
      </div> -->
    </div>
    <!-- <div>
      <OnOffSwitch
        id="failOnRetriesShipping"
        label="Fail Retries"
        @toggle="toggleFailOnRetries"
        :value="shouldFailRetries"
      />
    </div> -->
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
      >
        View failure in ServicePulse
      </a>
    </template>
    <template v-else>
      <span v-if="isOrderPlaced(message.message)"
        >Received order placed message</span
      >
      <span v-else>Received order billed message</span>
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
  margin-top: 1em;
  display: flex;
  justify-content: space-between;
}
</style>
