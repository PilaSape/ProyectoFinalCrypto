<template>
  <div class="min-vh-100 d-flex justify-content-center align-items-center bg-success">
    <div class="card shadow-lg border-0" style="width: 350px;">
      <div class="card-body p-4">
        <h3 class="text-center fw-bold text-success mb-1">CriptoApp</h3>
        <p class="text-center text-muted mb-4">Ingresá a tu cuenta</p>

        <div class="mb-3">
          <label class="form-label">Usuario</label>
          <input type="text" v-model="usuario" placeholder="Usuario" class="form-control">
        </div>

        <div class="mb-3">
          <label class="form-label">Contraseña</label>
          <input type="password" v-model="password" placeholder="Contraseña" class="form-control">
        </div>

        <div v-if="error" class="alert alert-danger py-2">{{ error }}</div>

        <button class="btn btn-success w-100 fw-bold py-2" @click="login">Ingresar</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const usuario = ref("")
const password = ref("")
const error = ref("")
const router = useRouter()

const USUARIOS = [
  { usuario: "admin", password: "admin123", rol: "admin" },
  { usuario: "user", password: "pwd", rol: "usuario" },
  { usuario: "nico", password: "xd", rol: "admin" }
]

function login()
  {
  const encontrado = USUARIOS.find(function(u) { return u.usuario === usuario.value && u.password === password.value })
  if (encontrado) 
  {
    localStorage.setItem("login", JSON.stringify(encontrado))
    router.push("/")
  } else 
  {
    error.value = "Usuario o contraseña incorrectos."
  }
}
</script>