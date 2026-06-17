<template>
  <div>
    <h1>Portafolio</h1>
    <table class="table">
      <thead>
        <tr>
          <th>Cripto</th>
          <th>Cantidad</th>
          <th>Valor en ARS</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="t in portafolio.tenencias" :key="t.cryptoCode">
          <td>{{ t.cryptoCode }}</td>
          <td>{{ t.cantidad }}</td>
          <td class="text-success fw-bold">$ {{ t.valorARS.toLocaleString('es-AR') }}</td>
        </tr>
      </tbody>
    </table>
    <p >Total: <span class="text-success fw-bold">${{ portafolio.totalEnARS.toLocaleString('es-AR') }}</span></p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const portafolio = ref({ tenencias: [], totalEnARS: 0 })

onMounted(async function() 
{
  portafolio.value = await (await fetch("https://localhost:7076/Transactions/portfolio")).json()
})
</script>