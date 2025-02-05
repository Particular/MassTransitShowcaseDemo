<script setup lang="ts">
import BillingEndpoint from "./components/BillingEndpoint.vue";
import ClientEndpoint from "./components/ClientEndpoint.vue";
import SalesEndpoint from "./components/SalesEndpoint.vue";
import ShippingEndpoint from "./components/ShippingEndpoint.vue";
import TryItOut from "./components/guideline/TryItOut.vue";
import FloatingButton from "./components/FloatingButton.vue";
import FloatingContainer from "./components/FloatingContainer.vue";
import { ref } from "vue";
import { store } from "./components/shared";

const tab = ref("showcase");
</script>

<template>
  <h1>MassTransit recoverability Showcase</h1>
  <div class="tabs">
    <h5
      class="tab"
      :class="{ active: tab === 'showcase' }"
      @click="tab = 'showcase'"
    >
      Showcase
    </h5>
    <h5
      class="tab"
      id="try-it"
      :class="{ active: tab === 'tryit' }"
      @click="tab = 'tryit'"
    >
      Try retries with <i>your</i> system
    </h5>
  </div>
  <div class="container">
    <div v-show="tab === 'showcase'">
      <h2>Simulating customers placing orders on a website</h2>
      <div class="architecture-diagram"></div>
      <div class="sections">
        <div><ClientEndpoint /></div>
        <div><SalesEndpoint /></div>
        <div><BillingEndpoint /></div>
        <div><ShippingEndpoint /></div>
      </div>
    </div>
    <div v-show="tab === 'tryit'"><TryItOut /></div>
    <floating-container>
      <floating-button
        v-if="store.messageRetried"
        text="Thanks for trying our showcase. Get your free eBook now!"
        location="https://gleam.io/ViON2/manning-ebook-giveaway"
        color="green"
      />
      <floating-button
        text="Any issues? Ping us"
        location="https://discuss.particular.net/tag/masstransit"
      />
    </floating-container>
  </div>
</template>

<style scoped>
@import "./assets/tabs.css";

.architecture-diagram {
  background-image: url("./assets/data_flow.png");
  height: 7.9rem;
  width: 18rem;
  position: fixed;
  background-size: contain;
  top: 1rem;
  right: calc(50% - 640px + 7em);
}

.container {
  flex: 1;
  overflow: auto;
  padding: 0.5em;
  display: flex;
  flex-direction: column;
}

.container > div {
  min-height: 0;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.sections > div {
  border-top: 1px solid #2c3e50;
  border-bottom: 1px solid #2c3e50;
  border-radius: 1em;
  margin: 0 -0.5em;
  padding: 0 0.5em;
  margin-bottom: 1em;
}
</style>
