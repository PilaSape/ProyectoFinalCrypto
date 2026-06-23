<template>
  <div>

    <nav v-if="logueado" class="navbar navbar-light bg-success px-3 d-flex justify-content-between">
      
      <RouterLink to="/" class="navbar-brand text-dark fw-bold">Inicio </RouterLink>
      
      <div class="d-flex gap-2">
        <span class="text-dark fw-bold d-flex align-items-center">$ {{ saldo.toLocaleString('es-AR') }}</span>
        <RouterLink to="/saldos" class="btn btn-dark">Cargar Saldo</RouterLink>
        <button class="btn btn-sm btn-danger" @click="cerrarSesion">Cerrar sesión</button>
      </div>
    </nav>
    <RouterView />
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { isLogueado, getUsuario, logout, getClaveSaldo } from './auth.js'
const router = useRouter()
const route = useRoute()
const usuario = ref(getUsuario())

const saldo = ref(0)
const logueado = ref(isLogueado())

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

cargarSaldoDesdeStorage()


watch(function() { return route.path }, function() 
{
  cargarSaldoDesdeStorage()
  logueado.value = isLogueado()
})

function cerrarSesion()
 {
  logout()
  usuario.value = null
  logueado.value = false
  router.push('/login')
}
</script>