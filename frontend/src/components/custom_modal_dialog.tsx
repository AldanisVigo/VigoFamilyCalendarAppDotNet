import classNames from 'classnames';
const CustomModalDialog = ({title,content,showModal,setShowModal}:any) => {
    var dialogClass = classNames({
        'grid' : true,
        'place-content-center': true,
        'z-50' : true,
        'hidden' : !showModal,
        'visible' : showModal,
        'mt-96' : true,
        'lg:mt-0' : true,
        'md:mt-0' : true,
        'overflow-x-hidden' : true,
        'p-4' : true,
        'md:inset-0' : true,
        'fixed' : true,
    });

    return <div className="flex justify-center items-center h-full">
        <div
            tabIndex={-1}
            aria-hidden="true"
            className={dialogClass}
        >
            <div className="w-full max-w-2xl">
                <div className="relative rounded-lg bg-white shadow dark:bg-gray-700">
                    <div
                        className="flex items-start justify-between rounded-t border-b p-5 dark:border-gray-600"
                    >
                        <h3
                            className="text-xl font-semibold text-gray-900 dark:text-white lg:text-2xl"
                        >
                            {title}
                        </h3>
                        <button
                            onClick={()=>setShowModal(false)}
                            type="button"
                            className="ms-auto inline-flex h-8 w-8 items-center justify-center rounded-lg bg-transparent text-sm text-gray-400 hover:bg-gray-200 hover:text-gray-900 dark:hover:bg-gray-600 dark:hover:text-white"
                        >
                            <svg
                                className="h-3 w-3"
                                aria-hidden="true"
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 14 14"
                            >
                                <path
                                    stroke="currentColor"
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth="2"
                                    d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
                                />
                            </svg>
                            <span className="sr-only">Close modal</span>
                        </button>
                    </div>
        
                    <div className="space-y-6 p-6">
                        <p
                            className="text-base leading-relaxed text-gray-500 dark:text-gray-400"
                        >
                            {content}
                        </p>
                    </div>
                    <div
                        className="flex space-x-2 rtl:space-x-reverse rounded-b border-t border-gray-200 p-6 dark:border-gray-600"
                    >
                        <button
                            type="button"
                            className="mr-0 ml-auto rounded-lg bg-blue-700 px-5 py-2.5 text-center text-sm font-medium text-white hover:bg-blue-800 focus:outline-none focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                            onClick={()=>setShowModal(false)}
                        >
                            Close
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
}

export default CustomModalDialog;