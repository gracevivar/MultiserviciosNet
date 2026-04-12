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
******************************************************************************************************************
******************************************************************************************************************
*******************************************RETO  3*****************************************************************
*****************************************************************************************************************
********************************************************************************************************************

---

## Contenerización (Docker) y Orquestación (Kubernetes)

### Decisiones arquitectónicas

1. **Dockerfile multi-stage**
   - Se utilizó un Dockerfile multi-stage (SDK para build / ASP.NET runtime para ejecución) para reducir el tamaño final de la imagen y separar build de runtime.
   - La API se expone en el puerto **8080** dentro del contenedor mediante `ASPNETCORE_URLS=http://+:8080`.

2. **docker-compose con 2 servicios**
   - Se definieron **dos servicios** en `docker-compose.yml`:
     - `ordenes-api`: servicio principal (API)
     - `ordenes-api-peer`: segunda instancia del mismo servicio para cumplir el requisito de 2 contenedores y demostrar comunicación interna.
   - Se agregó un endpoint interno `GET /internal/ping-peer` que realiza una llamada HTTP desde `ordenes-api` hacia `ordenes-api-peer` usando el DNS interno de Docker Compose (`http://ordenes-api-peer:8080/health`), evidenciando comunicación contenedor → contenedor.

3. **Kubernetes (Deployment + Service NodePort)**
   - Se creó un `Deployment` para administrar el ciclo de vida de los Pods y habilitar el escalado.
   - Se expone el servicio mediante `Service` tipo **NodePort** (por simplicidad en entorno local), usando el puerto **30080** para acceder desde el host.
   - Se configuraron **readinessProbe** y **livenessProbe** apuntando al endpoint `GET /health` para soportar:
     - detección de disponibilidad (readiness)
     - self-healing ante fallos (liveness)

4. **Escalado y self-healing**
   - Se escala el deployment a **3 réplicas** para evidenciar alta disponibilidad.
   - Se elimina manualmente un Pod para demostrar que Kubernetes recrea el Pod automáticamente (self-healing) manteniendo el “desired state” del deployment.

---

### Trade-offs asumidos

1. **Sin persistencia real (in-memory)**
   - La persistencia es in-memory, por lo que la información se pierde al reiniciar contenedores o Pods.
   - Se decidió así para mantener el alcance mínimo y enfocarse en contenerización/orquestación.

2. **NodePort en lugar de Ingress/LoadBalancer**
   - Se utiliza NodePort por simplicidad en Kubernetes local.
   - Trade-off: No es el patrón ideal en producción; normalmente se usaría Ingress + Controller o LoadBalancer según el entorno.

3. **Dos servicios idénticos en docker-compose**
   - Se usan dos instancias del mismo servicio para cumplir el requisito de “dos servicios” y demostrar comunicación.
   - Trade-off: En un escenario real, el segundo servicio normalmente sería un servicio distinto (ej. catálogo, pagos, etc.), pero el reto no exige microservicios completos.

4. **Sin configuración avanzada de observabilidad**
   - No se incluyeron logs centralizados, métricas o tracing distribuido por alcance del reto.
   - Se priorizó demostrar despliegue, escalado y recuperación automática.

---

### Evidencias sugeridas (para adjuntar en entrega)
- Salida de `docker build` (imagen construida).
- Logs de `docker compose up` mostrando respuesta de `/internal/ping-peer`.
- Salida de `kubectl get pods` y `kubectl get svc`.
- Prueba de acceso a `http://localhost:30080/health` y `/swagger`.
- Salida de `kubectl scale deployment ordenes-api --replicas=3`.
- Captura/registro de `kubectl delete pod ...` y `kubectl get pods -w` mostrando recreación automática.


###############################################################################################################################################################################
###############################################################################################################################################################################
###############################################################################################################################################################################

## Reto 4 - Evolución hacia una arquitectura de microservicios

###############################################################################################################################################################################
###############################################################################################################################################################################
###############################################################################################################################################################################

### Arquitectura
Componentes:
- **RetoTienda.Gateway** (API Gateway con YARP)
- **RetoTienda.Api** (Orders API: crea órdenes y publica eventos)
- **RabbitMQ** (Message Broker)
- **RetoTienda.Worker** (Worker Service: consume eventos)

Flujo mínimo (evidenciado):
**Cliente → Gateway → API → RabbitMQ → Worker**

### Comunicación
- **Gateway → API (HTTP)**: YARP enruta `/orders/*` hacia la API.
- **API → Worker (Eventos)**: La API publica el evento `OrderCreated` a RabbitMQ y el Worker lo consume desde una cola.

### Contratos
- **RetoTienda.Contracts** contiene el contrato del evento:
  - `OrderCreated(OrderId, CustomerId, CreatedAtUtc)`

  ### Cómo correr (Docker Compose)
Desde la raíz del repositorio:

```bash
docker compose up --build -d
docker compose ps

 ### Servicios expuestos:

 ### Gateway: http://localhost:8080
 ### API (directo): http://localhost:8081
 ### RabbitMQ UI: http://localhost:15672 (user/pass: guest/guest)

 ###Crear orden vía Gateway:


Invoke-RestMethod -Method Post -Uri "http://localhost:8080/orders" -ContentType "application/json" -Body '{ "clienteId":"CUST-001", "moneda":"USD" }'

 ###Verificar consumo del evento en el Worker:
docker compose logs -f retotienda-worker

---

## 3) Agrega “Decisiones técnicas” (por qué usaste cada cosa)

```md
### Decisiones técnicas

1. **API Gateway con YARP**
   - Se utilizó **YARP** por ser una solución oficial/estándar en .NET para reverse proxy y gateway, basada en configuración (Routes/Clusters) y fácil de demostrar en un entorno local.

2. **Mensajería con RabbitMQ**
   - Se utilizó **RabbitMQ** como broker por ser liviano, ampliamente usado y soportar el patrón pub/sub y colas de trabajo.

3. **Publicación/consumo de eventos con MassTransit**
   - Se utilizó **MassTransit** para simplificar la integración con RabbitMQ (publicación de eventos y configuración de consumers) reduciendo boilerplate.

4. **Separación de contratos**
   - Se creó **RetoTienda.Contracts** para compartir el contrato del evento `OrderCreated` entre la API y el Worker, evitando duplicación y manteniendo tipado fuerte.

   ### Trade-offs asumidos

1. **Sin Outbox / Garantía de entrega**
   - El evento `OrderCreated` se publica directamente tras crear la orden (best-effort).
   - En producción se recomienda patrón **Outbox** para evitar pérdida de eventos ante fallos entre BD y broker.

2. **Persistencia simple**
   - La persistencia se mantiene simple (in-memory o mínima) para enfocarse en el objetivo del reto: gateway + eventos + worker.

3. **Gateway básico**
   - El Gateway se limita a enrutar rutas (sin autenticación, rate limiting, retries, etc.) por alcance del reto.

4. **Observabilidad mínima**
   - No se incluyeron métricas/tracing centralizado; la evidencia se realiza con logs del Worker y Docker Compose.

   ### Evidencias
- Captura/salida de `docker compose ps` mostrando los 4 servicios arriba.
- Respuesta del POST al Gateway.
- Logs del Worker consumiendo el evento `OrderCreated`.
- (Opcional) Captura de RabbitMQ UI mostrando broker activo.