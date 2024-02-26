
import { useLoading, useToast } from "@/hooks";
import axios from "axios";
import { FC, useEffect } from "react";

const AxiosSetup: FC = () => {
    const { setIsLoading } = useLoading();
    const { showToast} = useToast();
  
    useEffect(() => {
      const requestInterceptor = axios.interceptors.request.use((request) => {
        setIsLoading(true);
        return request;
      });
  
      const responseInterceptor = axios.interceptors.response.use(
        async (response) => {  
          setIsLoading(false);
          return response;
        },
        (error) => {
          setIsLoading(false);
          showToast(error.response?.data?.message, 'error')
          return Promise.reject(error);
        }
      );
  
      return () => {
        axios.interceptors.request.eject(requestInterceptor);
        axios.interceptors.response.eject(responseInterceptor);
      };
    }, [setIsLoading, showToast]);
  
    return null;
  };


  export default AxiosSetup;