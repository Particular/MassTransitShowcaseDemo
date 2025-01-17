<script setup lang="ts">
import BillingEndpoint from "./components/BillingEndpoint.vue";
import ClientEndpoint from "./components/ClientEndpoint.vue";
import SalesEndpoint from "./components/SalesEndpoint.vue";
import ShippingEndpoint from "./components/ShippingEndpoint.vue";
import TryItOut from "./components/guideline/TryItOut.vue";
import FloatingButton from "./components/FloatingButton.vue";
import { ref } from "vue";

const tab = ref("showcase");
</script>

<template>
  <h1>Mass Transit message retry showcase</h1>
  <div class="tabs">
    <h5
      class="tab"
      :class="{ active: tab === 'showcase' }"
      @click="tab = 'showcase'"
    >
      Showcase
    </h5>
    <h5 class="tab" :class="{ active: tab === 'tryit' }" @click="tab = 'tryit'">
      Try retries with my system
    </h5>
  </div>
  <div class="container">
    <div v-show="tab === 'showcase'">
      <h2>Simulating customers placing orders on a website</h2>
      <div>
        <ClientEndpoint />
        <SalesEndpoint />
        <BillingEndpoint />
        <ShippingEndpoint />
      </div>
      <div style="padding-top: 2rem">
        <img src="/src/assets/small-diagram.svg" alt="Particular Software" />
      </div>
    </div>
    <div v-show="tab === 'tryit'"><TryItOut /></div>
    <floating-button text="Still having issues? Ping us" color="#ff5722" />
  </div>
</template>

<style scoped>
@import "./assets/tabs.css";

.tabs {
  font-size: 1.5em;
  display: flex;
  gap: 1em;
  margin: 0.5rem 0;
}

.tabs h5 {
  text-transform: uppercase;
  font-weight: bold;
}

.container {
  flex: 1;
  overflow: auto;
  padding: 0.5em;
}
</style>
