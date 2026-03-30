import { useState , useEffect} from "react";
import { useForm } from "react-hook-form";
import GroupLabel from "./GroupLabel"
import { useNavigate } from "react-router-dom"; 
import Api  from "./Api";

function Login({ setIsConnected ,isConnected }) {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const navigate = useNavigate();

  async function onSubmit(data) {
    try {
      const response = await Api.post("/api/auth", {
        "courriel": data.email,
        "motDePasse": data.password
      });
      if (response.status === 200) {
        setIsConnected(true);
        localStorage.setItem("isConnected", "true");
        localStorage.setItem("idUser", response.data.idUtilisateur);
        navigate("/");
      } else {
        alert("La connexion a échoué. Veuillez vérifier vos identifiants.");
      }
    } catch (error) {
      alert("La connexion a échoué. Veuillez vérifier vos identifiants.");
    }
  }

  return (
    <div style={styles.container}>
      <div style={styles.formWrapper}>
        <h1 style={styles.heading}>Se connecter</h1>
        <form onSubmit={handleSubmit(onSubmit)} style={styles.form}>
          <div style={styles.inputGroup}>
            <label style={styles.label}>Email</label>
            <input
              type="email"
              placeholder="Email"
              style={styles.input}
              {...register("email", { required: "L'Email est requis" })}
            />
            {errors.email && <span style={styles.error}>{errors.email.message}</span>}
          </div>

          <div style={styles.inputGroup}>
            <label style={styles.label}>Mot de passe</label>
            <input
              type="password"
              placeholder="Mot de passe"
              style={styles.input}
              {...register("password", {
                required: "Le mot de passe est requis",
                minLength: { value: 4, message: "Le mot de passe doit avoir au moins 4 caractères" }
              })}
            />
            {errors.password && <span style={styles.error}>{errors.password.message}</span>}
          </div>
          <button type="submit" style={styles.button}>Se connecter</button>
        </form>
      </div>
    </div>
  );
}


const styles = {
  container: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    height: '50vh',
    backgroundColor: '#f4f7fc',
    fontFamily: 'Arial, sans-serif',
  },
  formWrapper: {
    backgroundColor: 'white',
    padding: '30px',
    borderRadius: '15px',
    boxShadow: '0 4px 10px rgba(0, 0, 0, 0.1)',
    width: '100%',
    maxWidth: '400px',
  },
  heading: {
    textAlign: 'center',
    marginBottom: '20px',
    fontSize: '24px',
    fontWeight: 'bold',
    color: '#333',
  },
  form: {
    display: 'flex',
    flexDirection: 'column',
    gap: '15px',
  },
  inputGroup: {
    display: 'flex',
    flexDirection: 'column',
    gap: '5px',
  },
  label: {
    fontSize: '14px',
    fontWeight: '500',
    color: '#555',
  },
  input: {
    padding: '10px',
    fontSize: '16px',
    border: '1px solid #ddd',
    borderRadius: '5px',
    outline: 'none',
    width: '100%',
  },
  inputFocus: {
    borderColor: '#007bff',
    boxShadow: '0 0 5px rgba(0, 123, 255, 0.5)',
  },
  error: {
    color: 'crimson',
    fontSize: '12px',
  },
  button: {
    padding: '10px',
    fontSize: '16px',
    backgroundColor: '#007bff',
    color: 'white',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
    transition: 'background-color 0.3s ease',
    width: '100%',
  },
};

export default Login;
