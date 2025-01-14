<script setup lang="ts">
import { ref } from "vue";
import { GA4 } from "../utils/analytics";
import useSignalR from "../composables/useSignalR";
import EndpointHeader from "./EndpointHeader.vue";

var { connection, state } = useSignalR("http://localhost:5000/clientHub");

const orderCount = ref(0);
const messages = ref<string[]>([]);
const requestCount = ref(1);
const requestFailureEndpoint = ref("");
const failureEndpointNames = ref<string[]>([]);

connection.on("OrderRequested", (order, currentCount) => {
  orderCount.value = currentCount ?? 0;
  if (order) {
    var date = new Date();
    messages.value = [
      `${date.toLocaleDateString()} ${date.toLocaleTimeString()} Order ${
        order.orderId
      } requested for ${order.contents}`,
      ...messages.value,
    ].slice(0, Math.max(messages.value.length, 100));
  }
});

connection.on("Initialise", (currentCount, endpointNames) => {
  orderCount.value = currentCount ?? 0;
  failureEndpointNames.value = endpointNames;
  requestFailureEndpoint.value =
    requestFailureEndpoint.value || endpointNames[0];
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
    requestFailureEndpoint.value
  );
}
</script>

<template>
  <EndpointHeader label="Customer Order Client" :state="state" />
  <div class="withCount">
    <span>Request</span>
    <input class="requestCount" type="number" v-model.number="requestCount" />
    <span>orders, failing on Endpoint</span>
    <select v-model="requestFailureEndpoint">
      <option
        v-for="endpointName in failureEndpointNames"
        :value="endpointName"
      >
        {{ endpointName }}
      </option>
    </select>
    <div class="valueChangeControl">
      <button type="button" @click="createOrder">Request Order(s)</button>
    </div>
    <div class="counter-info">
      <span>{{ orderCount }} orders sent</span>
    </div>
  </div>
  <textarea rows="3" readonly v-text="messages.join('\n')" />
</template>

<style scoped>
.withCount {
  display: flex;
  gap: 0.5em;
  align-items: baseline;
}

.requestCount {
  width: 3em;
}

.valueChangeControl {
  display: flex;
  gap: 0.25em;
}

textarea {
  margin-top: 0.5em;
  width: 100%;
}
</style>
