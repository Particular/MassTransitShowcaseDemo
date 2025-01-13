<script setup lang="ts">
import { onMounted, ref } from "vue";
import { HubConnectionBuilder } from "@microsoft/signalr";

var connection = new HubConnectionBuilder()
  .withUrl("http://localhost:5002/billingHub")
  .build();

const failureRate = ref(0);

connection.on("FailureRateChanged", function (newValue) {
  failureRate.value = newValue;
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
</script>

<template>
  <!-- TODO: make this into a RateChange control -->
  <div class="valueChangeControl">
    <label>Billing Endpoint Failure Rate:</label>
    <button type="button" @click="changeBillingRateDown">-</button>
    <div>{{ failureRate }}%</div>
    <button type="button" @click="changeBillingRateUp">+</button>
  </div>
</template>

<style scoped>
.valueChangeControl {
  display: flex;
  gap: 0.25em;
}
</style>
