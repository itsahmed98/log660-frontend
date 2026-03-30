import { useState } from 'react';
import { Routes, Route } from 'react-router-dom';
import './App.css';
import NavBar from './components/NavBar';
import Login from './components/Login';
import HomepageConnected from './pages/HomepageConnected';
import ProfilePage from './pages/ProfilePage';
import MoviePage from './pages/MoviePage';
import PersonPage from './pages/PersonPage';

function  App() {
  const [isConnected, setIsConnected] = useState(() => {
    return localStorage.getItem("isConnected") === "true";
  });


  const handleSetIsConnected = (value) => {
    setIsConnected(value);
    localStorage.setItem("isConnected", value ? "true" : "false");
  };

  return(
    <>
      <NavBar isConnected={isConnected} setIsConnected={handleSetIsConnected} />
      <div style={{ paddingTop: '70px' }}>
        <Routes>
          <Route
            path="/"
            element={isConnected ? <HomepageConnected /> : <Login setIsConnected={handleSetIsConnected} isConnected={isConnected} />}
          />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/movie/:movieId" element={<MoviePage />} />
          <Route path="/personnes/:id" element={<PersonPage />} />
          <Route path="*" element={<h1>404 Not Found</h1>} />
        </Routes>
      </div>
    </>
  )
  
}

export default App
