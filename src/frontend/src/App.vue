<script setup lang="ts">
import BillingEndpoint from "./components/BillingEndpoint.vue";
import ClientEndpoint from "./components/ClientEndpoint.vue";
import SalesEndpoint from "./components/SalesEndpoint.vue";
import ShippingEndpoint from "./components/ShippingEndpoint.vue";
</script>

<template>
  <h1>Mass Transit message retry showcase</h1>
  <h2>Simulating customers placing orders on a website</h2>
  <ClientEndpoint />
  <SalesEndpoint />
  <BillingEndpoint />
  <ShippingEndpoint />
</template>
