import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export function MoviePoster({ id, name, image, description }) {
  const [secureImageUrl, setSecureImageUrl] = useState(image);
  const [imageError, setImageError] = useState(false);
  const navigate = useNavigate();

  const ensureHttps = (url) => {
    if (url && url.startsWith("http://")) {
      return url.replace("http://", "https://");
    }
    return url;
  };

  const handleImageError = () => {
    setImageError(true);
  };

  useEffect(() => {
    const updatedImageUrl = ensureHttps(image);
    setSecureImageUrl(updatedImageUrl);
    setImageError(false);
  }, [image]);

  const naviagtToMovie = (movieId) => {
    navigate(`/movie/${movieId}`);
  };

  return (
    <div 
      className="movie-poster" 
      onClick={() => naviagtToMovie(id)} 
      style={{ cursor: 'pointer' }}
    >
      <img
        src={secureImageUrl}
        alt={name}
        className="movie-poster-image"
        loading="lazy"
        onError={handleImageError}
        style={imageError ? { backgroundColor: '#ccc', display: 'flex', alignItems: 'center', justifyContent: 'center' } : {}}
      />
      {imageError && (
        <div className="movie-poster-placeholder">
          <p>No Image Available</p>
        </div>
      )}
      <div className="movie-poster-info">
        <h2 className="movie-name">{name}</h2>
        <p className="movie-description">{description}</p>
      </div>
    </div>
  );
}