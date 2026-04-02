import { useState, useEffect } from 'react';
import Api from '../components/Api';

function AnalytiquePage() {
  const [filtres, setFiltres] = useState({
    groupesAge: [],
    provinces: [],
    joursSemaine: [],
    mois: [],
  });

  const [selection, setSelection] = useState({
    groupeAge: 'Tous',
    province: 'Toutes',
    jourSemaine: 'Tous',
    moisAnnee: 'Tous',
  });

  const [nombreLocations, setNombreLocations] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    Api.get('/api/analytique/filtres')
      .then((res) => setFiltres(res.data))
      .catch(() => setError('Erreur lors du chargement des filtres.'));
  }, []);

  const handleChange = (e) => {
    setSelection((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      const res = await Api.get('/api/analytique', { params: selection });
      setNombreLocations(res.data.nombreLocations);
    } catch {
      setError('Erreur lors de la récupération des données.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: '480px', margin: '40px auto', padding: '24px', border: '1px solid #ccc', borderRadius: '8px' }}>
      <h2 style={{ marginBottom: '20px' }}>Calcul du nombre de locations</h2>
      <form onSubmit={handleSubmit}>
        <table style={{ width: '100%', borderCollapse: 'collapse', marginBottom: '16px' }}>
          <tbody>
            <FilterRow label="Groupe d'âge" name="groupeAge" value={selection.groupeAge} onChange={handleChange} options={filtres.groupesAge} allLabel="Tous" />
            <FilterRow label="Mois" name="moisAnnee" value={selection.moisAnnee} onChange={handleChange} options={filtres.mois} allLabel="Tous" />
            <FilterRow label="Jour" name="jourSemaine" value={selection.jourSemaine} onChange={handleChange} options={filtres.joursSemaine} allLabel="Tous" />
            <FilterRow label="Province" name="province" value={selection.province} onChange={handleChange} options={filtres.provinces} allLabel="Toutes" />
          </tbody>
        </table>
        <button type="submit" disabled={loading} style={{ padding: '6px 18px', cursor: 'pointer' }}>
          {loading ? 'Calcul...' : 'Ok'}
        </button>
      </form>

      {error && <p style={{ color: 'red', marginTop: '12px' }}>{error}</p>}

      {nombreLocations !== null && !loading && (
        <p style={{ marginTop: '16px', fontWeight: 'bold' }}>
          Nombre de locations : {nombreLocations}
        </p>
      )}
    </div>
  );
}

function FilterRow({ label, name, value, onChange, options, allLabel }) {
  return (
    <tr>
      <td style={{ padding: '6px 12px 6px 0', fontWeight: '500', whiteSpace: 'nowrap' }}>{label} :</td>
      <td style={{ padding: '6px 0' }}>
        <select name={name} value={value} onChange={onChange} style={{ minWidth: '140px' }}>
          <option value={allLabel}>{allLabel}</option>
          {options.map((opt) => (
            <option key={opt} value={opt}>{opt}</option>
          ))}
        </select>
      </td>
    </tr>
  );
}

export default AnalytiquePage;
