<script setup lang="ts">
import { ref } from "vue";
import { GA4 } from "../utils/analytics";
import useSignalR from "../composables/useSignalR";
import EndpointHeader from "./EndpointHeader.vue";

var { connection, state } = useSignalR("http://localhost:5000/clientHub");

const orderCount = ref(0);
const orders = ref<string[]>([]);

connection.on("OrderPlaced", (order, currentCount) => {
  orderCount.value = currentCount;
  if (order) {
    var date = new Date();
    orders.value = [
      `${date.toLocaleDateString()} ${date.toLocaleTimeString()} Order ${
        order.orderId
      } placed for ${order.contents}`,
      ...orders.value,
    ].slice(0, Math.max(orders.value.length, 100));
  }
});

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
  <textarea rows="3" v-text="orders.join('\n')" />
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

textarea {
  width: 100%;
}
</style>
