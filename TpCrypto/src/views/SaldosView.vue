<template>
  <div>
    <div class="container mt-4">
      <h1>Cargar Saldo</h1>

      <div v-if="mensaje" :class="{ 'alert': true, 'alert-success': mensajeTipo === 'success', 'alert-danger': mensajeTipo === 'danger' }">
        {{ mensaje }}
      </div>

      <form>
        <div class="mb-3">
          <label>Selecciona el metodo de pago</label>
          <select class="form-select" v-model="metodo">
            <option value="">Selecciona un método de pago</option>
            <option value="mercadopago">MercadoPago</option>
            <option value="transferencia">Transferencia bancaria</option>
            <option value="paypal">PayPal</option>
          </select>
          <label class="mt-3">Monto a cargar</label>
          <input type="number" class="form-control" placeholder="Monto en ARS" v-model="monto">
          <button type="button" class="btn btn-primary mt-3" @click="cargarSaldo">Confirmar</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>

import { ref } from 'vue'
import { getUsuario, getClaveSaldo } from '../auth.js'

const saldo = ref(0)
const metodo = ref("")
const monto = ref(0)
const mensaje = ref("")
const mensajeTipo = ref("success")
const guardado = localStorage.getItem(getClaveSaldo())

if (guardado) 
{
  saldo.value = parseFloat(guardado)
} else
{
  localStorage.setItem(getClaveSaldo(), 0)
}

function cargarSaldo() {
  mensaje.value = ""

  if (!metodo.value) {
    mensajeTipo.value = "danger"
    mensaje.value = "Seleccioná un método de pago."
    return
  }
  if (!monto.value || monto.value <= 0) {
    mensajeTipo.value = "danger"
    mensaje.value = "Ingresá un monto válido."
    return
  }

  saldo.value += parseFloat(monto.value)
  localStorage.setItem(getClaveSaldo(), saldo.value)

  mensajeTipo.value = "success"
  mensaje.value = "Saldo cargado con éxito."

  metodo.value = ""
  monto.value = 0
  location.reload()
}
</script>