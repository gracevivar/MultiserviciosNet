# MultiserviciosNet
# Reto 2 - Arquitectura en Capas + DDD (Orden)

Proyecto en **.NET 8** con **arquitectura en capas** y un flujo mínimo **end-to-end**:
- Crear una **Orden**
- Agregar un **Item** a la Orden
- Consultar la **Orden**

---

## Requisitos
- .NET SDK 8 instalado  
- (Opcional) Visual Studio 2022 / VS Code

---

## Cómo ejecutar el proyecto

### 1) Restaurar dependencias
Desde la raíz del repositorio (donde está el `.sln`):

```bash
dotnet restore



Ejecutar la API
dotnet run --project src/RetoTienda.Api

3) Probar en Swagger

Abre en el navegador:

/swagger

La URL exacta (http/https y puerto) se muestra en la consola al ejecutar el proyecto.

Endpoints (flujo completo)
1) Crear Orden

POST /api/ordenes

Body:

{ "customerId": "CUST-001", "currency": "USD" }

2) Agregar Item a la Orden

POST /api/ordenes/{ordenId}/items

Body:

{ "productId": "PROD-ABC", "quantity": 2, "unitPrice": 10.50 }

3) Obtener Orden

GET /api/ordenes/{ordenId}

Flujo mínimo esperado (end-to-end)

El caso de uso recorre las capas:
Presentation (API Controller) → Application (UseCase) → Domain (Orden) → Repository (abstracción) → Infrastructure (in-memory)

Use Cases implementados:

CrearOrdenUseCase

AgregarItemOrdenUseCase

ObtenerOrdenUseCase

Temas implementados
Arquitectura en capas

Solución organizada en proyectos:

RetoTienda.Domain (Dominio puro, sin frameworks)

RetoTienda.Application (Casos de uso / orquestación)

RetoTienda.Infrastructure (Persistencia in-memory)

RetoTienda.Api (Controllers + DI)

Dependencias:

Api → Application

Application → Domain

Infrastructure → Domain

Domain → (nada)

DDD (introducción práctica)

Entidad principal Orden con identidad y comportamiento (no anémica)

Entidad OrdenItem

Value Object Money (valida moneda/monto y controla operaciones)

Reglas de negocio dentro del dominio (ej. solo agregar items en Draft, moneda consistente, consolidación de items)

Patrones aplicados (uso consciente)

Repository: IOrdenRepository como frontera entre dominio y persistencia

Factory: creación de Orden centralizada cuando hay reglas/decisiones iniciales (Id, fecha, estado, moneda)

Dependency Injection: desacoplamiento entre casos de uso y repositorio

Notas / Trade-offs

Persistencia in-memory para mantener el alcance mínimo (los datos se pierden al reiniciar).

No se incluye autenticación, UI ni patrones avanzados (fuera del alcance del reto).