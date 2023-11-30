import './header.css'
import logo from '../assets/logo.svg';
import { useState, useContext } from 'react';
import AppContext from '../context/AppContext';
import { useLocation, useNavigate } from 'react-router-dom';
const Header = () => {
    const { token,setToken } = useContext(AppContext)
    const [appTitle] = useState("Synergy")
    const navigate = useNavigate()
    const location = useLocation();

    const signOut = () => {
        if(setToken){
            // Remove the token from localstorage
            localStorage.removeItem('token')
            // And from the app context
            setToken(null)
            navigate("/login")
        }
    }

    return <div className="header min-w-max">
         <img className="h-20 p-4 align-middle inline-block hover:cursor-pointer" src={logo} alt="Synergy Logo" onClick={()=>navigate('/')}/>
         <div className="p-0 align-middle text-center text-lg inline-block lg:text-2xl font-bold leading-9 tracking-tight text-white md:text-lg hover:cursor-pointer" onClick={()=>navigate('/')}>{appTitle}</div>
         {token && <div className="logoutButton inline-block float-right mt-5 mr-5 text-white font-bold">
            <button className="inline-block bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={signOut} title="Logout">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6 inline-block">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 9V5.25A2.25 2.25 0 0013.5 3h-6a2.25 2.25 0 00-2.25 2.25v13.5A2.25 2.25 0 007.5 21h6a2.25 2.25 0 002.25-2.25V15M12 9l-3 3m0 0l3 3m-3-3h12.75" />
                </svg>
                &nbsp;
                <span className="hidden">Sign Out</span>
            </button>
         </div>}
         {!token && location.pathname !== '/register' && <div className="logoutButton inline-block float-right mt-5 mr-5 text-white font-bold">
            <button className="inline-block bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={()=>navigate('/register')} title="Logout">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6 inline-block">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M17.982 18.725A7.488 7.488 0 0012 15.75a7.488 7.488 0 00-5.982 2.975m11.963 0a9 9 0 10-11.963 0m11.963 0A8.966 8.966 0 0112 21a8.966 8.966 0 01-5.982-2.275M15 9.75a3 3 0 11-6 0 3 3 0 016 0z" />
                </svg>
                &nbsp;
                <span className="hidden md:inline-block">Register</span>
            </button>
         </div>}
    </div>
    
}

export default Header