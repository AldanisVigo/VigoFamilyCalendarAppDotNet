export default {
    content: [
      "./index.html",
      "./src/**/*.{js,ts,jsx,tsx}",
    ],
    theme: {
      extend: {
        boxShadow: {
          lg: '0 0px 10px 3px rgba(255, 255, 255, 0.5), 0 0px 10px 3px rgba(255, 255, 255, 0.05)',
        }
      },
    },
    plugins: [],
  }