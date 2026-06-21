export function getUsuario() 
{
  const data = localStorage.getItem("login")
  return data ? JSON.parse(data) : null
}

export function isAdmin() 
{
  const u = getUsuario()
  return u?.rol === "admin"
}

export function isLogueado() 
{
  return getUsuario() !== null
}

export function logout() 
{
  localStorage.removeItem("login")
}

export function getClaveSaldo() 
{
  const user = getUsuario()
  if(user === null)
  {
     return "saldo_invitado"
  }
  return "saldo_" + user.usuario
}