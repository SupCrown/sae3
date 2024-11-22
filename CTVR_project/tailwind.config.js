/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html','./src/**/*.{vue,js,ts,jsx,tsx}',],
  theme: {
    extend: {colors: {
      grayCustom: '#A3A3A3',
      greenCustom: '#D9F3E4',
      greenCustom2: '#003E1C',
    },},
  },
  plugins: [],
}

