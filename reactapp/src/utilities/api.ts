import axios from "axios";
// This might need to change when I deploy
export const api = axios.create({ baseURL: "/api" });
