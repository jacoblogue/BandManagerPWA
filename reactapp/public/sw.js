importScripts(
  "https://storage.googleapis.com/workbox-cdn/releases/6.4.1/workbox-sw.js"
);

precacheAndRoute(self.__WB_MANIFEST);
workbox.preCaching;
// Cache images
workboxConfig.registerRoute(
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
skipWaiting();
