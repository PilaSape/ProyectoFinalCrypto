<template>
  <div>
    <div class="container mt-5">
      <h1 class="fw-bold text-center mb-4">Bienvenido a <span class="text-success">CriptoApp</span></h1>
      <div class="row justify-content-center g-3">
        <div class="col-5">
          <RouterLink to="/mercado" class="btn btn-success text-dark fw-bold w-100 py-3">  Ver Mercado </RouterLink>
        </div>
        <div class="col-5">
          <RouterLink to="/portafolio" class="btn btn-success text-dark fw-bold w-100 py-3">  Portafolio </RouterLink>
        </div>
        <div class="col-5">
          <RouterLink to="/transacciones" class="btn btn-success text-dark fw-bold w-100 py-3">  Transacciones </RouterLink>
        </div>
        <div class="col-5">
          <RouterLink to="/ventacompra" class="btn btn-success text-dark fw-bold w-100 py-3">  Compra/Venta </RouterLink>
        </div>
      </div>
    </div>
  </div>
</template>