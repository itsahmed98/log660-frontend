function GroupLabel({ label, error, ...inputProps }) {
  return (
    <div style={styles.fieldContainer}>
      <label style={styles.label}>{label}</label>
      <div style={styles.inputWrapper}>
        <input style={styles.input} {...inputProps} />
        {error && <span style={styles.error}>{error}</span>}
      </div>
    </div>
  );
}

export default GroupLabel;
const styles = {
  fieldContainer: {
    display: 'flex',
    flexDirection: 'row',
    alignItems: 'flex-start',
    gap: '55px',
    marginBottom: '15px',
  },
  label: {
    fontSize: '14px',
    fontWeight: '500',
    color: '#555',
    width: '70px',
    flexShrink: 0,
    textAlign: 'left',
    paddingTop: '10px',
  },
  inputWrapper: {
    flex: 1,
    display: 'flex',
    flexDirection: 'column',
    minWidth: 0,
  },
  input: {
    padding: '10px',
    fontSize: '16px',
    border: '1px solid #ddd',
    borderRadius: '5px',
    outline: 'none',
    width: '100%',
    boxSizing: 'border-box',
  },
  error: {
    color: 'red',
    fontSize: '12px',
    marginTop: '3px',
  },
};