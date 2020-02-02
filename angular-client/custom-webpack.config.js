const path = require('path');
const WebpackNotifierPlugin = require('webpack-notifier');

module.exports = {
  plugins: [
    new WebpackNotifierPlugin({
      alwaysNotify: true,
      title: 'App Name',
      contentImage: path.join(__dirname, 'image.png')
    }),
  ]
}
