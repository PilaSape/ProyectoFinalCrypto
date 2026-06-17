<template>
  <div class="container mt-4">
    <h1>Transacciones</h1>
    <table class="table">
      <thead>
        <tr>
          <th>Crypto</th>
          <th>Cantidad</th>
          <th>Acción</th>
          <th>Total (ARS)</th>
          <th>Hora de transaccion</th>
          <!-- columna de acciones solo visible si es admin -->
          <th v-if="isAdmin()">Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="transaccion in transacciones" :key="transaccion.id">
          <td>{{ transaccion.cryptoCode }}</td>
          <td>{{ transaccion.cryptoAmount }}</td>
          <td>
              <span v-if="transaccion.action === 'purchase'" class="badge bg-success">Compra</span>
              <span v-else class="badge bg-danger">Venta</span>
          </td>
          <td>{{ transaccion.money }} ARS</td>
          <td>{{ new Date(transaccion.dateTime).toLocaleString('es-AR') }}</td>
          <!-- botones de editar y borrar solo si es admin -->
          <td v-if="isAdmin()">
            <button class="btn btn-sm btn-warning me-1" @click="editar(transaccion)">Editar</button>
            <button class="btn btn-sm btn-danger" @click="confirmarBorrar(transaccion)">Borrar</button>
          </td>
        </tr>
      </tbody>
    </table>

    <div v-if="editando" class="modal d-block bg-dark bg-opacity-50">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Editar transacción</h5>
            <button class="btn-close" @click="editando = null"></button>
          </div>
          <div class="modal-body">
            <label class="form-label">Cripto</label>
            <input class="form-control mb-2" v-model="editando.cryptoCode">
            <label class="form-label">Cantidad</label>
            <input class="form-control mb-2" type="number" v-model="editando.cryptoAmount">
            <label class="form-label">Monto (ARS)</label>
            <input class="form-control mb-2" type="number" v-model="editando.money">
          </div>
          <div class="modal-footer">
            <button class="btn btn-secondary" @click="editando = null">Cancelar</button>
            <button class="btn btn-primary" @click="guardarEdicion">Guardar</button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="paraBorrar" class="modal d-block bg-dark bg-opacity-50">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Confirmar borrado</h5>
            <button class="btn-close" @click="paraBorrar = null"></button>
          </div>
          <div class="modal-body">
            <p>¿Seguro que querés borrar la transacción de <strong>{{ paraBorrar.cryptoCode }}</strong>?</p>
          </div>
          <div class="modal-footer">
            <button class="btn btn-secondary" @click="paraBorrar = null">Cancelar</button>
            <button class="btn btn-danger" @click="borrar">Borrar</button>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { isAdmin } from '../auth.js'

const transacciones = ref([])
const editando = ref(null)    // guarda la transaccion que se está editando
const paraBorrar = ref(null)  // guarda la transaccion que se quiere borrar

onMounted(async function() 
{
  await cargarTransacciones()
})

// trae todas las transacciones del backend
async function cargarTransacciones() 
{
  const res = await fetch("https://localhost:7076/Transactions")
  transacciones.value = await res.json()
}

// copia la transaccion para no modificar la tabla mientras se edita
function editar(t) 
{
  editando.value = { ...t }
}

function confirmarBorrar(t)
{
  paraBorrar.value = t
}

// manda el PATCH al backend y recarga la tabla
async function guardarEdicion() 
{
  await fetch(`https://localhost:7076/Transactions/${editando.value.id}`, 
  {
    method: "PATCH",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      cryptoCode: editando.value.cryptoCode,
      cryptoAmount: Number(editando.value.cryptoAmount),
      money: Number(editando.value.money)
    })
  })
  editando.value = null
  await cargarTransacciones()
}

// manda el DELETE al backend y recarga la tabla
async function borrar()
{
  await fetch(`https://localhost:7076/Transactions/${paraBorrar.value.id}`, {
    method: "DELETE"
  })
  paraBorrar.value = null
  await cargarTransacciones()
}
</script>