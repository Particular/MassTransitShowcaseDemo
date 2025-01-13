<script setup lang="ts">
import BillingEndpoint from "./components/BillingEndpoint.vue";
import ClientEndpoint from "./components/ClientEndpoint.vue";
</script>

<template>
  <div>
    <h2>Simulating customers placing orders on a website</h2>
  </div>
  <ClientEndpoint />
  <BillingEndpoint />
</template>
