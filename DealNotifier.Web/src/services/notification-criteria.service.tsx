import { ApiResponse, NotificationCriteria } from '@/models';
import axios from 'axios';

const env = import.meta.env;
const NOTIFICATION_CRITERIA_URL = `${env.VITE_DEAL_NOTIFIER_BASE_URL}/NotificationCriteria`;

export const getNotificationCriteria = async (): Promise<ApiResponse<NotificationCriteria>> => {
  const response = await axios.get<ApiResponse<NotificationCriteria>>(NOTIFICATION_CRITERIA_URL);
  return response.data;
};
