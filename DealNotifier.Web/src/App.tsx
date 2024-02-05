import { Loading, Toast } from '@/components/';
import { AxiosInterceptor } from '@/interceptors';
import { AuthProvider, LoadingProvider, ToastProvider } from '@/providers';
import { Home, Item, Login, NotificationCriteria } from '@/routers';
import { FC } from 'react';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';


const router = createBrowserRouter([
  {
    path: '/',
    element: <Home />,
  },
  {
    path: '*',
    element: <h1>Not found</h1>,
  },
  {
    path: '/items/:id',
    element: <Item />,
  },
  {
    path: '/login',
    element: <Login />,
  },
  {
    path: '/notification-criteria',
    element: <NotificationCriteria />,
  },
]);

const App: FC = () => {
  return (
    <LoadingProvider>
      <ToastProvider>
        <AuthProvider>
        <AxiosInterceptor />
        <Loading />
        <Toast />
        <RouterProvider router={router}  />
        </AuthProvider>
      </ToastProvider>
    </LoadingProvider>
  );
};

export default App;
