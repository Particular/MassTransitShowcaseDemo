<template>
  <h3 class="bolder">Want to try it in your system?</h3>
  Select your transport:
  <select>
    <option>RabbitMq</option>
    <option>Azure Service Bus</option>
  </select>
  <div style="padding-top: 2rem">
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
  </div>
</template>

<style scoped>
.bolder {
  font-weight: 600;
  font-size: 13px;
  line-height: 16px;
  padding-bottom: 0.5rem;
}
</style>
