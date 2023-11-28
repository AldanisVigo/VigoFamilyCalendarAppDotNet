import { useState } from "react"
import './child_registration.css'
import { useNavigate } from "react-router-dom"

const ChildRegistration = () => {
    const [childCode,setChildCode] = useState<string | number | readonly string[] | undefined>()
    const navigate = useNavigate()
    const attemptRegister = () => {

    }

    return <form className="space-y-6">
        <div className="child_registration_main">
            <div className="text-4xl font-extrabold dark:text-dark">Children</div>
            <br/>
            <div>
                <p>
                    Have you received a child code from one of your parents? Enter it below to begin your registration.
                </p>
                {/* <label htmlFor="fullname" className="text-center block text-sm font-medium leading-6 text-gray-900">Family Code</label> */}
                <div className="mt-2">
                    <input value={childCode} onChange={e=>setChildCode(e.target.value)} placeholder="Child Code" id="childcode" name="childcode" type="text" autoComplete="text" required className="block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6 text-center"/>
                </div>
                <br/>
                <div>
                    <button onClick={attemptRegister} type="submit" className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">Register</button>
                </div>
                <br/>
                <button onClick={()=>{navigate("/")}} type="button">Cancel and Return to Login</button>
            </div>
        </div>
    </form>
}

export default ChildRegistration