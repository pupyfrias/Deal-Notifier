
import axios from 'axios';
const env = import.meta.env;

const BAN_KEYWORD_URL = `${env.VITE_DEAL_NOTIFIER_BASE_URL}/banKeywords`;

export const banKeywords = async (keyword: string) => {
  
  await axios.post(BAN_KEYWORD_URL, { keyword: keyword });
};
