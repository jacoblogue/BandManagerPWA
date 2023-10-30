// Version 1.0
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.js";
import "./index.css";
import "./custom-theme.scss";
import { registerSW } from "virtual:pwa-register";
import Auth0ProviderWithNavigate from "./components/auth/Auth0ProviderWithNavigate.js";
import { BrowserRouter } from "react-router-dom";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <BrowserRouter>
    <React.StrictMode>
      <Auth0ProviderWithNavigate>
        <App />
      </Auth0ProviderWithNavigate>
    </React.StrictMode>
  </BrowserRouter>
);

if ("serviceWorker" in navigator) {
  registerSW({
    onRegisterError(error) {
      console.error(error.message);
    },
    onRegisteredSW(sw, r) {
      console.log("Registered SW", sw, r);
      r &&
        setInterval(() => {
          r.update();
        }, 1000 * 60 * 60 * 24);
    },
  });
}
