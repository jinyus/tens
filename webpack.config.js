const path = require("path")

module.exports = {
    mode: "development",
    entry: "./src/App.fsproj",
    devServer: {
        static: path.join(__dirname, "./dist")
    },
    module: {
        rules: [{
            test: /\.fs(x|proj)?$/,
            use: "fable-loader"
        }]
    }
}