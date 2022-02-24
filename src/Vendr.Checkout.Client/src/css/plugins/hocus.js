const plugin = require('tailwindcss/plugin')
const selectorParser = require('postcss-selector-parser');

module.exports = plugin(function({ addVariant, e }) {

    addVariant('hocus', ({ modifySelectors, separator }) => {
        modifySelectors(({ className }) => {
            return `.${e(`hocus${separator}${className}`)}:hover, .${e(`hocus${separator}${className}`)}:focus`;
        });
    });

    addVariant('group-hocus', ({ modifySelectors, separator }) => {
        modifySelectors(({ className }) => {
            return `.group:hover .${e(`group-hocus${separator}${className}`)}, .group:focus .${e(`group-hocus${separator}${className}`)}`;
        });
    });

})