import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    envPrefix: 'FLASHCARD_', 
    root: ".",
    server: {
        port: 59313,
    },
    define: {
        "process.env": process.env
    }
})