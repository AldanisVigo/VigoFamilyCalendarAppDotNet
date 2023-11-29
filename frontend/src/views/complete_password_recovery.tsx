import Background from "../components/background"
import Header from "../components/header"
import { useEffect, useState } from 'react'
import { useSearchParams, useNavigate } from "react-router-dom"
import '../App.css'

const CompletePasswordRecovery = () => {
    const [searchParams] = useSearchParams();
    const [recoveryCode, setRecoveryCode] = useState<string | null>();
    const [newPassword,setNewPassword] = useState<string | number | readonly string[] | undefined>("");
    const [newPasswordConfirmation,setNewPasswordConfirmation] = useState<string | number | readonly string[] | undefined>("");
    const navigate = useNavigate();

    useEffect(()=>{
        const code = searchParams.get("recoveryCode")
        setRecoveryCode(code);
    })

    const changePassword = async () => {
        if(newPassword == newPasswordConfirmation){
            const request = await fetch(`https://localhost:7283/Users/RecoveryChangePassword`,{
                method : 'POST',
                headers : {
                    "Content-Type" : "application/json"
                },
                body : JSON.stringify({
                    Code : recoveryCode,
                    NewPassword : newPassword,
                    NewPasswordConfirmation : newPasswordConfirmation
                })
            }) 
            
            const response = await request.json();

            console.log(response);

            if('error' in response){
                window.alert(response.error)
            }else if('success' in response){
                window.alert(response.success)
                setTimeout(()=>{
                    navigate("/")
                },2000);
            }
        }else{
            window.alert("The provided passwords don't match.")
        }
    }

    return <>
        <Header/>
        <Background/>
        <div className="main">
            <div className="flex flex-col md:flex-row justify-center gap-3 mt-40 lg:mt-0 sm:mb-10 m-4 items-center  mb-40">
                <div className="parent_registration z-0 text-white">
                    Recovery Code: {recoveryCode}
                    <input type="password" className="text p-4 rounded w-full mt-2 mb-2 text-black" placeholder="New Password" value={newPassword} onChange={e=>setNewPassword(e.target.value)}/>
                    <br/>
                    <input type="password" className="text p-4 rounded w-full mb-2 text-black" placeholder="Confirm New Password" value={newPasswordConfirmation} onChange={e=>setNewPasswordConfirmation(e.target.value)}/>
                    <button className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600" onClick={changePassword}>Change Password</button>
                </div>
            </div>
        </div>
    </>
}

export default CompletePasswordRecovery