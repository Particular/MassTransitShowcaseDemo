import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { onMounted, ref } from "vue";

export default function useSignalR(url: string) {
  const state = ref(HubConnectionState.Disconnected);

  var connection = new HubConnectionBuilder()
    .withUrl(url)
    .withAutomaticReconnect()
    .build();

  async function connect() {
    try {
      state.value = HubConnectionState.Connecting;
      await connection.start();
      state.value = connection.state;
    } catch (err) {
      console.error(err);
      state.value = HubConnectionState.Disconnected;
      setTimeout(connect, 5000);
    }
  }

  onMounted(async () => await connect());
  connection.onreconnecting(() => (state.value = connection.state));
  connection.onreconnected(() => (state.value = connection.state));
  connection.onclose(async () => await connect());

  return { connection, state };
}
