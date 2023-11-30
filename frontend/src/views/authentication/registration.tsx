import ChildRegistration from "./child_registration";
import ParentRegistration from "./parent_registration";
import "./registration.css"
import Header from "../../components/header";
import Background from "../../components/background";
const Registration = () => {
    return <>
        <Header/>
        <Background/>
        <div className="main">
            <div className="flex flex-col md:flex-row justify-center h-full gap-3 mt-40 lg:mt-0 sm:mb-10 m-4 items-center  mb-40">
                <div className="parent_registration z-0">
                    <ParentRegistration/>
                </div>
                <div className="child_registration z-0">
                    <ChildRegistration/>
                </div>
            </div>  
        </div>
    </>
}

export default Registration;