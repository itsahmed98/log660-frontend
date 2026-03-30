import React, { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import Api from "../components/Api";

function PersonPage() {
  const { id } = useParams();  
  const [person, setPerson] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    Api.get(`/api/personnes/${id}`)
      .then(response => {
        setPerson(response.data);
      })
      .catch(error => {
        console.error('Error fetching person data:', error);
      });
  }, [id]);

  if (!person) {
    return <div className="loading">Chargement...</div>; 
  }

  const { nom, dateNaissance, lieuNaissance, biographie, photo, estActeur, estRealisateur } = person;

  function ensureHttps(url) {
    if (url && url.startsWith("http://")) {
      return url.replace("http://", "https://");
    }
    return url;
  }

  const personPhoto = photo ? ensureHttps(photo) : "https://via.placeholder.com/150"; 

  return (
    <div className="person-page">
      <style jsx>{`
        .person-page {
          max-width: 800px;
          margin: 0 auto;
          padding: 1.5rem;
          background-color: #f0f0f0;
          border-radius: 12px;
          box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
        }

        .person-page h1 {
          font-size: 2.5rem;
          color: #2c2c2c;
          margin-bottom: 1rem;
          text-align: center;
        }

        .person-page img {
          max-width: 200px;
          border-radius: 10px;
          margin-bottom: 1rem;
          display: block;
          margin-left: auto;
          margin-right: auto;
        }

        .person-page p {
          font-size: 1.1rem;
          color: #444;
          text-align: center;
        }

        .person-page strong {
          color: #333;
        }

        .person-page .biography {
          margin-top: 1rem;
          font-size: 1.1rem;
          color: #555;
          text-align: justify;
        }

        .person-page button {
          padding: 0.8rem 1.5rem;
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
          color: white;
          border: none;
          border-radius: 8px;
          font-weight: 600;
          cursor: pointer;
          transition: all 0.3s ease;
          margin-top: 2rem;
          display: block;
          margin-left: auto;
          margin-right: auto;
        }

        .person-page button:hover {
          transform: translateY(-4px);
          box-shadow: 0 8px 25px rgba(102, 126, 234, 0.3);
        }

        .person-page button:active {
          transform: translateY(0);
        }

        .loading {
          text-align: center;
          font-size: 1.5rem;
          color: #888;
        }
      `}</style>

      <h1>{nom}</h1>
      {photo ? (
        <img src={ensureHttps(photo)} alt={nom} />
      ) : (
        <div style={{textAlign: 'center', color: '#888', marginBottom: '1rem'}}>Pas d'image disponible</div>
      )}
      
      <p><strong>Date de naissance:</strong> {new Date(dateNaissance).toLocaleDateString()}</p>
      <p><strong>Lieu de naissance:</strong> {lieuNaissance}</p>

      <div className="biography">
        <p><strong>Biographie:</strong></p>
        <p>{biographie}</p>
      </div>

      <div className="roles">
        {estActeur && <p><strong>Role:</strong> Acteur</p>}
        {estRealisateur && <p><strong>Role:</strong> Realisateur</p>}
      </div>

      <button onClick={() => navigate(-1)}>Retour</button>
    </div>
  );
}

export default PersonPage;