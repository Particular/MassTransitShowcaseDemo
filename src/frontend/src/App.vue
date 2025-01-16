<script setup lang="ts">
import BillingEndpoint from "./components/BillingEndpoint.vue";
import ClientEndpoint from "./components/ClientEndpoint.vue";
import SalesEndpoint from "./components/SalesEndpoint.vue";
import ShippingEndpoint from "./components/ShippingEndpoint.vue";
import TryItOut from "./components/guideline/TryItOut.vue";
import FloatingButton from "./components/FloatingButton.vue";
</script>

<template>
  <h1>Mass Transit message retry showcase</h1>
  <h2>Simulating customers placing orders on a website</h2>
  <div class="container">
    <div class="left-section">
      <ClientEndpoint />
      <SalesEndpoint />
      <BillingEndpoint />
      <ShippingEndpoint />
    </div>
    <div class="right-section"><TryItOut /></div>
    <floating-button icon="&#43;" color="#ff5722" />
  </div>
</template>
