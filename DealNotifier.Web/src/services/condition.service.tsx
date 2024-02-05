import { ApiResponse, Condition, PagedCollection } from '@/models';
import axios from 'axios';

const env = import.meta.env;
const CONDITION_URL = `${env.VITE_DEAL_NOTIFIER_BASE_URL}/conditions`;

export const getConditions = async (): Promise<ApiResponse<PagedCollection<Condition>>> => {
  const response = await axios.get<ApiResponse<PagedCollection<Condition>>>(CONDITION_URL);
  return response.data;
};

