<template>
  <div>
    <h1>Customers</h1>
    <table>
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
          <td>{{ c.state }}</td>
          <td>{{ c.email }}</td>
        </tr>
      </tbody>
    </table>
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
table { border-collapse: collapse; width: 100%; }
th, td { border: 1px solid #ddd; padding: 8px; }
th { background: #f4f4f4; text-align: left; }
</style>
