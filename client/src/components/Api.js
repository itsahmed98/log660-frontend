import axios from "axios";

const Api = axios.create({
  baseURL: "https://localhost:7273",
});

export default Api;