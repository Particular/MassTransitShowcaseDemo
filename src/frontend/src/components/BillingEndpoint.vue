<script setup lang="ts">
import { onMounted, ref } from "vue";
import { HubConnectionBuilder } from "@microsoft/signalr";

var connection = new HubConnectionBuilder()
  .withUrl("http://localhost:5001/billingHub")
  .build();

const rate = ref(0);

connection.on("RateChanged", function (newValue) {
  rate.value = newValue;
});

onMounted(async () => {
  await connection.start();
});

async function changeBillingRateUp() {
  await connection.invoke("IncreaseFailureRate");
}

async function changeBillingRateDown() {
  await connection.invoke("DecreaseFailureRate");
}
</script>

<template>
  <!-- TODO: make this into a RateChange control -->
  <div class="percentChangeControl">
    <label>Billing Endpoint Simulated Failure Rate:</label>
    <button type="button" @click="changeBillingRateDown">-</button>
    <div>{{ rate }}%</div>
    <button type="button" @click="changeBillingRateUp">+</button>
  </div>
</template>

<style scoped>
.percentChangeControl {
  display: flex;
  gap: 0.25em;
}
</style>
