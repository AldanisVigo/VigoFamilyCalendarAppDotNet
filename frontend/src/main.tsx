import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { AppContextProvider } from './context/AppContext'
import { createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import Registeration from './views/authentication/registration.tsx'
import PasswordRecovery from './views/authentication/password_recovery.tsx'
import Login from './views/authentication/Login.tsx'
import CompletePasswordRecovery from './views/authentication/complete_password_recovery.tsx'
import Landing from './views/landing.tsx'
import ParentAddChild from './views/parents/parent_add_child.tsx'
import ParentAddChore from './views/parents/parent_add_chore.tsx'
import ParentManageAccounts from './views/parents/parent_manage_accounts.tsx'
import ParentAddReward from './views/parents/parent_add_reward.tsx'

const router = createBrowserRouter([
  {
    path: "/",
    element: <Landing/>,
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
    path : '/complete_recovery',
    element : <CompletePasswordRecovery/>
  },
  {
    path : '/home',
    element : <App/>
  },
  {
    path : '/login',
    element: <Login/>
  },
  {
    path : '/parentaddchild',
    element: <ParentAddChild/>
  },
  {
    path : '/parentaddchore',
    element: <ParentAddChore/>
  },
  {
    path : "/parentmanageaccounts",
    element: <ParentManageAccounts/>
  },
  {
    path : "/parentaddreward",
    element: <ParentAddReward/>
  }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AppContextProvider>
      <RouterProvider router={router} />
    </AppContextProvider>
  </React.StrictMode>,
)
