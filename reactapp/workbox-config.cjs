module.exports = {
  swSrc: "public/sw.js",
  globDirectory: "build/",
  globPatterns: [
    "**/*.{html,js,css,png,jpg,jpeg,gif,svg,woff,woff2,eot,ttf,otf}",
  ],
  swDest: "build/sw.js",
};
