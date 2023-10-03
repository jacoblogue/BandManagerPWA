// This is your custom service worker file
import { precacheAndRoute } from "workbox-precaching";
import { registerRoute } from "workbox-routing";
import {
  CacheFirst,
  StaleWhileRevalidate,
  NetworkFirst,
} from "workbox-strategies";
import { ExpirationPlugin } from "workbox-expiration";
import { clientsClaim, skipWaiting } from "workbox-core";

precacheAndRoute([]);

registerRoute(
  /\/api\/.*$/,
  new NetworkFirst({
    cacheName: "api-cache",
  })
);

// Cache static resources
registerRoute(
  /\.(?:js|css)$/,
  new StaleWhileRevalidate({
    cacheName: "static-resources",
  })
);

// Take control of clients as soon as possible
clientsClaim();

// Skip waiting for the service worker to become active
skipWaiting();
