import { ApiResponse, ItemType, PagedCollection } from '@/models';
import axios from 'axios';

const env = import.meta.env;
const ITEM_TYPE_URL = `${env.VITE_DEAL_NOTIFIER_BASE_URL}/itemTypes`;

export const getItemTypes = async (): Promise<ApiResponse<PagedCollection<ItemType>>> => {
  const response = await axios.get<ApiResponse<PagedCollection<ItemType>>>(ITEM_TYPE_URL);
  return response.data;
};
