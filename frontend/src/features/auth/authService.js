import axios, { HttpStatusCode } from "axios";
const API_URL = "https://localhost:62414/api/authentication/";

//Register User
const register = async (userData) => {
  console.log('logging register data')
  console.log({userData})
  const response = await axios.post(API_URL + "register", userData);
  console.log('register log')
  console.log({response})
 // if (response.data.status === HttpStatusCode.Created) return "Created";
  return response.data;
};

const login = async (userData) => {
  const response = await axios.post(API_URL + "login", userData);
  if (response.data) {
    localStorage.setItem("user", JSON.stringify(response.data));
  }
  return response;
};

const logout = () => localStorage.removeItem("user");

const authService = {
  register,
  login,
  logout,
};

export default authService;
