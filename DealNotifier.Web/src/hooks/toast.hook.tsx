import { ToastContext } from "@/contexts";
import { useContext } from "react";

 const useToast = () => useContext(ToastContext);

 export default useToast;