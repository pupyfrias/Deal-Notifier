import { LoadingContext } from "@/contexts";
import { FC, ReactNode, useState } from "react";

type LoadingProviderProps = {
    children: ReactNode;
  };

const LoadingProvider: FC<LoadingProviderProps> = ({ children }) => {
  const [isLoading, setIsLoading] = useState(false);

  return (
    <LoadingContext.Provider value={{ isLoading, setIsLoading }}>
      {children}
    </LoadingContext.Provider>
  );
};

export default LoadingProvider;