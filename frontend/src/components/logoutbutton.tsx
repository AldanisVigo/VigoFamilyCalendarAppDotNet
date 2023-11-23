import { useContext } from "react"
import AppContext from "../context/AppContext"

const LogoutButton = () => {
    const {setToken} = useContext(AppContext)

    const logout = () => {
        if(setToken){
            setToken(null)
            localStorage.removeItem('token')
        }
    }

    return <>
        <button onClick={logout}>Logout</button>
    </>
}

export default LogoutButton