import { ApiResponse, PagedCollection, UnlockProbability } from '@/models';
import axios from 'axios';


const env = import.meta.env;
const UNLOCK_PROBABILITY_URL = `${env.VITE_DEAL_NOTIFIER_BASE_URL}/unlockProbabilities`;

export const getUnlockProbabilities = async (): Promise<ApiResponse<PagedCollection<UnlockProbability>>> => {
  const response = await axios.get<ApiResponse<PagedCollection<UnlockProbability>>>(UNLOCK_PROBABILITY_URL);
  return response.data;
};

