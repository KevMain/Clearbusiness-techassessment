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
import { ref } from 'vue'

export default {
  setup() {
    const customers = ref([
      { customerId: 1, firstName: 'Debra', lastName: 'Burks', state: 'NY', email: 'debra.burks@yahoo.com' },
      { customerId: 2, firstName: 'Kasha', lastName: 'Todd', state: 'CA', email: 'kasha.todd@yahoo.com' },
      { customerId: 3, firstName: 'Tameka', lastName: 'Fisher', state: 'CA', email: 'tameka.fisher@aol.com' },
      { customerId: 4, firstName: 'Daryl', lastName: 'Spence', state: 'NY', email: 'daryl.spence@aol.com' }
    ])

    return { customers }
  }
}
</script>

<style>
table { border-collapse: collapse; width: 100%; }
th, td { border: 1px solid #ddd; padding: 8px; }
th { background: #f4f4f4; text-align: left; }
</style>
