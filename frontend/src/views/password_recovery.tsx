import { useNavigate } from "react-router-dom";
import Background from "../components/background";
import Header from "../components/header";
import { useState } from 'react'
import '../App.css'
const PasswordRecovery = () => {
    const [forgotUsername,setForgotUsername] = useState(false)
    const [identifier,setIdentifier] = useState('')
    const navigate = useNavigate()
    return <>
        <Header/>
        <Background/>
        <div className="main">
            <div className="flex flex-col md:flex-row justify-center gap-3 mt-40 lg:mt-0 sm:mb-10 m-4 items-center  mb-40">
                <div className="parent_registration z-0">
                    <div className="flex flex-col justify-center items-center w-100">
                        {!forgotUsername && <div className="text-white text-center p-10 mb-4">Please enter your username, we'll send you an email with a recovery link.</div>}
                        {forgotUsername && <div className="text-white text-center p-10 mb-4">Please enter your email instead, we'll send you an email with a recovery link.</div>}
                        <input type="text" className="mb-10 px-4 py-2 leading-tight rounded" placeholder={forgotUsername? "Email Address" : "Username"} value={identifier} onChange={e=>setIdentifier(e.target.value)}/>
                        {!forgotUsername && !identifier && <div>
                            <button className="bg-blue-500 hover:bg-orange-700 mr-4 text-white font-bold py-2 px-4 rounded" onClick={()=>navigate("/")}>Cancel</button>
                            <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={()=>setForgotUsername(true)}>Forgot Username</button>
                        </div>}
                        {forgotUsername && !identifier && <div>
                            <button className="bg-blue-500 hover:bg-orange-700 mr-4 text-white font-bold py-2 px-4 rounded" onClick={()=>navigate("/")}>Cancel</button>
                            <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={()=>alert("You can only recover your account if you know the username or email associated with it. If you can't remember the information please contact Customer Support.")}>Forgot Email</button>
                        </div>}
                        {identifier && <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">Send me Link</button>}
                    </div>
                </div>
            </div>  
        </div>
    </>
}

export default PasswordRecovery;