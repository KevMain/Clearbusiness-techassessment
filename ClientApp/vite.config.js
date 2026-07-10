import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  server: {
    // Proxy API requests to the ASP.NET backend running locally over HTTPS.
    // secure: false allows self-signed dev certificates.
    proxy: {
      '/api': {
        target: 'https://localhost:7101',
        changeOrigin: true,
        secure: false
      }
    }
  }
})
