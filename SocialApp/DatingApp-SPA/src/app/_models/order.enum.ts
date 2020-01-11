export enum Order {
    LastActive,
    Created
}

export function mapStringToOrder(value: string) {
    switch(value) {
        case 'lastActive':
            return Order.LastActive;
        case 'created':
            return Order.Created;
        default:
            throw new Error('Unvalid order string value.');
    }
}

export function mapOrderToString(order: Order) {
    switch(order) {
        case Order.LastActive:
            return 'lastActive';
        case Order.Created:
            return 'created';
        default:
            throw new Error('Unvalid order value.');
    }
}