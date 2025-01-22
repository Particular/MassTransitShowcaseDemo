<script setup lang="ts">
import { onMounted, ref } from "vue";
import VueMarkdown from "vue-markdown-render";

const rabbitMarkdown = ref("");
const azureMarkdown = ref("");
const selection = ref("rabbit");

onMounted(async () => {
  const rabbitFile = await fetch("./rabbit.md");
  const azureFile = await fetch("./azure.md");
  rabbitMarkdown.value = await rabbitFile.text();
  azureMarkdown.value = await azureFile.text();
});
</script>

<template>
  <div class="try-it-out">
    <h3 class="bolder">Want to try it in your system?</h3>
    <div>
      Select your transport:
      <select v-model="selection">
        <option value="rabbit">RabbitMq</option>
        <option value="azure">Azure Service Bus</option>
      </select>
    </div>
    <div class="markdown-container">
      <VueMarkdown
        :source="selection === 'rabbit' ? rabbitMarkdown : azureMarkdown"
      />
    </div>
    <!-- <div style="padding-top: 2rem">
    <ul>
      <li>Open the .env file corresponding to your transport</li>
      <li>
        Copy the file content and modify the right values with info from
        <a hre="#" target="_blank"> here.</a>
      </li>
      <li>Configure the error queues</li>
      <li>Run `docker compose down` and `docker compose up</li>
    </ul>
    <br />
    <div>
      <span class="bolder">Ran into issues?</span>
      <div>
        Check out this
        <a
          href="https://github.com/Particular/MassTransitShowcaseDemo/blob/main/troubleshooting-guide.md"
          target="_blank"
        >
          troubleshooting guide</a
        >
      </div>
    </div>
  </div> -->
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

.markdown-container {
  margin: 0.5em 0;
  padding: 0.5em;
  width: 100%;
  border-radius: 0.5em;
  box-shadow: 0 2px 10px #2c3e50;
  overflow: auto;
  flex: 1;
}

:deep(p) {
  margin: 0.5em 0;
}
</style>
