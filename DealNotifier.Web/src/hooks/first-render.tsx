import { useRef, useEffect } from 'react';

function useFirstRender() {
    const isFirstRender = useRef(true);

    useEffect(() => {
        isFirstRender.current = false;
    }, []); 

    return isFirstRender.current;
}

export default useFirstRender;
