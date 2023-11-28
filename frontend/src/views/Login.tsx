import '../App.css'
import Login from '../components/login'
import Header from '../components/header'
import Background from '../components/background'
const App = () => {
  return <>
    <Header/>
    <Background/>
    <div className="main flex items-center">
      <Login/>
    </div>
  </>

}

export default App
