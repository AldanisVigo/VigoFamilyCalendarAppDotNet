import logo from '../assets/logo.svg';
import { useState } from 'react';
import AppContext from '../context/AppContext';
import { useContext } from 'react'
import LogoutButton from './logoutbutton';
import './login.css'
import CustomModalDialog from './custom_modal_dialog';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  // const [appTitle] = useState<string|null>("Vigo Family Calendar v1.0")
  const [username,setUsername] = useState<string | number | readonly string[] | undefined>("")
  const [password,setPassword] = useState<string | number | readonly string[] | undefined>("")
  const {setToken, token} = useContext(AppContext);
  const [modalTitle,setModalTitle] = useState<string|undefined|null>()
  const [modalContent,setModalContent] = useState<string|undefined|null|any>()
  const [showModal,setShowModal] = useState<boolean>(false);
  const navigate = useNavigate()

  const attemptLogin = async (e:any) => {
    e.preventDefault();

    if(!username || !password){
      // window.alert("Please include a username and password before attempting to log in.")
      setModalTitle("Missing Information")
      setModalContent("Please include a username and password before attempting to log in.")
      setShowModal(true)
    }else{
      try {
        const request = await fetch(`https://localhost:7283/Users/Login`,{
          method : 'POST',
          headers : {
            "Content-Type" : "application/json"
          },
          body: JSON.stringify({
              "Username" : username, // Include the username 
              "Password" : password, // And the password
          })
        })

        console.log(request)

        const response = await request.json();

        console.log(response)

        // Check if we get a JWT token from the login call to the backend.
        if('token' in response){
          console.log("Login was successful.")
          console.log(`JWT Token: ${response.token}`)
          if(setToken != null){
            setToken(response.token)
            localStorage.setItem('token',response.token)

          }else{
            window.alert("setToken is null")
          }
        }

        if('error' in response) {
          setModalTitle("Error Found")
          setModalContent(response.error)
          setShowModal(true)
        }
      }catch(err:any){
        console.log("Error while attempting to log in")
        console.log(err)
        setModalTitle("Error Found")
        setModalContent(err)
        setShowModal(true)
      }
    }
  }

  return !token ? <div className="w-full">
    {/*  */}
    <CustomModalDialog title={modalTitle} content={modalContent} showModal={showModal} setShowModal={setShowModal}/>
    <div className="grid place-content-center z-0">
      <img className="mx-auto w-auto loginLogo z-10" src={logo} alt="Your Company"/>

      <form className="space-y-6 loginForm z-0">
        <div>
          <label htmlFor="username" className="text-start block text-sm font-medium leading-6 text-gray-900">Username</label>
          <div className="mt-2">
            <input value={username} onChange={e=>setUsername(e.target.value)} placeholder="Username" id="username" name="username" type="text" autoComplete="text" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
          </div>
        </div>
  
        <div>
          <div className="flex items-center justify-between">
            <label htmlFor="password" className="block text-sm font-medium leading-6 text-gray-900">Password</label>
            <div className="text-xs">
              <a href="#" className="font-semibold text-indigo-600 hover:text-indigo-500" onClick={()=>navigate('/recover')}>Forgot password?</a>
            </div>
          </div>
          <div className="mt-2">
            <input value={password} onChange={e=>setPassword(e.target.value)} placeholder="Password" id="password" name="password" type="password" autoComplete="current-password" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
          </div>
        </div>
  
        <div>
          <button onClick={attemptLogin} type="submit" className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">Sign in</button>
        </div>
      </form>
      <p className="mt-10 text-center text-sm text-white">
          Family Member?&nbsp;
          <br/>
          <a href="/register" className="font-semibold leading-6 text-grey-600 hover:text-grey">
              Request family access.
          </a>
      </p>
   </div>
 </div> : <div>
  Logged in with token: {token}
  <br/>
  <LogoutButton/>
 </div>
}

export default Login