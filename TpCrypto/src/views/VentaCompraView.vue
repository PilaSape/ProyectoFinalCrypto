<template>
 <div>
    <h1>Compra/Venta</h1>
    <select class="form-select" v-model="accion">
      <option selected>Selecciona una acción</option>
      <option value="purchase">Comprar</option>
      <option value="sale">Vender</option>
    </select>
    
    <!-- si es compra muestro todas las cryptos, si es venta solo las que tengo -->
    <select class="form-select" v-model="criptomoneda">
      <option selected>Selecciona una criptomoneda</option>

      <template v-if="accion === 'purchase'">
        <option value="BTC">Bitcoin</option>
        <option value="ETH">Ethereum</option>
        <option value="ADA">Cardano</option>
      </template>

      <template v-else-if="accion === 'sale'">
        <option v-for="t in portafolio.tenencias" :key="t.cryptoCode" :value="t.cryptoCode">
          {{ t.cryptoCode }}
        </option>
      </template>

    </select>
    
    <input type="number" class="form-control" placeholder="Cantidad" v-model="cantidad">

    <button class="btn btn-primary" @click="enviar">Confirmar</button>
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue'

const accion = ref("");
const criptomoneda = ref("");
const cantidad = ref(0);
const portafolio = ref({ tenencias: [], totalEnPesos: 0 });

onMounted(async () => {
  portafolio.value = await((await fetch("https://localhost:7076/Transactions/portfolio")).json());
})

async function enviar() {
  await fetch("https://localhost:7076/Transactions", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      cryptoCode: criptomoneda.value,
      action: accion.value,
      cryptoAmount: cantidad.value,
      dateTime: new Date().toISOString()
    })
  })
}
</script>