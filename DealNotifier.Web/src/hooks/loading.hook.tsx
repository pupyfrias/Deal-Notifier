import { LoadingContext } from "@/contexts";
import { useContext } from "react";

 const useLoading = () => useContext(LoadingContext);

 export default useLoading;