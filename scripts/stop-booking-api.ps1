# Detiene el proceso que escucha en el puerto por defecto de la API (evita MSB3021/MSB3027).
$port = 5225
$conn = Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue | Select-Object -First 1
if ($null -ne $conn) {
  Stop-Process -Id $conn.OwningProcess -Force -ErrorAction SilentlyContinue
  Write-Host "Proceso en puerto $port (PID $($conn.OwningProcess)) detenido."
} else {
  Get-Process -Name "Microservicio.Booking.Api" -ErrorAction SilentlyContinue | Stop-Process -Force
  Write-Host "Nada escuchando en $port; proceso Microservicio.Booking.Api detenido si existía."
}
