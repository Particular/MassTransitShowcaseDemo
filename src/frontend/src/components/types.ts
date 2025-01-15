export interface Order {
  orderId: string;
  contents: string[];
}

export class PlaceOrder implements Order {
  readonly type = "PlaceOrder";

  orderId: string;
  contents: string[];

  constructor(order: Order) {
    this.orderId = order.orderId;
    this.contents = order.contents;
  }
}

export class OrderPlaced implements Order {
  readonly type = "OrderPlaced";

  orderId: string;
  contents: string[];

  constructor(order: Order) {
    this.orderId = order.orderId;
    this.contents = order.contents;
  }
}

export class OrderBilled implements Order {
  readonly type = "OrderBilled";

  orderId: string;
  contents: string[];

  constructor(order: Order) {
    this.orderId = order.orderId;
    this.contents = order.contents;
  }
}

export interface Message {
  timestamp: Date;
  message: PlaceOrder | OrderPlaced | OrderBilled;
}

export interface ErrorMessage extends Message {
  isError: true;
  messageId: string;
}

export type MessageOrError = Message | ErrorMessage;

export function isError(message: MessageOrError): message is ErrorMessage {
  return (message as ErrorMessage).isError;
}

export function isOrderPlaced(
  message: PlaceOrder | OrderPlaced | OrderBilled
): message is OrderPlaced {
  return message.type === "OrderPlaced";
}

export function isOrderBilled(
  message: PlaceOrder | OrderPlaced | OrderBilled
): message is OrderBilled {
  return message.type === "OrderBilled";
}
