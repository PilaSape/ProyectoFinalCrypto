<template>
  <div class="container mt-4">
    <h1 class="fw-bold text-center mb-4 text-success">COMPRAR O VENDER</h1>
    <p class="text-center text-muted">Saldo disponible: $ {{ saldo.toLocaleString('es-AR') }}</p>
    
    <div v-if="mensaje" :class="{ 'alert': true, 'alert-success': mensajeTipo === 'success', 'alert-danger': mensajeTipo === 'danger' }">
      {{ mensaje }}
    </div>

    <div class="row justify-content-center">
      <div class="col-6">

        
        <select class="form-select mb-3" v-model="accion">
          <option value="" disabled>Selecciona una acción</option>
          <option value="purchase">Comprar</option>
          <option value="sale">Vender</option>
        </select>

        
        <select class="form-select mb-3" v-model="criptomoneda">
          <option value="" disabled>Selecciona una criptomoneda</option>
          <template v-if="accion === 'purchase'">
            <option value="BTC">Bitcoin</option>
            <option value="ETH">Ethereum</option>
            <option value="ADA">Cardano</option>
            <option value="USDC">USDC</option>
            <option value="SOL">Solana</option>
            <option value="BNB">BNB</option>
          </template>
          
          <template v-else-if="accion === 'sale'">
            <option v-for="t in portafolio.tenencias" :key="t.cryptoCode" :value="t.cryptoCode">
              {{ t.cryptoCode }} ({{ t.cantidad }} disponibles)
            </option>
          </template>
        </select>


        <input type="number" class="form-control mb-3" placeholder="Cantidad" v-model="cantidad" min="0" step="any">


        <div v-if="precioUnitario && cantidad > 0" class="mb-3">
          <p>Precio unitario: $ {{ precioUnitario.toLocaleString('es-AR') }}</p>
          <p><strong>Total de {{ cantidad }} {{ criptomoneda }}: $ {{ (precioUnitario * cantidad).toLocaleString('es-AR') }}</strong></p>
        </div>

        <button class="btn btn-primary" @click="enviar" :disabled="cargando">
          <span v-if="cargando">Procesando...</span>
          <span v-else>Confirmar</span>
        </button>

      </div>
    </div>
  </div>
</template>

<script setup>

import { ref, onMounted, watch } from 'vue'
import { isLogueado, getUsuario, logout, getClaveSaldo } from '../auth.js'

const accion = ref("")
const criptomoneda = ref("")
const cantidad = ref("")
const portafolio = ref({ tenencias: [], totalEnARS: 0 })
const mensaje = ref("")
const mensajeTipo = ref("success")
const cargando = ref(false)
const precioUnitario = ref(null)
const saldo = ref(0)
onMounted(async function() 
{
  portafolio.value = await (await fetch("https://localhost:7076/Transactions/portfolio")).json()
  cargarSaldoDesdeStorage()
})

async function enviar() 
{
  mensaje.value = ""

  if (!accion.value) 
  {
    mensajeTipo.value = "danger"
    mensaje.value = "Seleccioná una acción."
    return
  }
  else if (!criptomoneda.value) 
  {
    mensajeTipo.value = "danger"
    mensaje.value = "Seleccioná una criptomoneda."
    return
  }
  else if (!cantidad.value || Number(cantidad.value) <= 0) 
  {
    mensajeTipo.value = "danger"
    mensaje.value = "La cantidad debe ser mayor a 0."
    return
  }
  

  cargando.value = true
  if(accion.value === 'purchase' && (precioUnitario.value * cantidad.value) > saldo.value)
  {
    mensajeTipo.value = "danger"
    mensaje.value = "No tenes saldo suficiente para comprar esa cantidad."
    cargando.value = false
    return
  }
  try 
  {
    const res = await fetch("https://localhost:7076/Transactions", 
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify
      ({
        cryptoCode: criptomoneda.value,
        action: accion.value,
        cryptoAmount: Number(cantidad.value),
        dateTime: new Date().toISOString()
      })
    })

    if (res.ok) // si el back responde bien aca, entonces muestro mensaje de éxito y limpio el form
    {
      mensajeTipo.value = "success"
      textoMensaje()
      if(accion.value==="purchase")
      {
      saldo.value = saldo.value - (precioUnitario.value * cantidad.value) 
      localStorage.setItem(getClaveSaldo(), saldo.value)
      }
      else
      {
          saldo.value = saldo.value + (precioUnitario.value * cantidad.value) 
          localStorage.setItem(getClaveSaldo(), saldo.value)
      }
      accion.value = ""
      criptomoneda.value = ""
      cantidad.value = ""
      // actualizo portafolio para que la lista de venta quede actualizada
      portafolio.value = await (await fetch("https://localhost:7076/Transactions/portfolio")).json()
      location.reload()
    }
    else 
    {
      const error = await res.text()
      mensajeTipo.value = "danger"
      mensaje.value = `Error: ${error}`
    }
  } 
  catch (error)//si no se conecto a la bdd
  {
    mensajeTipo.value = "danger"
    mensaje.value = "No se pudo conectar con el servidor."
  } 
  finally 
  {
    cargando.value = false
  }
}

function textoMensaje() 
{
  if (accion.value === 'purchase') 
  {
    mensaje.value = "Compra realizada con éxito."
  } else 
  {
    mensaje.value = "Venta realizada con éxito."
  }
}

function cargarSaldoDesdeStorage() 
{
  const guardado = localStorage.getItem(getClaveSaldo())
  if (guardado)
  {
    saldo.value = parseFloat(guardado)
  } 
  else 
  {
    saldo.value = 0
  }
}

watch(criptomoneda, async function(nuevaCripto) 
{
  if (!nuevaCripto) return
  const res = await fetch(`https://localhost:7076/Prices/${nuevaCripto}`)
  const data = await res.json()
  precioUnitario.value = data.totalAsk
})
</script>