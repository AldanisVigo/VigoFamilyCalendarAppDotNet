import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import './parent_registration.css'
import CustomModalDialog from '../components/custom_modal_dialog'

const ParentRegistration = () => {
    // const {token} = useContext(AppContext);
    const [fullname,setFullName] = useState<string | number | readonly string[] | undefined>("")
    const [email,setEmail] = useState<string | number | readonly string[] | undefined>("")
    const [username,setUsername] = useState<string | number | readonly string[] | undefined>("")
    const [password,setPassword] = useState<string | number | readonly string[] | undefined>("")
    const [passwordConf,setPasswordConf] = useState<string | number | readonly string[] | undefined>("")
    const [showDialog,setShowDialog] = useState<boolean>(false)
    const [dialogTitle,setDialogTitle] = useState<string | undefined | null>("")
    const [dialogContent,setDialogContent] = useState<string | undefined | null>()

    const navigate = useNavigate();

    const attemptRegister = async (e:any) => {
        e.preventDefault();
        try{
            const request = await fetch(`https://localhost:7283/Users/RegisterNewUser`,{
                method : 'POST',
                headers : {
                    "Content-Type" : "application/json",
                    // "Authorization" : `Bearer ${token}`
                },
                body: JSON.stringify({
                    "username" : username, // Include the username 
                    "password" : password, // And the password
                    "passwordConfirmation" : passwordConf,
                    "FullName" : fullname,
                    "email" : email, 
                    "registrationDate" : (new Date()).toISOString(),
                    "userAccountType": 0
                })
            })

            const response = await request.json();

            // If we get an error in the response
            if('error' in response){
                // Show the error message
                setDialogTitle("Error Found")
                setDialogContent(response.error)
                setShowDialog(true)
                // But if we get a success in the response
            }else if('success' in response){
                // Show the success message
                setDialogTitle("Success")
                setDialogContent(response.success)
                setShowDialog(true)

                //Wait four seconds and send them to login page
                setTimeout(()=>{
                    navigate('/')
                },4000)
            }else if('errors' in response){
                const msg = response.errors[''].join(',')
                setDialogTitle("Errors Found")
                setDialogContent(msg)
                setShowDialog(true)
            }

            console.log(response)
        }catch(err:any){
            console.log(err['errors'][''])
        }
    }

    return <div className="parent_registration_main">
        <CustomModalDialog content={dialogContent} title={dialogTitle} showModal={showDialog} setShowModal={setShowDialog} className="z-1"/>

        <form className="space-y-6">
            <div className="text-4xl font-extrabold dark:text-dark">Parents</div>
            <p>
                Create a parent account. Once logged in you'll be able to create child accounts and generate access codes for them.
            </p>

            <div>
                <label htmlFor="fullname" className="text-start block text-sm font-medium leading-6 text-gray-900">Full Name</label>
                <div className="mt-2">
                    <input value={fullname} onChange={e=>setFullName(e.target.value)} placeholder="Full Name" id="fullname" name="fullname" type="text" autoComplete="text" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
                </div>
            </div>

            <div>
                <label htmlFor="username" className="text-start block text-sm font-medium leading-6 text-gray-900">Username</label>
                <div className="mt-2">
                    <input value={username} placeholder="Username" onChange={e=>setUsername(e.target.value)} id="username" name="username" type="text" autoComplete="text" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
                </div>
            </div>

            <div>
                <label htmlFor="email" className="text-start block text-sm font-medium leading-6 text-gray-900">Email</label>
                <div className="mt-2">
                    <input value={email} placeholder="Email Address" onChange={e=>setEmail(e.target.value)} id="email" name="email" type="text" autoComplete="text" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
                </div>
            </div>
    
            <div>
                <div className="flex items-center justify-between">
                    <label htmlFor="password" className="block text-sm font-medium leading-6 text-gray-900">Password</label>
                </div>
                <div className="mt-2">
                    <input value={password} placeholder="Password" onChange={e=>setPassword(e.target.value)} id="password" name="password" type="password" autoComplete="current-password" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
                </div>
            </div>

            <div>
                <div className="flex items-center justify-between">
                    <label htmlFor="password" className="block text-sm font-medium leading-6 text-gray-900">Confirm Password</label>
                </div>
                <div className="mt-2">
                    <input value={passwordConf} placeholder="Password Confirmation" onChange={e=>setPasswordConf(e.target.value)} id="passwordconf" name="passwordconf" type="password" autoComplete="current-password" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
                </div>
            </div>
    
            <div>
                <button onClick={attemptRegister} type="submit" className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">Register</button>
            </div>
        </form>
        <br/>
        <button onClick={()=>{navigate("/")}} type="button">Cancel and Return to Login</button>
    </div>
}

export default ParentRegistration;