export interface Item {
  id: number;
  name: string;
  price: number;
  oldPrice: number;
  saving: number;
  link: string;
  conditionId: number;
  status: boolean;
  shopId: number;
  image: string;
  lastModified: Date;
  savingsPercentage: number;
  typeId: number;
}
