type MenuOption = {
    title : string
    image : string
}

const MenuOptionButton = ({option}:{option: MenuOption}) => {
    return <div className="relative rounded-xl mx-auto hover:cursor-pointer hover:scale-[1.2] hover:z-[100]">
        <div className="relative fixed left-0 top-0 w-28 h-28 sm:w-40 sm:h-40 text-white rounded-xl flex justify-content-center items-center" style={{background : `url(${option.image})`,backgroundPosition: 'center', backgroundSize: 'cover'}}>
            <div className="absolute left-0 top-0 w-28 h-28 sm:h-40 sm:w-40 bg-black z-0 rounded-xl" style={{opacity: 0.6}}/>
            <span className="relative z-4 text-white sm:text-2xl font-bold text-xl text-center w-full">{option.title}</span>
        </div>
    </div>
}

export default MenuOptionButton