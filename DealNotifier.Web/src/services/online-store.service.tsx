import { ApiResponse, OnlineStore, PagedCollection } from '@/models';
import axios from 'axios';

const env = import.meta.env;
const ONLINE_STORE_URL = `${env.VITE_DEAL_NOTIFIER_BASE_URL}/onlineStores`;


export const getOnlineStores = async (): Promise<ApiResponse<PagedCollection<OnlineStore>>> => {
  const response = await axios.get<ApiResponse<PagedCollection<OnlineStore>>>(ONLINE_STORE_URL);
  return response.data;
};

