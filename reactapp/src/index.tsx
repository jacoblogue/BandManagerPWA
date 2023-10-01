import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.js";
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { registerSW } from "virtual:pwa-register";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

if ("serviceWorker" in navigator) {
  registerSW({
    onRegisterError(error) {
      console.error(error.message);
    },
  });
}
