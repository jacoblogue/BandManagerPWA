import { precacheAndRoute } from "workbox-precaching";
import { registerRoute } from "workbox-routing";
import { StaleWhileRevalidate, NetworkFirst } from "workbox-strategies";
import { clientsClaim } from "workbox-core";

declare let self: ServiceWorkerGlobalScope;

precacheAndRoute(self.__WB_MANIFEST);
// Cache images
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
self.skipWaiting();
