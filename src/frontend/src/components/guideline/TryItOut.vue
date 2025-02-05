<script setup lang="ts"></script>

<template>
  <div class="try-it-out">
    <h3 class="bolder">Want to try it in your system?</h3>
    <ul>
      <li>
        <a
          target="_blank"
          href="https://github.com/Particular/MassTransitShowcaseDemo/blob/main/docs/rabbit.md"
        >
          View instructions for RabbitMQ
        </a>
      </li>
      <li>
        <a
          target="_blank"
          href="https://github.com/Particular/MassTransitShowcaseDemo/blob/main/docs/azure.md"
        >
          View instructions for Azure Service Bus
        </a>
      </li>
    </ul>
  </div>
</template>

<style scoped>
.bolder {
  font-weight: 600;
  font-size: 13px;
  line-height: 16px;
  padding-bottom: 0.5rem;
}

.try-it-out {
  display: flex;
  flex-direction: column;
  flex: 1;
  min-height: 0;
}
</style>
