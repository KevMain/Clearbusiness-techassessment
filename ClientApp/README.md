# ClientApp - Vue 3 Frontend

Modern Vue 3 + TypeScript + Vite frontend for the technical assessment.

## 🚀 Quick Start

```bash
cd ClientApp
npm install
npm run dev
```

The app runs at `http://localhost:5173` and automatically proxies API calls to the backend.

## 📁 Project Structure

```
ClientApp/
├── src/
│   ├── App.vue          # Main application component
│   ├── main.ts          # Application entry point
│   └── assets/          # Static assets
├── vite.config.js       # Vite configuration with API proxy
└── package.json         # Dependencies
```

## 🔧 Configuration

### API Proxy
The app uses Vite's dev proxy to route `/api` requests to the backend at `https://localhost:5001`.

If your backend runs on a different port, edit `vite.config.js`:

```js
export default defineConfig({
  server: {
	proxy: {
	  '/api': {
		target: 'https://localhost:YOUR_PORT',  // Change this
		changeOrigin: true,
		secure: false,
	  }
	}
  }
})
```

### Direct API Calls (Alternative)
Instead of using the proxy, you can update the fetch URL in `src/App.vue` to the full backend URL:

```typescript
const response = await fetch('https://localhost:5001/api/hello')
```

## 🛠️ Available Commands

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build locally

## 🎨 Technology Stack

- **Vue 3**: Composition API, reactive state management
- **TypeScript**: Type-safe development
- **Vite**: Fast builds and hot module replacement

---

For backend setup, see the [main README](../README.md).
