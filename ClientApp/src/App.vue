<template>
  <div class="app">
    <header class="header">
      <h1>Customer Directory</h1>
    </header>

    <main class="main-content">
      <div v-if="loading" class="loading">
        <div class="spinner"></div>
        <p>Loading customers...</p>
      </div>

      <div v-else-if="error" class="error">
        <p>{{ error }}</p>
      </div>

      <div v-else class="table-container">
        <div class="table-header">
          <h2>Customers ({{ customers.length }})</h2>
        </div>
        <table class="customers-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>First Name</th>
              <th>Last Name</th>
              <th>State</th>
              <th>Email</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="c in customers" :key="c.customerId">
              <td>{{ c.customerId }}</td>
              <td>{{ c.firstName }}</td>
              <td>{{ c.lastName }}</td>
              <td><span class="state-badge">{{ c.state }}</span></td>
              <td><a :href="`mailto:${c.email}`" class="email-link">{{ c.email }}</a></td>
            </tr>
          </tbody>
        </table>
      </div>
    </main>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'

export default {
  setup() {
    const customers = ref([])
    const loading = ref(true)
    const error = ref(null)

    onMounted(async () => {
      loading.value = true
      error.value = null
      try {
        const res = await fetch('/api/customers')
        if (!res.ok) throw new Error(`HTTP ${res.status}`)
        const data = await res.json()

        // Normalize server-side property casing (support PascalCase or camelCase)
        customers.value = (data || []).map((c) => ({
          customerId: c.customerId ?? c.CustomerId,
          firstName: c.firstName ?? c.FirstName,
          lastName: c.lastName ?? c.LastName,
          state: c.state ?? c.State,
          email: c.email ?? c.Email,
        }))
      } catch (e) {
        error.value = 'Failed to load customers'
        // keep customers empty
      } finally {
        loading.value = false
      }
    })

    return { customers, loading, error }
  }
}
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen',
    'Ubuntu', 'Cantarell', 'Fira Sans', 'Droid Sans', 'Helvetica Neue', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  min-height: 100vh;
}

.app {
  min-height: 100vh;
  padding: 2rem;
}

.header {
  text-align: center;
  margin-bottom: 3rem;
}

.header h1 {
  color: white;
  font-size: 2.5rem;
  font-weight: 700;
  text-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.main-content {
  max-width: 1200px;
  margin: 0 auto;
}

.loading, .error {
  background: white;
  border-radius: 12px;
  padding: 3rem;
  text-align: center;
  box-shadow: 0 10px 30px rgba(0,0,0,0.2);
}

.spinner {
  width: 50px;
  height: 50px;
  margin: 0 auto 1rem;
  border: 4px solid #f3f3f3;
  border-top: 4px solid #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.loading p {
  color: #666;
  font-size: 1.1rem;
}

.error {
  background: #fee;
  border: 1px solid #fcc;
}

.error p {
  color: #c33;
  font-weight: 500;
}

.table-container {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  box-shadow: 0 10px 30px rgba(0,0,0,0.2);
}

.table-header {
  margin-bottom: 1.5rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #f0f0f0;
}

.table-header h2 {
  color: #333;
  font-size: 1.5rem;
  font-weight: 600;
}

.customers-table {
  width: 100%;
  border-collapse: collapse;
}

.customers-table thead {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.customers-table th {
  padding: 1rem;
  text-align: left;
  color: white;
  font-weight: 600;
  font-size: 0.875rem;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.customers-table th:first-child {
  border-top-left-radius: 8px;
}

.customers-table th:last-child {
  border-top-right-radius: 8px;
}

.customers-table tbody tr {
  border-bottom: 1px solid #f0f0f0;
  transition: background-color 0.2s ease;
}

.customers-table tbody tr:hover {
  background-color: #f8f9ff;
}

.customers-table tbody tr:last-child {
  border-bottom: none;
}

.customers-table td {
  padding: 1rem;
  color: #333;
  font-size: 0.95rem;
}

.state-badge {
  display: inline-block;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: 12px;
  font-size: 0.875rem;
  font-weight: 600;
}

.email-link {
  color: #667eea;
  text-decoration: none;
  transition: color 0.2s ease;
}

.email-link:hover {
  color: #764ba2;
  text-decoration: underline;
}

@media (max-width: 768px) {
  .app {
    padding: 1rem;
  }

  .header h1 {
    font-size: 2rem;
  }

  .table-container {
    padding: 1rem;
    overflow-x: auto;
  }

  .customers-table {
    font-size: 0.875rem;
  }

  .customers-table th,
  .customers-table td {
    padding: 0.75rem 0.5rem;
  }
}
</style>
