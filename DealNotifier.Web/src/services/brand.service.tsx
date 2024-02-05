import { ApiResponse, Brand, PagedCollection } from '@/models';
import axios from 'axios';


const DEAL_NOTIFIER_BASE_URL = 'https://localhost:4430/api/v1/';
const BRAND_URL = `${DEAL_NOTIFIER_BASE_URL}brands`;

export const getBrands = async (): Promise<ApiResponse<PagedCollection<Brand>>> => {
  const response = await axios.get<ApiResponse<PagedCollection<Brand>>>(BRAND_URL);
  return response.data;
};

