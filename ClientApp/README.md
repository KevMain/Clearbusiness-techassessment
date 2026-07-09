This is a minimal Vue 3 client (Vite) that calls the backend API at /api/hello.

Quick run

1. cd ClientApp
2. npm install
3. npm run dev

Vite prints the dev server URL (e.g., http://localhost:5173). Open it in the browser and the page will call `/api/hello`.

If your backend API runs on a different origin/port, configure a dev proxy (recommended) or change the fetch URL in `src/App.vue` to the full API URL.

Example Vite proxy (create `ClientApp/vite.config.js`):

```js
import { defineConfig } from 'vite'

export default defineConfig({
  server: {
	proxy: {
	  '/api': {
		target: 'https://localhost:5001',
		changeOrigin: true,
		secure: false,
	  }
	}
  }
})
```

Notes
- The frontend expects the API at `/api/hello` by default. Use the proxy or a full URL when necessary.
- If you want, I can add `vite.config.js` with the proxy configured for your backend port.
