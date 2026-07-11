<template>
  <div class="app">
    <header class="header">
      <h1>Customer Directory</h1>
      <button v-if="isAuthenticated" @click="logout" class="logout-btn">Logout</button>
    </header>

    <main class="main-content">
      <!-- Login Form -->
      <div v-if="!isAuthenticated" class="login-container">
        <div class="login-card">
          <h2>Login</h2>
          <form @submit.prevent="login">
            <div class="form-group">
              <label for="username">Username</label>
              <input
                id="username"
                v-model="username"
                type="text"
                placeholder="Enter username"
                required
              />
            </div>
            <div class="form-group">
              <label for="password">Password</label>
              <input
                id="password"
                v-model="password"
                type="password"
                placeholder="Enter password"
                required
              />
            </div>
            <button type="submit" class="login-btn" :disabled="loggingIn">
              {{ loggingIn ? 'Logging in...' : 'Login' }}
            </button>
            <div v-if="loginError" class="login-error">
              {{ loginError }}
            </div>
            <div class="login-hint">
              <small>Hint: admin / password</small>
            </div>
          </form>
        </div>
      </div>

      <!-- Customers Grid -->
      <div v-else>
        <!-- Orders View -->
        <div v-if="showOrders">
          <div v-if="loading" class="loading">
            <div class="spinner"></div>
            <p>Loading orders...</p>
          </div>

          <div v-else-if="error" class="error">
            <p>{{ error }}</p>
            <button @click="backToCustomers" class="back-btn" style="margin-top: 1rem;">← Back to Customers</button>
          </div>

          <div v-else class="table-container">
            <div class="table-header">
              <button @click="backToCustomers" class="back-btn">← Back to Customers</button>
              <h2>Orders for {{ selectedCustomer?.firstName }} {{ selectedCustomer?.lastName }} ({{ orders.length }})</h2>
              <div class="customer-total">
                <strong>Customer Total: £{{ customerTotal.toFixed(2) }}</strong>
              </div>
            </div>
            <div v-if="orders.length === 0" class="no-orders">
              <p>No orders found for this customer.</p>
            </div>
            <table v-else class="customers-table">
              <thead>
                <tr>
                  <th>Order ID</th>
                  <th>Order Date</th>
                  <th>Required Date</th>
                  <th>Shipped Date</th>
                  <th>Status</th>
                  <th>Order Total</th>
                </tr>
              </thead>
              <tbody>
                <template v-for="order in orders" :key="order.orderId">
                  <tr class="order-row">
                    <td>{{ order.orderId }}</td>
                    <td>{{ formatDate(order.orderDate) }}</td>
                    <td>{{ formatDate(order.requiredDate) }}</td>
                    <td>{{ formatDate(order.shippedDate) }}</td>
                    <td><span class="status-badge">{{ getStatusText(order.orderStatus) }}</span></td>
                    <td class="order-total">£{{ order.orderTotal.toFixed(2) }}</td>
                  </tr>
                  <tr class="items-row">
                    <td colspan="6">
                      <div class="items-container">
                        <h4>Order Items</h4>
                        <table class="items-table">
                          <thead>
                            <tr>
                              <th>Item ID</th>
                              <th>List Price</th>
                              <th>Discount</th>
                              <th>Final Price</th>
                            </tr>
                          </thead>
                          <tbody>
                            <tr v-for="item in getOrderItems(order.orderId)" :key="item.itemId">
                              <td>{{ item.itemId }}</td>
                              <td>£{{ item.listPrice.toFixed(2) }}</td>
                              <td>£{{ item.discount.toFixed(2) }}</td>
                              <td>£{{ (item.listPrice - item.discount).toFixed(2) }}</td>
                            </tr>
                          </tbody>
                        </table>
                      </div>
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>
          </div>
        </div>

        <!-- Customers List -->
        <div v-else>
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
              <button @click="applyCADiscount" class="apply-discount-btn">
                Apply CA Discount
              </button>
            </div>
            <table class="customers-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>First Name</th>
                  <th>Last Name</th>
                  <th>State</th>
                  <th>Email</th>
                  <th>Orders</th>
                  <th>Total</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="c in customers" :key="c.customerId">
                  <td>{{ c.customerId }}</td>
                  <td>{{ c.firstName }}</td>
                  <td>{{ c.lastName }}</td>
                  <td><span class="state-badge">{{ c.state }}</span></td>
                  <td><a :href="`mailto:${c.email}`" class="email-link">{{ c.email }}</a></td>
                  <td class="order-count">{{ c.orderCount }}</td>
                  <td class="customer-total-cell">£{{ c.total.toFixed(2) }}</td>
                  <td>
                    <button @click="viewOrders(c)" class="orders-btn">View Orders</button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'

