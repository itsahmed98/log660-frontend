import React, { useState, useEffect } from 'react';
import '../App.css';
import { MoviePoster } from '../components/MoviePoster';
import Api from "../components/Api";
import GroupLabel from '../components/GroupLabel';
import { useNavigate } from 'react-router-dom';

function HomepageConnected() {
  const [searchQuery, setSearchQuery] = useState('');
  const [showAdvancedSearch, setShowAdvancedSearch] = useState(false);

  const [movies, setMovies] = useState([]);
  const [languages, setLanguages] = useState([]);
  const [genre, setGenre] = useState([]);
  const [countries, setCountries] = useState([]);

  const [formData, setFormData] = useState({
    name: '',
    actor: '',
    director: '',
    genre: '',
    country: '',
    language: '',
    minYear: '',
    maxYear: ''
  });

  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  useEffect(() => {
    Api.get('/api/Films/pays')
      .then(response => {
        setCountries(response.data);
      })
      .catch(error => {
        console.error('Error fetching countries:', error);
      });
  }, []);

  useEffect(() => {
    Api.get('/api/Films/langues')
      .then(response => {
        setLanguages(response.data);
      })
      .catch(error => {
        console.error('Error fetching languages:', error);
      });
  }, []);

  useEffect(() => {
    Api.get('/api/Films/genres')
      .then(response => {
        setGenre(response.data);
      })
      .catch(error => {
        console.error('Error fetching genres:', error);
      });
  }, []);

  const handleSearch = async (e) => {
    e.preventDefault();

    try {
      const response = await Api.get("/api/Films/search", {
         params: {
          "Titre": formData.name,
          "Realisateur": formData.director,
          "Acteur": formData.actor,
          "IdGenre": formData.genre ? parseInt(formData.genre, 10) : null,
          "IdPays": formData.country ? parseInt(formData.country, 10) : null,
          "Langue": formData.language,
          "MinAnnee": formData.minYear ? parseInt(formData.minYear, 10) : null,
          "MaxAnnee": formData.maxYear ? parseInt(formData.maxYear, 10) : null,
        }
      });

      if (response.status === 200) {
        setMovies(response.data)
        navigate("/");
      } else {
        alert("La connexion a échoué. Veuillez vérifier vos identifiants.");
      }
      setShowAdvancedSearch(false)
    } catch (error) {
      setShowAdvancedSearch(false)
      alert("La recherche n'est pas valide");
    }
  };

  const filteredMovies = searchQuery.trim() === ""
    ? movies
    : movies.filter(movie =>
        movie.titre && movie.titre.toLowerCase().includes(searchQuery.toLowerCase())
      );

  return (
    <div className="homepage-container">
      <div className="search-container">
        <form onSubmit={handleSearch} className="search-form">
          <input
            type="text"
            placeholder="Rechercher des films..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="search-input"
          />
          <button 
            type="button" 
            onClick={() => setShowAdvancedSearch(!showAdvancedSearch)} 
            className="advanced-search-btn">
                Recherche avancée
          </button>
        </form>
      </div>
      
      {showAdvancedSearch && (
        <>
          <div className="modal-backdrop" onClick={() => setShowAdvancedSearch(false)}></div>
          <div className="advanced-search-modal">
            <h3>Recherche avancée</h3>
            <form onSubmit={handleSearch}>
              <GroupLabel
                label="Nom"
                name="name"
                value={formData.name}
                onChange={handleChange}
              />
              <GroupLabel
                label="Acteur"
                name="actor"
                value={formData.actor}
                onChange={handleChange}
              />
              <GroupLabel
                label="Réalisateur"
                name="director"
                value={formData.director}
                onChange={handleChange}
              />
              
              <div className="form-row">
                <label htmlFor="genre-select">Genre</label>
                <select
                  id="genre-select"
                  name="genre"
                  value={formData.genre}
                  onChange={handleChange}
                >
                  <option value="">Sélectionner le genre</option>
                  {genre.map((g, idx) => (
                    <option key={idx} value={g.idGenre}>{g.nomGenre}</option>
                  ))}
                </select>
              </div>

              <div className="form-row">
                <label htmlFor="country-select">Pays</label>
                <select
                  id="country-select"
                  name="country"
                  value={formData.country}
                  onChange={handleChange}
                >
                  <option value="">Sélectionner le pays</option>
                  {countries.map((c, idx) => (
                    <option key={idx} value={c.idPays}>{c.nomPays}</option>
                  ))}
                </select>
              </div>

              <div className="form-row">
                <label htmlFor="language-select">Langue</label>
                <select
                  id="language-select"
                  name="language"
                  value={formData.language}
                  onChange={handleChange}
                >
                  <option value="">Sélectionner la langue</option>
                  {languages.map((l, idx) => (
                    <option key={idx} value={l}>{l}</option>
                  ))}
                </select>
              </div>

              <GroupLabel
                label="Année min"
                name="minYear"
                value={formData.minYear}
                onChange={handleChange}
              />
              <GroupLabel
                label="Année max"
                name="maxYear"
                value={formData.maxYear}
                onChange={handleChange}
              />

              <div className="button-container">
                <button type="submit" className="search-btn">
                  Rechercher
                </button>
                <button
                  type="button"
                  onClick={() => setShowAdvancedSearch(false)}
                  className="close-btn"
                >
                  Fermer
                </button>
              </div>
            </form>
          </div>
        </>
      )}
      
      <div className="movie-list">
        {filteredMovies.map((movie) => (
          <MoviePoster
            key={movie.idFilm}
            id={movie.idFilm}
            name={movie.titre}
            description={movie.resume}
            image={movie.affiche}
          />
        ))}
      </div>
    </div>
  );
}

export default HomepageConnected;