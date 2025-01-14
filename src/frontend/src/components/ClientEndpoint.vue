<script setup lang="ts">
import { ref } from "vue";
import { GA4 } from "../utils/analytics";
import useSignalR from "../composables/useSignalR";
import EndpointHeader from "./EndpointHeader.vue";

var { connection, state } = useSignalR("http://localhost:5000/clientHub");

const rate = ref(0);
const orderCount = ref(0);

connection.on("RateChanged", (newValue) => (rate.value = newValue));
connection.on(
  "OrderPlaced",
  (currentCount) => (orderCount.value = currentCount)
);

async function createOrder() {
  try {
    GA4.showcaseMessageSent();
  } catch (e) {
    console.error(e);
  }
  await connection.invoke("CreateOrder");
}
</script>

<template>
  <EndpointHeader label="Customer Order Client" :state="state" />
  <div class="withCount">
    <div class="valueChangeControl">
      <button type="button" @click="createOrder">Create Customer Order</button>
    </div>
    <div class="counter-info">
      <span>{{ orderCount }} orders sent</span>
    </div>
  </div>
</template>

<style scoped>
.withCount {
  display: flex;
  gap: 0.5em;
}

.valueChangeControl {
  display: flex;
  gap: 0.25em;
}
</style>
