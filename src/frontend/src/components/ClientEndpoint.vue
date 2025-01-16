<script setup lang="ts">
import { ref } from "vue";
import { GA4 } from "../utils/analytics";
import useSignalR from "../composables/useSignalR";
import EndpointHeader from "./EndpointHeader.vue";
import { store } from "./shared";
import type { PlaceOrder, Message } from "./types";
import MessageContainer from "./MessageContainer.vue";

var { connection, state } = useSignalR("http://localhost:5000/clientHub");

const orderCount = ref(0);
const messages = ref<Message[]>([]);
const requestCount = ref(1);
const requestFailureConsumer = ref("");
const failureEndpointNames = ref<string[]>([]);

connection.on("OrderRequested", (order: PlaceOrder, currentCount: number) => {
  orderCount.value = currentCount ?? 0;
  if (order) {
    messages.value = [
      { timestamp: new Date(), message: order },
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});

connection.on("Initialise", (currentCount, endpointNames) => {
  orderCount.value = currentCount ?? 0;
  failureEndpointNames.value = endpointNames;
  requestFailureConsumer.value =
    requestFailureConsumer.value || endpointNames[0];
});

async function createOrder() {
  try {
    GA4.showcaseMessageSent();
  } catch (e) {
    console.error(e);
  }
  await connection.invoke(
    "CreateOrder",
    requestCount.value,
    requestFailureConsumer.value
  );
}
</script>

<template>
  <EndpointHeader label="Customer Order Client" :state="state" />
  <div class="withCount">
    <span>Request</span>
    <input class="requestCount" type="number" v-model.number="requestCount" />
    <span>order(s), failing on Consumer</span>
    <select v-model="requestFailureConsumer">
      <option
        v-for="endpointName in failureEndpointNames"
        :value="endpointName"
      >
        {{ endpointName }}
      </option>
    </select>
    <button type="button" @click="createOrder">Place Order(s)</button>
    <div class="counter-info">
      <span>{{ orderCount }} total orders sent</span>
    </div>
  </div>
  <MessageContainer :messages="messages" v-slot="{ message }">
    <span>{{ message.timestamp.toLocaleTimeString() }}</span>
    <span>Order</span>
    <span
      class="coloured"
      :class="store.selectedMessage === message.message.orderId && 'selected'"
      >{{ message.message.orderId }}</span
    >
    <span>requested for</span>
    <span class="coloured">{{ message.message.contents.join(", ") }}</span>
  </MessageContainer>
</template>

<style scoped>
.withCount {
  display: flex;
  gap: 0.5em;
  align-items: baseline;
  flex-wrap: wrap;
}

.requestCount {
  width: 3em;
}
</style>
