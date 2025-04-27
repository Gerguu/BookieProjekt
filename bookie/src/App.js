import logo from './logo.svg';
import './App.css';
import Menu from './components/Menu/Menu'
import Vasarlas from './components/Vasarlas/Vasarlas'
import Eladas from './components/Eladas/Eladas'
import SearchResultsList from './components/Eladas/SearchResultsList'
import Chat from './components/Chat/Chat'
import Profil from './components/Profil/Profil'
import Registration from './components/Registration/Registration'
import Login from './components/Login/Login'
import NewBook from './components/NewBook/NewBook'
import Termek from './components/Termek/Termek'
import Termekeim from './components/Termekeim/Termekeim';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
      <Menu />
        <Routes>
          <Route path='/' element={<Vasarlas />} />
          <Route path='/Vasarlas' element={<Vasarlas />} />
          <Route path='/Termek/:termekId' element={<Termek />} />
          <Route exact path='/Termek' element={<Termek />} />
          <Route path='/Eladas' element={<Eladas />} />
          <Route path='/Chat' element={<Chat />} />
          <Route path='/Profil' element={<Profil />} />
          <Route path='/Profil/Termekeim/:termekemId' element={<Termekeim />} />
          <Route path='/Vasarlas/Termekeim/:termekemId' element={<Termekeim />} />
          <Route exact path='/Termekeim' element={<Termekeim />} />
          <Route path='/Registration' element={<Registration />} />
          <Route path='/Login' element={<Login />} />
          <Route path='/NewBook' element={<NewBook />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
