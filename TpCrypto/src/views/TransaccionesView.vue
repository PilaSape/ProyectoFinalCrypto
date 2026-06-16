

<template>
  <div>
    <table class="table">
      <thead>
        <tr>
          <th>Crypto</th>
          <th>Amount</th>
          <th>Action</th>
          <th>Money</th>
          <th>Date</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="transaccion in transacciones" :key="transaccion.id">
          <td>{{ transaccion.cryptoCode }}</td>
          <td>{{ transaccion.cryptoAmount }}</td>
          <td>{{ transaccion.action }}</td>
          <td>{{ transaccion.money }} €</td>
          <td>{{ transaccion.dateTime }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue'
 const transacciones = ref([]);
 onMounted(async () => {
  transacciones.value = await((await fetch("https://localhost:7076/Transactions")).json());
})
</script>