const colors = require('tailwindcss/colors')
const defaultTheme = require('tailwindcss/defaultTheme')

module.exports = {
  mode: 'jit', 
  content: [
    '../Vendr.Checkout/Web/UI/App_Plugins/VendrCheckout/views/**/*.cshtml',
    '../Vendr.Checkout/Web/UI/App_Plugins/VendrCheckout/scripts/**/*.js',
  ],
  safelist: [
    {
      // Make configurable theme colors safe
      pattern: /(bg|text)-(red|orange|yellow|green|teal|blue|indigo|purple|pink)-500/,
      variants: ['hocus'],
    }
  ],
  variants: {
    extend: {
      backgroundColor: ['responsive', 'hocus', 'scrolled', 'group-hover'],
      borderColor: ['responsive', 'hocus', 'scrolled', 'group-hover'],
      textColor: ['responsive', 'hocus', 'scrolled', 'group-hover'],
      margin: ['responsive', 'scrolled'],
      padding: ['responsive', 'scrolled']
    },
  },
  corePlugins: {
    container: false,
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('./src/css/plugins/hocus'),
    require('./src/css/plugins/scrolled')
  ],
}
