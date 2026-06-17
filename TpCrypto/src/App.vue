<template>
  <div>

    <nav v-if="isLogueado()" class="navbar navbar-light bg-success px-3 d-flex justify-content-between">
      
      <span class="navbar-brand text-dark">CriptoApp</span>
      
      <div class="d-flex gap-2">
        <button class="btn btn-sm btn-outline-dark" @click="cerrarSesion">Cerrar sesión</button>
      </div>
    </nav>
    <RouterView />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { isLogueado, getUsuario, logout } from './auth.js'

const router = useRouter()
const usuario = ref(getUsuario())

function cerrarSesion() {
  logout()
  usuario.value = null
  router.push('/login')
}
</script>