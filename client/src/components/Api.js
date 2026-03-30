import axios from "axios";

const Api = axios.create({
  baseURL: "http://localhost:5275",
});

export default Api;