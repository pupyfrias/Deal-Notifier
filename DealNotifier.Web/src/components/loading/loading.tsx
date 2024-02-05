import { useLoading } from '@/hooks';
import { FC, useEffect } from 'react';
import './loading.css'

const Spinner: FC = () => {
  return (
    <div className='loading-container'>
      <div className='loading-spinner'>
        <div className='spinner'>
          <div>
            <div></div>
          </div>
          <div>
            <div></div>
          </div>
          <div>
            <div></div>
          </div>
          <div>
            <div></div>
          </div>
          <div>
            <div></div>
          </div>
          <div>
            <div></div>
          </div>
          <div>
            <div></div>
          </div>
          <div>
            <div></div>
          </div>
        </div>
      </div>
    </div>
  );
};



const Loading: FC = () => {
  const { isLoading } = useLoading();
  useEffect(() => {
    if (isLoading) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = 'auto';
    }
  }, [isLoading]);

  return isLoading ? <Spinner /> : null;
};

export default Loading;
