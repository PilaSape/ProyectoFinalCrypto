<template>
  <div>
    <h1>Portafolio</h1>
    <table class="table">
      <thead>
        <tr>
          <th>Cripto</th>
          <th>Cantidad</th>
          <th>Valor en €</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="t in portafolio.tenencias" :key="t.cryptoCode">
          <td>{{ t.cryptoCode }}</td>
          <td>{{ t.cantidad }}</td>
          <td>{{ t.valorARS }}</td>
        </tr>
      </tbody>
    </table>
    <p>Total: {{ portafolio.totalEnPesos }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const portafolio = ref({ tenencias: [], totalEnPesos: 0 })

onMounted(async () => {
  portafolio.value = await (await fetch("https://localhost:7076/Transactions/portfolio")).json()
})
</script>