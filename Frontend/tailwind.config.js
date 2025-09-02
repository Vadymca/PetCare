/** @type {import('tailwindcss').Config} */
const tokens = require('./src/styles/tokens');
module.exports = {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      lineHeight: {
        none: '1',
      },
      animation: {
        'fade-in': 'fadeIn 0.5s ease-in-out',
        'fade-out': 'fadeOut 0.5s ease-in-out',
        'slide-up': 'slideUp 0.5s ease-in-out',
        'slide-down': 'slideDown 0.5s ease-in-out',
        'slide-left': 'slideLeft 0.5s ease-in-out',
        'slide-right': 'slideRight 0.5s ease-in-out',
        'slide-left-with-opacity': 'slideLeftWithOpacity 0.5s ease-in-out',
        'slide-right-with-opacity': 'slideRightWithOpacity 0.5s ease-in-out',
        'scale-in': 'scaleIn 0.5s ease-in-out',
        'scale-out': 'scaleOut 0.5s ease-in-out',
        'pulse-slow': 'pulseSlow 6s ease-in-out infinite',
        rotate: 'rotate 0.5s ease-in-out',
        bounce: 'bounce 0.5s ease-in-out',
        'counter-animated': 'counterAnimated 0.5s ease-in-out',
      },
      keyframes: {
        counterAnimated: {
          '0%': { transform: 'scale(1)' },
          '50%': { transform: 'scale(1.05)' },
          '100%': { transform: 'scale(1)' },
        },
        pulseSlow: {
          '0%, 100%': { boxShadow: '0 0 0 0 rgba(255,165,0,0.7)' },
          '50%': { boxShadow: '0 0 20px 10px rgba(255,165,0,0.5)' },
        },
        fadeIn: {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' },
        },
        fadeOut: {
          '0%': { opacity: '1' },
          '100%': { opacity: '0' },
        },

        slideUp: {
          '0%': { transform: 'translateY(20px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' },
        },
        slideDown: {
          '0%': { transform: 'translateY(0)', opacity: '1' },
          '100%': { transform: 'translateY(20px)', opacity: '0' },
        },
        slideLeft: {
          '0%': { transform: 'translateX(-20px)', opacity: '0' },
          '100%': { transform: 'translateX(0)', opacity: '1' },
        },
        slideRight: {
          '0%': { transform: 'translateX(20px)', opacity: '0' },
          '100%': { transform: 'translateX(0)', opacity: '1' },
        },
        slideLeftWithOpacity: {
          '0%': { transform: 'translateX(-20px)', opacity: '1' },
          '100%': { transform: 'translateX(0)', opacity: '1' },
        },
        slideRightWithOpacity: {
          '0%': { transform: 'translateX(20px)', opacity: '1' },
          '100%': { transform: 'translateX(0)', opacity: '1' },
        },
        scaleIn: {
          '0%': { transform: 'scale(0.9)', opacity: '0' },
          '100%': { transform: 'scale(1)', opacity: '1' },
        },
        scaleOut: {
          '0%': { transform: 'scale(1)', opacity: '1' },
          '100%': { transform: 'scale(0.9)', opacity: '0' },
        },
        rotate: {
          '0%': { transform: 'rotate(0deg)' },
          '100%': { transform: 'rotate(360deg)' },
        },
        bounce: {
          '0%, 100%': { transform: 'translateY(0)' },
          '50%': { transform: 'translateY(-10px)' },
        },
      },
      colors: tokens.colors,
      fontFamily: tokens.fontFamily,
      spacing: tokens.spacing,
      borderRadius: tokens.borderRadius,
      fontSize: {
        40: '40px',
        64: '64px',
        102: '102px',
      },
    },
    lineHeight: {
      DEFAULT: '1',
    },
  },
  plugins: [],
};
