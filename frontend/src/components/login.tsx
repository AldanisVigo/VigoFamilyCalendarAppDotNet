import logo from '../assets/logo.svg';
import { useState } from 'react';
import AppContext from '../context/AppContext';
import { useContext } from 'react'
import LogoutButton from './logoutbutton';

const Login = () => {
  const [appTitle] = useState<string|null>("Vigo Family Calendar v1.0")
  const [username,setUsername] = useState<string | number | readonly string[] | undefined>("")
  const [password,setPassword] = useState<string | number | readonly string[] | undefined>("")
  const {setToken, token} = useContext(AppContext);

  const attemptLogin = async (e:any) => {
    e.preventDefault();

    if(!username || !password){
      window.alert("Please include a username and password before attempting to log in.")
    }else{
      try {
        const request = await fetch(`https://localhost:7283/Users/Login`,{
          method : 'POST',
          headers : {
            "Content-Type" : "application/json"
          },
          body: JSON.stringify({
              "Username" : username, // Include the username 
              "Password" : password// And the password
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
      }catch(err){
        console.log("Error while attempting to log in")
        console.log(err)
      }
    }
  }

  return !token ? <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
    <div className="sm:mx-auto sm:w-full sm:max-w-sm">
      <img className="mx-auto h-50 w-auto" src={logo} alt="Your Company"/>
      <h2 className="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-gray-900">{appTitle}</h2>
    </div>
  
    <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
      <form className="space-y-6">
        <div>
          <label htmlFor="email" className="text-start block text-sm font-medium leading-6 text-gray-900">Email address</label>
          <div className="mt-2">
            <input value={username} onChange={e=>setUsername(e.target.value)} id="username" name="username" type="text" autoComplete="text" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
          </div>
        </div>
  
        <div>
          <div className="flex items-center justify-between">
            <label htmlFor="password" className="block text-sm font-medium leading-6 text-gray-900">Password</label>
            <div className="text-sm">
              <a href="#" className="font-semibold text-indigo-600 hover:text-indigo-500">Forgot password?</a>
            </div>
          </div>
          <div className="mt-2">
            <input value={password} onChange={e=>setPassword(e.target.value)} id="password" name="password" type="password" autoComplete="current-password" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"/>
          </div>
        </div>
  
        <div>
          <button onClick={attemptLogin} type="submit" className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">Sign in</button>
        </div>
      </form>
      <p className="mt-10 text-center text-sm text-gray-500">
          Family Member?&nbsp;
          <a href="#" className="font-semibold leading-6 text-indigo-600 hover:text-indigo-500">
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