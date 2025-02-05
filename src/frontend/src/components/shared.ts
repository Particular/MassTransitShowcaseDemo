import { reactive } from "vue";

export const store = reactive({
  selectedMessage: "",
  setSelectedMessage(messageId: string) {
    this.selectedMessage = messageId;
  },
  clearSelectedMessage() {
    this.selectedMessage = "";
  },
  messageRetried: false,
  setMessageRetried() {
    setTimeout(() => (this.messageRetried = true), 5000);
  },
});
