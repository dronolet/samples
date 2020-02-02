var concat = require('concat-files');
concat([
  './dist/main.js',
  './dist/polyfills.js',
  './dist/runtime.js',
  './dist/styles.js',
  './dist/vendor.js'
], './dist/app.js', function (err) {
  if (err) throw err
  console.log('done');
});
