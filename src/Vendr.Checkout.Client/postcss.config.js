module.exports = {
    plugins: [
        require('postcss-import'),
        require('tailwindcss')
    ]
}

if (process.env.NODE_ENV === 'production') {
    module.exports.plugins.push(require('cssnano'))
}