import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Api from "../components/Api";

function MoviePage() {
  const { movieId } = useParams();
  const [movie, setMovie] = useState(null);
  const [imageError, setImageError] = useState(false);
  const [showPopup, setShowPopup] = useState(false);
  const [reservationStatus, setReservationStatus] = useState(null);
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    setImageError(false);
  }, [movie]);

  useEffect(() => {
    Api.get(`/api/Films/${movieId}`)
      .then(response => {
        setMovie(response.data);
      })
      .catch(error => {
        console.error('Error fetching movie data:', error);
      });
  }, [movieId]);

  async function makeReservation() {
    if (!movie || !movie.copiesFilm) {
      console.error("Movie or copiesFilm is not loaded yet.");
      return;
    }

    const firstAvailableCopy = movie.copiesFilm.find(copy => copy.statut === 'Disponible');
    const idCopie = firstAvailableCopy ? firstAvailableCopy.idCopie : null;
    const idUser = localStorage.getItem('idUser');

    if (!idCopie) {
      alert("Aucune copie disponible à réserver.");
      return;
    }

    const url = `/api/locations/louer?idUtilisateur=${parseInt(idUser, 10)}&idCopie=${idCopie}`;

    try {
      const response = await Api.post(url, {}, {
        headers: {
          "accept": "text/plain"
        }
      });
      setShowPopup(true);
      if (response.status === 200 && response.data.responseCode === 1) {
        setReservationStatus('success');
        setMovie(prevMovie => ({
          ...prevMovie,
          copiesFilm: prevMovie.copiesFilm.filter(copy => copy.idCopie !== idCopie)
        }));
        setTimeout(() => {
          setShowPopup(false);
          setReservationStatus(null);
        }, 2000);
      } else {
        setReservationStatus('error');
        setErrorMessage(response.data.message);
        setTimeout(() => {
          setShowPopup(false);
          setReservationStatus(null);
        }, 2000);
      }
    } catch (error) {
      setReservationStatus('error');
      setTimeout(() => {
        setShowPopup(false);
        setReservationStatus(null);
      }, 2000);
      console.error('Error making reservation:', error);
    }
  }


  if (!movie) {
    return <div className="loading">Chargement...</div>;
  }

  function ensureHttps(url) {
    if (url && url.startsWith("http://")) {
      return url.replace("http://", "https://");
    }
    return url;
  }


  const totalCopies = movie.copiesFilm ? movie.copiesFilm.length : 0;
  const availableCopies = movie.copiesFilm ? movie.copiesFilm.filter(copy => copy.statut === 'Disponible').length : 0;


  const progress = totalCopies > 0 ? (availableCopies / totalCopies) * 100 : 0;

  const navigateToPerson = (personId) => {
    navigate(`/personnes/${personId}`);
  };

  return (
    <div className="movie-page">
      <h1>{movie.titre}</h1>
      <img
        src={ensureHttps(movie.affiche)}
        alt={movie.titre}
        className="movie-poster-image"
        loading="lazy"
        onError={() => setImageError(true)}
      />
      {imageError && (
        <div className="movie-poster-placeholder">
          <p>Aucune image disponible</p>
        </div>
      )}

      <p><strong>Année:</strong> {movie.annee}</p>
      <p><strong>Langue:</strong> {movie.langue}</p>
      <p><strong>Durée:</strong> {movie.duree} minutes</p>
      <p><strong>Description:</strong> {movie.resume}</p>

      <p><strong>Genres:</strong> {movie.filmsGenres.map((genre, index) => (
        <span key={index}>
          {genre.nomGenre}{index < movie.filmsGenres.length - 1 && ', '}
        </span>
      ))}</p>

      <div className="director">
        <p><strong>Réalisateur:</strong> <span style={{ cursor: 'pointer', color: '#0000EE', textDecoration: 'underline' }} onClick={() => navigateToPerson(movie.realisateur.idPersonne)}>{movie.realisateur.nom}</span> ({movie.realisateur.lieuNaissance})</p>
      </div>

      <div className="actors">
        <p><strong>Acteurs:</strong> {movie.acteurs.map((actor, index) => (
          <span key={index}>
            <span style={{ cursor: 'pointer', color: '#0000EE', textDecoration: 'underline' }} onClick={() => navigateToPerson(actor.idPersonne)}>
              {actor.nom}
            </span> as {actor.nomPersonnage}
            {index < movie.acteurs.length - 1 && ', '}
          </span>
        ))}</p>
      </div>

      <p><strong>Copies disponibles:</strong></p>
      <div className="progress-bar">
        <span></span>
      </div>
      <div className="progress-bar-label">
        {availableCopies} sur {totalCopies} copies disponibles
      </div>

      <div className="button-container">
        <button onClick={() => navigate(-1)}>Retour</button>
        <button
          disabled={!(movie.copiesFilm && movie.copiesFilm.filter(copy => copy.statut === 'Disponible').length > 0)}
          style={{
            opacity: movie.copiesFilm && movie.copiesFilm.filter(copy => copy.statut === 'Disponible').length > 0 ? 1 : 0.5,
            cursor: movie.copiesFilm && movie.copiesFilm.filter(copy => copy.statut === 'Disponible').length > 0 ? 'pointer' : 'not-allowed'
          }}
          onClick={makeReservation}
        >
          Réserver
        </button>
      </div>

      {showPopup && (
        <div className="popup">
          <div className="popup-content">
            {reservationStatus === 'success' ? (
              <>
                <h2>Réservation réussie</h2>
                <p>Votre réservation a réussi!</  p>
              </>
            ) : (
              <>
                <h2>Réservation échouée</h2>
                <p>Erreur lors de la réservation.</p>
                <p>{errorMessage}</p>
              </>
            )}
          </div>
        </div>
      )}

      <style jsx>{`
        .movie-page {
          max-width: 1200px;
          margin: 0 auto;
          padding: 1.5rem;
          display: flex;
          flex-direction: column;
          align-items: center;
          background-color: #f0f0f0;
          border-radius: 12px;
          box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
        }

        .movie-page h1 {
          font-size: 2rem;
          color: #2c2c2c;
          margin-bottom: 1rem;
          text-align: left;
        }

        .movie-page button {
          padding: 0.8rem 1.5rem;
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
          color: white;
          border: none;
          border-radius: 8px;
          font-weight: 600;
          cursor: pointer;
          transition: all 0.3s ease;
        }

        .movie-page button:hover {
          transform: translateY(-4px);
          box-shadow: 0 8px 25px rgba(102, 126, 234, 0.3);
        }

        .button-container {
          display: flex;
          gap: 1rem;
        }

        .popup {
          position: fixed;
          top: 0;
          left: 0;
          right: 0;
          bottom: 0;
          background-color: rgba(0, 0, 0, 0.5);
          display: flex;
          justify-content: center;
          align-items: center;
          z-index: 1000;
        }

        .popup-content {
          background: white;
          padding: 2rem;
          border-radius: 10px;
          text-align: center;
          width: 300px;
        }

        .popup button {
          margin-top: 1rem;
        }

        .movie-poster-image {
          width: 100%;
          max-width: 300px;
          aspect-ratio: 2 / 3;
          object-fit: cover;
          background-color: #f5f5f5;
          border-radius: 12px;
          box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
          margin-bottom: 1rem;
        }

        .movie-page p {
          font-size: 1.1rem;
          margin: 0.4rem 0;
          color: #444;
          text-align: center;
        }

        .movie-page strong {
          color: #333;
        }

        .movie-page .director, .movie-page .actor {
          color: #444;
          font-size: 1.1rem;
          margin-bottom: 0.5rem;
        }

        .movie-page .director p, .movie-page .actor p {
          margin: 0;
        }

        .movie-page .actors a strong,
        .movie-page .director a strong {
          color: #0000EE !important;
          font-weight: normal;
        }

        .movie-page .actors a {
          color: #0000EE;
          text-decoration: none;
          font-weight: normal;
        }

        .movie-page .actors a:hover,
        .movie-page .director a:hover {
          text-decoration: underline;
        }

        .loading {
          text-align: center;
          font-size: 1.5rem;
          color: #888;
        }

        .progress-bar {
          width: 100%;
          height: 20px;
          background-color: #e0e0e0;
          border-radius: 10px;
          overflow: hidden;
          margin-top: 1rem;
        }

        .progress-bar span {
          display: block;
          height: 100%;
          background-color: #4caf50;
          width: ${progress}%;
          transition: width 0.3s ease;
        }

        .progress-bar-label {
          text-align: center;
          margin-top: 5px;
          font-size: 1rem;
        }
      `}</style>
    </div>
  );
}

export default MoviePage;