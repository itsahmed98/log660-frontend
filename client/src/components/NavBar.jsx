import { Link, useNavigate } from 'react-router-dom';
import '../styles/NavBar.css';

function NavBar({ isConnected, setIsConnected }) {
  const navigate = useNavigate();

  const handleLogout = () => {
    setIsConnected(false);
    navigate('/');
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <ul className="navbar-menu">
          <li className="navbar-item">
            <Link to="/" className="navbar-link">Accueil</Link>
          </li>
          {isConnected && (
            <li className="navbar-item">
              {/*<Link to="/profile" className="navbar-link">Profile</Link>*/}
            </li>
          )}
        </ul>
             <div className="navbar-auth">
               {isConnected && (
                 <button onClick={handleLogout} className="navbar-button">Se déconnecter</button>
               )}
               {!isConnected && (
                 <Link to="/" className="navbar-link">Se connecter</Link>
               )}
             </div>
      </div>
    </nav>
  );
}

export default NavBar;
