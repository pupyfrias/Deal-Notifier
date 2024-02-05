import { ApiResponse, LoginRequest, LoginResponse } from '@/models';
import axios from 'axios';

const env = import.meta.env;
const ACCOUNT_URL = `${env.VITE_DEAL_NOTIFIER_BASE_URL}/account`;

export const login = async (body: LoginRequest): Promise<ApiResponse<LoginResponse>> => {
  const response = await axios.post<ApiResponse<LoginResponse>>(`${ACCOUNT_URL}/login`, body);
  return response.data;
};
