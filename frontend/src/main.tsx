import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { AppContextProvider } from './context/AppContext'
import { createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import Registeration from './views/registration.tsx'
import PasswordRecovery from './views/password_recovery.tsx'
import Login from './views/Login.tsx'


const router = createBrowserRouter([
  {
    path: "/",
    element: <Login/>,
  },
  {
    path : "/register",
    element: <Registeration />
  },
  {
    path : "/recover",
    element : <PasswordRecovery/>
  },
  {
    path : '/home',
    element : <App/>
  }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AppContextProvider>
      <RouterProvider router={router} />
    </AppContextProvider>
  </React.StrictMode>,
)
