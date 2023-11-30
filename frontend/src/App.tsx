import Background from "./components/background"
import Header from "./components/header"
import './App.css'
import CalendarSelector from "./components/CalendarSelector"
import StatChart from "./components/statchart"
import { Box } from '@mui/material'
import dailyRecurringChoreBg from './assets/parent_view/add_chore_bg.jpeg';
import goingOutRewardBg from './assets/parent_view/going_out_reward_bg.jpeg';
import monetaryRewardBg from './assets/parent_view/monetary_reward_bg.jpeg';
import extendedBedtimeRewardBg from './assets/parent_view/extended_bedtime_reward_bg.jpeg';
import candyRewardBg from './assets/parent_view/candy_reward_bg.jpeg';
import restaurantRewardBg from './assets/parent_view/restaurant_reward_bg.jpeg';
import purchasedItemRewardBg from './assets/parent_view/purchased_item_bg.jpeg';
import toysRewardBg from './assets/parent_view/toys_reward_bg.jpeg';
import mysteryRewardBg from './assets/parent_view/mystery_reward_bg.jpeg';
import addChildBg from './assets/parent_view/add_child_bg.jpeg';
import manageAccountBg from './assets/parent_view/manage_account_bg.jpeg';
import MenuOptionButton from "./components/menu_option_button"
import { useNavigate } from "react-router-dom"

type MenuOption = {
    title : string
    image : string
    open : ()=>void
}

const App = () => {
    const navigate = useNavigate()
    const addChoresOptions = [
        {
            title : "Daily\nRecurring\nChore",
            image : dailyRecurringChoreBg,
            open : () => navigate("/parentaddchore",{
                state:{
                    type: "recurring"
                }
            })
        },
        {
            title : "Single\nOne‑Time\nChore",
            image : dailyRecurringChoreBg,
            open : () => navigate("/parentaddchore",{
                state: {
                    type: "single"
                }
            })
        }
    ]

    const manageAccountOptions = [
        {
            title : "Add\nChild",
            image : addChildBg,
            open : () => navigate("/parentaddchild")
        },
        {
            title : "Manage\nAccounts",
            image : manageAccountBg,
            open : () => navigate("/parentmanageaccounts")
        }
    ]

    const rewardOptions = [
        {
            title : "Going Out\nReward",
            image : goingOutRewardBg,
            open : () => navigate("/parentaddreward",{
                state : {
                    type : "going_out_reward"
                }
            })
        },
        {
            title : "Monetary\nReward",
            image : monetaryRewardBg,
            open : () => navigate("/parentaddreward",{
                state : {
                    type : "monetary_reward"
                }
            })
        },
        {
            title : "Extended\nBedtime\nReward",
            image : extendedBedtimeRewardBg,
            open : () => navigate("/parentaddreward",{
                state : {
                    type : "extended_bedtime_reward"
                }
            })
        },
        {
            title : "Candy\nReward",
            image : candyRewardBg,
            open : () => navigate("/parentaddreward",{
                state : {
                    type : "candy_reward"
                }
            })
        },
        {
            title : "Restaurant\nReward",
            image : restaurantRewardBg,
            open : () => navigate("/parentaddreward",{
                state : {
                    type : "restaurant_reward"
                }
            })
        },
        {
            title : "Purchased\nItem\nReward",
            image : purchasedItemRewardBg,
            open : () => navigate("/parentaddreward",{
                state : {
                    type : "purchased_item_reward"
                }
            })
        },
        {
            title : "New\nToys\nReward",
            image : toysRewardBg,
            open : () => navigate("/parentaddreward",{
                state : {
                    type : "new_toys_reward"
                }
            })
        },
        {
            title : "Mystery\nReward",
            image : mysteryRewardBg,
            open : () => navigate("/parentaddreward",{
                state : {
                    type : "mystery_reward"
                }
            })
        }
    ]

    return <div className="h-screen w-screen overflow-y-scroll">
        <Header/>
        <Background/>
        <div className="flex justify-content-center text-center w-full">
            <div className="overflow-y-scroll h-full max-w-[1140px] min-w-[400px] mx-auto">
                <div className="grid mt-20 z-0 bg-none lg:grid-cols-[1fr_3fr] grid-cols-1">
                    <div className="text-black p-6">
                        <div className="bg-white grid flex-row w-full justify-content-center text-center pt-5 rounded-xl shadow-lg">
                            <span className="font-bold text-3xl block">Calendar</span>
                            <hr className="h-[.5px] bg-black rounded-xl ml-5 mr-5"/>
                            <div style={{maxHeight : 228}} className="overflow-y-scroll rounded-xl">
                                <CalendarSelector/>
                            </div>
                        </div>
                        <div className="bg-white mt-5 rounded-xl text-center p-6 shadow-lg">
                            <span className="font-bold text-3xl block">Status</span>
                            <hr className="h-[.5px] bg-black rounded-xl"/>
                            <div className="flex flex-col justify-content-center items-center">
                                {/* <Container className="h-56 flex"> */}
                                    <span className="font-bold">Chores</span>
                                    <Box className="h-56 w-56">
                                        <StatChart data={[{ label: 'Done', value: 10, color: '#478EB9' },{ label: 'Pending', value: 90, color: '#FF944D' }]}/>
                                    </Box>
                                {/* </Container> */}
                                <Box className="h-56 w-56 mt-8">
                                    <span className="font-bold">Rewards</span>
                                    <StatChart data={[{ label: 'Claimed', value: 25, color: '#478EB9' },{ label: 'Unclaimed', value: 75, color: '#FF944D' }]}/>
                                </Box>
                            </div>
                        </div>
                    </div>

                    <div className="text-center text-black p-6 grid grid-cols-1 grid-rows-[auto_auto] md:grid-rows-[325px_1fr]">
                        <div className="bg-white grid md:grid-cols-2 w-full justify-content-center text-center pt-5 rounded-xl gap-0 shadow-lg grid-cols-1 pb-5">
                            <div>
                                <div className="font-bold text-3xl block text-left pl-5 mb-3">Add Chores</div>
                                <div className="grid grid-cols-2 gap-4 pl-5 pr-5 md:mt-0">
                                    {addChoresOptions.map((option:MenuOption,index:number)=><div onClick={option.open}><MenuOptionButton key={index} option={option}></MenuOptionButton></div>)}
                                </div>
                            </div>
                            <div>
                                <div className="font-bold text-3xl block text-left ml-5 mb-3">Manage Accounts</div>
                                <div className="grid grid-cols-2 gap-4 pl-5 pr-5 md:mt-0">
                                    {manageAccountOptions.map((option:MenuOption, index:number)=><div onClick={option.open}><MenuOptionButton key={index} option={option}></MenuOptionButton></div>)}
                                </div>
                            </div>
                        </div>
                        <div className="w-full bg-white rounded-xl pt-5 mt-5 shadow-lg text-center pb-5">
                            <div className="font-bold text-3xl block text-left pl-5 mb-3">Rewards</div>
                            <div className="grid grid-cols-2 md:grid-cols-4 gap-2 pl-2 pr-5 mt-0 pl-5 ml-0">
                                {rewardOptions.map((option:MenuOption, index:number)=><div onClick={option.open}><MenuOptionButton key={index} option={option}></MenuOptionButton></div>)}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
}

export default App