<script setup lang="ts">
import { onMounted, ref } from "vue";
import { HubConnectionBuilder } from "@microsoft/signalr";

var connection = new HubConnectionBuilder()
  .withUrl("http://localhost:5001/salesHub")
  .build();

const failureRate = ref(0);
const processingTime = ref(0.0);
const processedCount = ref(0);
const erroredCount = ref(0);

connection.on("FailureRateChanged", function (newValue) {
  failureRate.value = newValue;
});

connection.on("ProcessingTimeChanged", function (newValue) {
  processingTime.value = newValue;
});

connection.on(
  "OrderPlaced",
  (currentCount) => (processedCount.value = currentCount)
);

connection.on("MessagesProcessed", (processed, errored) => {
  processedCount.value = processed;
  erroredCount.value = errored;
});

onMounted(async () => {
  await connection.start();
  await connection.invoke("ClientConnected");
});

async function changeBillingRateUp() {
  await connection.invoke("IncreaseFailureRate");
}

async function changeBillingRateDown() {
  await connection.invoke("DecreaseFailureRate");
}

async function changeProcessingTimeUp() {
  await connection.invoke("IncreaseProcessingTime");
}

async function changeProcessingTimeDown() {
  await connection.invoke("DecreaseProcessingTime");
}
</script>

<template>
  <!-- TODO: make this into a RateChange control -->
  <div class="valueChangeControl">
    <label>Sales Endpoint Failure Rate:</label>
    <button type="button" @click="changeBillingRateDown">-</button>
    <div>{{ failureRate }}%</div>
    <button type="button" @click="changeBillingRateUp">+</button>
  </div>
  <div class="withCount">
    <div class="valueChangeControl">
      <label>Sales Endpoint Processing Time:</label>
      <button type="button" @click="changeProcessingTimeUp">-</button>
      <div>{{ processingTime }} seconds</div>
      <button type="button" @click="changeProcessingTimeDown">+</button>
    </div>
    <span>
      {{ processedCount }} messages processed / {{ erroredCount }} errored
    </span>
  </div>
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
</style>
