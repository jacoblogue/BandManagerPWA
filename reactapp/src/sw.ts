import { precacheAndRoute } from "workbox-precaching";
import { registerRoute } from "workbox-routing";
import { CacheFirst, StaleWhileRevalidate } from "workbox-strategies";
import { clientsClaim, skipWaiting } from "workbox-core";
import { ExpirationPlugin } from "workbox-expiration";

declare let self: ServiceWorkerGlobalScope;

precacheAndRoute(self.__WB_MANIFEST);
// Cache images
registerRoute(
  /\/api\/.*$/,
  new CacheFirst({
    cacheName: "api-cache",
    plugins: [
      new ExpirationPlugin({
        maxEntries: 50,
        maxAgeSeconds: 60 * 60, // 1 hour
      }),
    ],
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
