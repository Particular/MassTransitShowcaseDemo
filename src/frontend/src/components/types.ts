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

export interface Message {
  timestamp: Date;
  message: PlaceOrder | OrderPlaced;
}

export function isOrderPlaced(
  message: OrderPlaced | PlaceOrder
): message is OrderPlaced {
  return message.type === "OrderPlaced";
}
