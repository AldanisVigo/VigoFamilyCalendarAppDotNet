import { useState, createContext, useEffect} from "react"

type AppContextData = {
    token : string | undefined | null,
    setToken : Function | null
}

interface AppContextProviderChildren {
    children: JSX.Element | JSX.Element[]
}

const AppContext = createContext<AppContextData>({
    token : '',
    setToken : null
})

export const AppContextProvider = ({children}:AppContextProviderChildren) => {
    const [token,setToken] = useState<string|null|undefined>('')

    useEffect(()=>{
        if(!token){ // if the token is not set
            // Let's check localstorage for it
            const t = localStorage.getItem('token')
            if(t){
                setToken(t)
            }
        }   
    },[token,setToken])
    return <AppContext.Provider  value={{token,setToken}}>
        {Array.isArray(children) ? {...children} : children}
    </AppContext.Provider>
}

export default AppContext