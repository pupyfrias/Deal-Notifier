import { ApiResponse, Item, PagedCollection } from '@/models';
import axios from 'axios';
const env = import.meta.env;

const ITEM_URL = `${env.VITE_DEAL_NOTIFIER_BASE_URL}/items`;

export const getItems = async (searchParams: URLSearchParams): Promise<ApiResponse<PagedCollection<Item>>> => {
  try {
    const url = searchParams.size > 0 ? `${ITEM_URL}?${searchParams.toString()}` : ITEM_URL;
    const response = await axios.get<ApiResponse<PagedCollection<Item>>>(url);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error('Error en la petición:', error.response);
      throw error;
    } else {
      console.error('Error desconocido:', error);
      throw new Error('Un error desconocido ocurrió durante la petición HTTP');
    }
  }
};

export const bulkDeleteItems = async (ids: number[]) => {
  const url = `${ITEM_URL}/bulk`;
  await axios.delete(url, { data: ids });
};
