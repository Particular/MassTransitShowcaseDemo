<script setup lang="ts">
import { ref } from "vue";
import { GA4 } from "../utils/analytics";
import useSignalR from "../composables/useSignalR";
import EndpointHeader from "./EndpointHeader.vue";
import { store } from "./shared";
import type { PlaceOrder, Message } from "./types";
import MessageContainer from "./MessageContainer.vue";

const { connection, state } = useSignalR(`http://${import.meta.env.VITE_CLIENT_SIGNALR ?? "localhost:5000"}/clientHub`);

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
    GA4.createOrderEvent();
  } catch (e) {
    console.error(e);
  }
  await connection.invoke(
    "CreateOrder",
    requestCount.value,
    requestFailureConsumer.value
  );
}

async function runScenario() {
  try {
    GA4.runScenario();
  } catch (e) {
    console.error(e);
  }
  await connection.invoke("RunScenario");
}
</script>

<template>
  <div class="endpoint-header">
    <EndpointHeader label="Customer Order Client" :state="state" />
  </div>
  <div>
    <label
      >Run a scenario with multiple failures occuring in each consumer
    </label>
    <div class="inline">
      <button type="button" class="run-scenario" @click="runScenario">
        Run Scenario
      </button>
      <a target="_blank" href="http://localhost:9090/#/failed-messages/">
        <button type="button" class="secondary view-servicepulse">
          View Failures in ServicePulse
        </button>
      </a>
    </div>
  </div>
  <!-- <div class="or-line">
    <hr />
    <span>OR</span>
  </div>
  <div>
    <label>Manually create orders, specifying where they will fail</label>
    <div class="inline">
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
    </div>
  </div> -->
  <!-- <div class="counter-info">
    <span>{{ orderCount }} total orders sent</span>
  </div> -->
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
.endpoint-header {
  margin-top: 0.5em;
}

.inline {
  display: flex;
  gap: 0.5em;
  align-items: baseline;
  flex-wrap: wrap;
}

.requestCount {
  width: 3em;
}

.or-line {
  width: 50%;
  height: 2em;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.or-line > * {
  position: absolute;
  text-align: center;
  flex: 1;
}

.or-line > hr {
  width: 100%;
}

.or-line > span {
  background-color: white;
  padding: 0 0.5em;
}
</style>
