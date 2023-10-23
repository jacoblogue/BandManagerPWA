// Version 1.0
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.js";
import "./index.css";
import "./custom-theme.scss";
import { registerSW } from "virtual:pwa-register";
import { Auth0Provider } from "@auth0/auth0-react";

const domain = import.meta.env.VITE_REACT_APP_AUTH0_DOMAIN;
const clientId = import.meta.env.VITE_REACT_APP_AUTH0_CLIENT_ID;
console.log("cheese", domain, clientId);
ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <Auth0Provider
      domain={domain}
      clientId={clientId}
      authorizationParams={{
        redirect_uri: window.location.origin,
      }}
    >
      <App />
    </Auth0Provider>
  </React.StrictMode>
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
