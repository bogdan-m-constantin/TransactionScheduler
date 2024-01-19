export interface Product {
  id: number,
  name: string,
  description: string,
  stock: number,
  price: number,
  image: string
}
export interface ProductStockChange {
  productId: number,
  stock: number,
  timestamp: string
}


export interface ProductPriceChange {
  productId: number,
  price: number,
  timestamp: string
}
export interface Client {
  id: number,
  firstName: string,
  lastName: string,
  personalCode: string,
  idNumber: string,
  dateOfBirth: string,
  ammountOfPoints: number,
}

export interface Order {
  id: number;
  client: number;
  timestamp: string,
  items: OrderItem[],
  total: number
}
export interface OrderItem {
  id: number;
  productId: number
  productName: string
  price: number,
  quantity: number,
  image: string,
}
