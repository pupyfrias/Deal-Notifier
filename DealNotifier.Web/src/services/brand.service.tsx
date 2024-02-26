import { ApiResponse, Brand, PagedCollection } from '@/models';
import axios from 'axios';

const env = import.meta.env;
const BRAND_URL = `${env.VITE_DEAL_NOTIFIER_BASE_URL}/brands`;

export const getBrands = async (): Promise<ApiResponse<PagedCollection<Brand>>> => {
  const response = await axios.get<ApiResponse<PagedCollection<Brand>>>(BRAND_URL);
  return response.data;
};