export default {
  setup() {
    const customers = ref([])
    const loading = ref(false)
    const error = ref(null)

    // Auth state
    const isAuthenticated = ref(false)
    const token = ref(null)
    const username = ref('')
    const password = ref('')
    const loggingIn = ref(false)
    const loginError = ref(null)

    // Orders state
    const showOrders = ref(false)
    const selectedCustomer = ref(null)
    const orders = ref([])
    const orderItems = ref([])
    const customerTotal = ref(0)

    // Check for existing token on mount
    onMounted(() => {
      const savedToken = localStorage.getItem('jwt_token')
      if (savedToken) {
        token.value = savedToken
        isAuthenticated.value = true
        loadCustomers()
      }
    })

    const login = async () => {
      loggingIn.value = true
      loginError.value = null

      try {
        const res = await fetch('/api/auth/login', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            username: username.value,
            password: password.value
          })
        })

        if (!res.ok) {
          const errorData = await res.json().catch(() => ({}))
          throw new Error(errorData.message || 'Login failed')
        }

        const data = await res.json()
        token.value = data.token
        localStorage.setItem('jwt_token', data.token)
        isAuthenticated.value = true

        // Clear form
        username.value = ''
        password.value = ''

        // Load customers
        await loadCustomers()
      } catch (e) {
        loginError.value = e.message
      } finally {
        loggingIn.value = false
      }
    }

    const logout = () => {
      token.value = null
      localStorage.removeItem('jwt_token')
      isAuthenticated.value = false
      customers.value = []
      error.value = null
      showOrders.value = false
      selectedCustomer.value = null
      orders.value = []
      orderItems.value = []
    }

    const loadCustomers = async () => {
      loading.value = true
      error.value = null
      try {
        const res = await fetch('/api/customers', {
          headers: {
            'Authorization': `Bearer ${token.value}`
          }
        })

        if (!res.ok) {
          if (res.status === 401) {
            // Token expired or invalid
            logout()
            throw new Error('Session expired. Please login again.')
          }
          throw new Error(`HTTP ${res.status}`)
        }

        const data = await res.json()

        // Normalize server-side property casing (support PascalCase or camelCase)
        customers.value = (data || []).map((c) => ({
          customerId: c.customerId ?? c.CustomerId,
          firstName: c.firstName ?? c.FirstName,
          lastName: c.lastName ?? c.LastName,
          state: c.state ?? c.State,
          email: c.email ?? c.Email,
          orderCount: c.orderCount ?? c.OrderCount ?? 0,
          total: c.total ?? c.Total ?? 0,
        }))
      } catch (e) {
        error.value = e.message || 'Failed to load customers'
      } finally {
        loading.value = false
      }
    }

    const viewOrders = async (customer) => {
      selectedCustomer.value = customer
      showOrders.value = true
      loading.value = true
      error.value = null

      try {
        const res = await fetch(`/api/orders/customer/${customer.customerId}`, {
          headers: {
            'Authorization': `Bearer ${token.value}`
          }
        })

        if (!res.ok) {
          if (res.status === 401) {
            logout()
            throw new Error('Session expired. Please login again.')
          }
          throw new Error(`HTTP ${res.status}`)
        }

        const data = await res.json()

        // API now returns { Orders: [...], CustomerTotal: number }
        customerTotal.value = data.customerTotal ?? data.CustomerTotal ?? 0

        // Normalize server-side property casing
        orders.value = (data.orders ?? data.Orders ?? []).map((o) => ({
          orderId: o.orderId ?? o.OrderId,
          orderDate: o.orderDate ?? o.OrderDate,
          requiredDate: o.requiredDate ?? o.RequiredDate,
          shippedDate: o.shippedDate ?? o.ShippedDate,
          orderStatus: o.orderStatus ?? o.OrderStatus,
          orderTotal: o.orderTotal ?? o.OrderTotal ?? 0,
        }))

        // Fetch items for all orders
        await loadOrderItems(orders.value)
      } catch (e) {
        error.value = e.message || 'Failed to load orders'
        orders.value = []
        orderItems.value = []
        customerTotal.value = 0
      } finally {
        loading.value = false
      }
    }

    const loadOrderItems = async (ordersList) => {
      try {
        const itemPromises = ordersList.map(order =>
          fetch(`/api/orders/${order.orderId}/items`, {
            headers: {
              'Authorization': `Bearer ${token.value}`
            }
          }).then(res => {
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return res.json()
          })
        )

        const itemsArrays = await Promise.all(itemPromises)

        // Flatten and normalize all items
        const allItems = itemsArrays.flat().map((item) => ({
          orderId: item.orderId ?? item.OrderId,
          itemId: item.itemId ?? item.ItemId,
          listPrice: item.listPrice ?? item.ListPrice,
          discount: item.discount ?? item.Discount,
        }))

        orderItems.value = allItems
      } catch (e) {
        console.error('Failed to load order items:', e)
        orderItems.value = []
      }
    }

    const backToCustomers = () => {
      showOrders.value = false
      selectedCustomer.value = null
      orders.value = []
      orderItems.value = []
      customerTotal.value = 0
    }

    const getOrderItems = (orderId) => {
      return orderItems.value.filter(item => item.orderId === orderId)
    }

    const formatDate = (dateString) => {
      if (!dateString) return 'N/A'
      const date = new Date(dateString)
      return date.toLocaleDateString('en-US', { 
        year: 'numeric', 
        month: 'short', 
        day: 'numeric' 
      })
    }

    const getStatusText = (status) => {
      const statuses = {
        1: 'Pending',
        2: 'Processing',
        3: 'Shipped',
        4: 'Delivered',
        5: 'Cancelled'
      }
      return statuses[status] || 'Unknown'
    }

    const applyCADiscount = async () => {
      loading.value = true
      error.value = null

      try {
        const res = await fetch('/api/discount/apply-ca', {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${token.value}`
          }
        })

        if (!res.ok) {
          if (res.status === 401) {
            logout()
            throw new Error('Session expired. Please login again.')
          }
          throw new Error(`HTTP ${res.status}`)
        }

        // Reload customers to show updated totals
        await loadCustomers()
      } catch (e) {
        error.value = e.message || 'Failed to apply discount'
      } finally {
        loading.value = false
      }
    }

    return {
      customers, 
      loading, 
      error,
      isAuthenticated,
      username,
      password,
      loggingIn,
      loginError,
      login,
      logout,
      showOrders,
      selectedCustomer,
      orders,
      orderItems,
      customerTotal,
      viewOrders,
      backToCustomers,
      getOrderItems,
      formatDate,
      getStatusText,
      applyCADiscount
    }
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
  position: relative;
}

.header h1 {
  color: white;
  font-size: 2.5rem;
  font-weight: 700;
  text-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.logout-btn {
  position: absolute;
  top: 0;
  right: 0;
  background: rgba(255, 255, 255, 0.2);
  color: white;
  border: 1px solid rgba(255, 255, 255, 0.3);
  padding: 0.5rem 1rem;
  border-radius: 6px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: all 0.2s ease;
}

.logout-btn:hover {
  background: rgba(255, 255, 255, 0.3);
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
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.table-header h2 {
  color: #333;
  font-size: 1.5rem;
  font-weight: 600;
}

.apply-discount-btn {
  padding: 0.75rem 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.apply-discount-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(102, 126, 234, 0.3);
}

.apply-discount-btn:active:not(:disabled) {
  transform: translateY(0);
}

.apply-discount-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.customer-total {
  margin-top: 1rem;
  padding: 0.75rem 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 8px;
  font-size: 1.125rem;
  text-align: right;
}

.order-total {
  font-weight: 600;
  color: #667eea;
}

.order-count {
  text-align: center;
  font-weight: 600;
}

.customer-total-cell {
  font-weight: 600;
  color: #667eea;
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

.customers-table tbody tr.items-row {
  background-color: #fafbff;
}

.customers-table tbody tr.items-row:hover {
  background-color: #fafbff;
}

.customers-table tbody tr:last-child {
  border-bottom: none;
}

.items-container {
  padding: 1rem 2rem;
  background: white;
}

.items-container h4 {
  margin-bottom: 1rem;
  color: #667eea;
  font-size: 1rem;
}

.items-table {
  width: 100%;
  border-collapse: collapse;
  margin-left: 2rem;
}

.items-table thead {
  background: linear-gradient(135deg, rgba(102, 126, 234, 0.1) 0%, rgba(118, 75, 162, 0.1) 100%);
}

.items-table th {
  padding: 0.75rem;
  text-align: left;
  color: #667eea;
  font-weight: 600;
  font-size: 0.875rem;
  border-bottom: 2px solid #667eea;
}

.items-table td {
  padding: 0.75rem;
  color: #555;
  font-size: 0.9rem;
  border-bottom: 1px solid #f0f0f0;
}

.items-table tbody tr:last-child td {
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

.status-badge {
  display: inline-block;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: 12px;
  font-size: 0.875rem;
  font-weight: 600;
}

.orders-btn {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  font-size: 0.875rem;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.orders-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.back-btn {
  background: rgba(102, 126, 234, 0.1);
  color: #667eea;
  border: 1px solid #667eea;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  font-size: 0.875rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  margin-bottom: 1rem;
}

.back-btn:hover {
  background: rgba(102, 126, 234, 0.2);
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

.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 60vh;
}

.login-card {
  background: white;
  border-radius: 12px;
  padding: 2.5rem;
  box-shadow: 0 10px 30px rgba(0,0,0,0.2);
  width: 100%;
  max-width: 400px;
}

.login-card h2 {
  color: #333;
  margin-bottom: 1.5rem;
  text-align: center;
  font-size: 1.75rem;
}

.form-group {
  margin-bottom: 1.25rem;
}

.form-group label {
  display: block;
  color: #555;
  font-weight: 500;
  margin-bottom: 0.5rem;
  font-size: 0.95rem;
}

.form-group input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.2s ease;
}

.form-group input:focus {
  outline: none;
  border-color: #667eea;
}

.login-btn {
  width: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  padding: 0.875rem;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.login-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.login-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.login-error {
  margin-top: 1rem;
  padding: 0.75rem;
  background: #fee;
  border: 1px solid #fcc;
  border-radius: 6px;
  color: #c33;
  font-size: 0.9rem;
  text-align: center;
}

.login-hint {
  margin-top: 1rem;
  text-align: center;
  color: #888;
}

.no-orders {
  text-align: center;
  padding: 2rem;
  color: #666;
  font-size: 1.1rem;
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
