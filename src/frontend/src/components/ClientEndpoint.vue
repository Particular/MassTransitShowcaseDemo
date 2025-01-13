<script setup lang="ts">
import { onMounted, ref } from "vue";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { GA4 } from "../utils/analytics";

var connection = new HubConnectionBuilder()
  .withUrl("http://localhost:5000/clientHub")
  .build();

const rate = ref(0);
const orderCount = ref(0);

connection.on("RateChanged", (newValue) => (rate.value = newValue));
connection.on(
  "OrderPlaced",
  (currentCount) => (orderCount.value = currentCount)
);

onMounted(async () => {
  await connection.start();
  await connection.invoke("ClientConnected");
});

async function increaseTraffic() {
  await connection.invoke("IncreaseTraffic");
}

async function decreaseTraffic() {
  GA4.showcaseMessageSent();
  await connection.invoke("DecreaseTraffic");
}
</script>

<template>
  <div class="withCount">
    <!-- TODO: make this into a RateChange control -->
    <div class="valueChangeControl">
      <label>Customer Order Rate:</label>
      <button type="button" @click="decreaseTraffic">-</button>
      <div>{{ rate }} orders / second</div>
      <button type="button" @click="increaseTraffic">+</button>
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
