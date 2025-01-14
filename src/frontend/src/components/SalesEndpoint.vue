<script setup lang="ts">
import useSignalR from "../composables/useSignalR";
import { ref } from "vue";

var { connection, state } = useSignalR("http://localhost:5001/salesHub");

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
  <div>{{ state }}</div>
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
    <div class="counter-info">
      <span>
        {{ processedCount }} messages processed /
        <span class="red"> {{ erroredCount }} errored</span>
      </span>
    </div>
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
