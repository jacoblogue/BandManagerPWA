// Version 1.0
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.js";
import "./index.css";
import "./custom-theme.scss";
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
    onRegisteredSW(sw, r) {
      console.log("Registered SW", sw, r);
      r &&
        setInterval(() => {
          r.update();
        }, 1000 * 60 * 60 * 24);
    },
  });
}
