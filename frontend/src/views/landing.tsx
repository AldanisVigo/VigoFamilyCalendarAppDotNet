import Background from "../components/background"
import Header from "../components/header"
import { useNavigate } from "react-router-dom"
import '../App.css'
const Landing = () => {
    const navigate = useNavigate()

    return <>
        <Header/>
        <Background/>
        <div className="p-0 m-0">
           
            <div className="flex flex-col md:flex-row justify-center h-full">
                <video src="intro.mov" autoPlay muted loop className="absolute left-0 top-0 w-screen h-screen object-cover mt-auto justify-self-center hidden xl:block z-0"/>
            </div>
            <div className="mt-0 z-2 w-screen relative">
                <div className="flex w-screen h-screen justify-end items-end text-white pb-20 pr-8 lg:pr-20  ">
                    <span className="text-xl font-bold">Already have an account?</span>
                    {/* <button className="inline-block ml-2 text-xl font-bold">Login</button> */}
                    <button onClick={()=>navigate('/login')} type="submit" className="flex w-auto ml-3 justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">Sign in</button>
                </div>
            </div>
        </div>
    </>
}

export default Landing