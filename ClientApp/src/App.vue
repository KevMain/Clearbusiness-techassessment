<template>
  <div>
    <h1>Frontend: Hello Vue</h1>
    <p>API Response: {{ message }}</p>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'

export default {
  setup() {
    const message = ref('loading...')

    onMounted(async () => {
      try {
        const res = await fetch('/api/hello')
        const data = await res.json()
        message.value = data.message
      } catch (e) {
        message.value = 'error fetching API'
      }
    })

    return { message }
  }
}
</script>
