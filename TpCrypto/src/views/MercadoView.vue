<template>
 <div>
    <h1>Mercado</h1>
    <table class="table">
      <thead>
        <tr>
          <th>Codigo</th>
          <th>Cantidad</th>
          <th>Precio Venta</th>
          <th>Precio Compra</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="c in criptos" :key="c.cryptoCode">
          <td>{{ c.cryptoCode }}</td>
          <td>1</td>
          <td>{{ c.price.toLocaleString('es-AR') }}</td>
          <td>{{ c.totalAsk.toLocaleString('es-AR') }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue'

const criptos = ref([]);

onMounted(async function()
{
  const codigos = ["BTC", "ETH", "ADA", "USDC", "SOL", "BNB"];
  for (const codigo of codigos) {
    const precio = await((await fetch(`https://localhost:7076/Prices/${codigo}`)).json());
    criptos.value.push(precio);
  }
})
</script>