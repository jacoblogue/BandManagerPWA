import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";
import fs from "fs";
import path from "path";
import { VitePWA } from "vite-plugin-pwa";

process.env.BROWSER = "chrome";
const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ""
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`;

const certificateArg = process.argv
  .map((arg) => arg.match(/--name=(?<value>.+)/i))
  .filter(Boolean)[0];
const certificateName = certificateArg
  ? certificateArg.groups.value
  : "reactapp";

if (!certificateName) {
  console.error(
    "Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly."
  );
  process.exit(-1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    plugin(),
    VitePWA({
      strategies: "injectManifest",
      injectRegister: "script",
      srcDir: "src",
      filename: "sw.ts",
      registerType: "autoUpdate",
      workbox: {
        globPatterns: ["**/*.{js,css,html,png}", "assets/**/*"],
      },
      devOptions: {
        enabled: true,
        type: "module",
        navigateFallback: "/index.html",
      },
    }),
  ],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    host: "0.0.0.0", // allows phone to connect
    proxy: {
      "^/weatherforecast": {
        target: "https://localhost:7149/",
        secure: false,
      },
      "^/api/*": {
        target: "https://localhost:7149/",
        secure: false,
      },
    },
    port: 5173,
    open: {
      app: "chrome",
    },
    https: {
      key: fs.readFileSync(keyFilePath),
      cert: fs.readFileSync(certFilePath),
    },
  },
});
