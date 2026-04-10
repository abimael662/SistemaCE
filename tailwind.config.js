module.exports = {
    content: [
        "./Views/**/*.cshtml",
        "./Views/**/**/*.cshtml",
        "./Pages/**/*.cshtml",
        "./wwwroot/**/*.js"
    ],
    theme: {
        extend: {
            colors: {
                primary: {
                    100: '#F1EDFF',
                    300: '#C9C9FF',
                    500: '#8A78E1',
                    700: '#4A4FA9',
                    900: '#271870'
                }
            }
        }
    },
    plugins: [],
}