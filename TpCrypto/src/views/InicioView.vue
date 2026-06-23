<template>
  <div>
    <div class="container mt-5">
      <h1 class="fw-bold text-center mb-2">Bienvenido a <span class="text-success">CriptoApp</span></h1>
      <p class="text-center text-muted mb-5"><u><span class="fw-bold text-uppercase">{{ usuario?.usuario }}</span></u> Manejá tu cartera de criptomonedas en un solo lugar</p>

      <div class="row justify-content-center g-3">
        <div class="col-5">
          <RouterLink to="/mercado" class="btn btn-outline-success fw-bold w-100 py-4 shadow-sm">
             Ver Mercado
          </RouterLink>
        </div>
        <div class="col-5">
          <RouterLink to="/portafolio" class="btn btn-outline-success fw-bold w-100 py-4 shadow-sm">
             Portafolio
          </RouterLink>
        </div>
        <div class="col-5">
          <RouterLink to="/transacciones" class="btn btn-outline-success fw-bold w-100 py-4 shadow-sm">
             Transacciones
          </RouterLink>
        </div>
        <div class="col-5">
          <RouterLink to="/ventacompra" class="btn btn-outline-success fw-bold w-100 py-4 shadow-sm">
             Compra/Venta
          </RouterLink>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { getUsuario } from '../auth.js'

const usuario = getUsuario()
</script>