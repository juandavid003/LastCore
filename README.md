## Aplicación de Administración de Usuarios, Roles, Consumos, Productos y Especialistas
Esta aplicación está diseñada como una solución integral para administrar usuarios, roles, consumos de productos, inventario, especialistas y sus especialidades, proporcionando un sistema de gestión flexible y seguro. El sistema permite realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) con validaciones y control de acceso para garantizar la seguridad y la consistencia de los datos. Cada funcionalidad está cuidadosamente organizada para cumplir con las necesidades de un sistema administrativo, en donde diferentes roles pueden acceder a distintas secciones según sus permisos.

El objetivo principal es implementar una arquitectura robusta de administración, utilizando Angular en el frontend, ASP.NET para el backend y SQL para gestionar los datos. La aplicación está desplegada en AWS, con RDS para la base de datos y EC2 para alojar tanto el frontend como el backend, permitiendo un acceso remoto y seguro.

## Descripción del Proyecto
Este proyecto tiene como propósito central ofrecer una plataforma administrativa con varias funcionalidades:

Usuarios y Roles: Permite gestionar usuarios de diferentes roles y estados. Solo los usuarios con privilegios específicos pueden realizar ciertas acciones, como la creación y eliminación de otros usuarios.
Consumos y Productos: Gestión de inventario, con capacidad para registrar consumos de productos y realizar un ajuste automático de la cantidad disponible.
Especialistas y Especialidades: Permite asignar especialidades a especialistas, asegurando que las asignaciones no tengan solapamientos en sus horarios y que solo especialistas activos sean asignados a consultas.
Además de estas funcionalidades básicas, el proyecto incluye un sistema de validación avanzada para la seguridad de la información y protección de rutas, lo que asegura que únicamente los usuarios autenticados y autorizados puedan acceder a determinadas áreas de la aplicación.

## ¿Por qué estas tecnologías?
Angular/Node.js: Elegido por su estructura modular, facilidad de reutilización de componentes y fuerte soporte de TypeScript.
.NET: Usado en el backend debido a su naturaleza asincrónica y escalabilidad.
SQL: Elegido por su facilidad de integración con .NET.
AWS: Utilizado como un desafío personal para experimentar la complejidad de desplegar en una plataforma conocida (versión de prueba de 7 días).

## Cambios y Validaciones Recientes

### Validaciones Implementadas en Módulos

Cada módulo cuenta con validaciones específicas para garantizar la precisión y seguridad de los datos.

#### Módulo de Administración de Usuarios y Roles

- **Validación de Edad Mínima (18 años):** Solo se permiten registros de usuarios mayores de 18 años.
- **Requisitos de Contraseña Segura:** Las contraseñas deben tener al menos 8 caracteres, una letra mayúscula y un número para asegurar la protección de las cuentas.
- **Validación de Estado y Rol:** Solo usuarios activos y con roles específicos pueden acceder a funciones administrativas.
- **Validación de Campos Obligatorios:** Datos esenciales como nombre, apellido, rol y estado son obligatorios al crear o modificar usuarios.
- **Protección de Rutas por Rol:** Se restringe el acceso a áreas específicas de la aplicación según el rol del usuario, asegurando que solo ciertos usuarios puedan ejecutar determinadas acciones.

#### Módulo de Consumos y Productos

- **Validación de Inventario y Cantidad Disponible:** Antes de registrar un consumo, se verifica que la cantidad disponible del producto sea suficiente.
- **Validación de Campos Requeridos:** Cada producto debe tener nombre, costo, precio de venta y cantidad disponible.
- **Ajuste Automático de Inventario:** Al crear, editar o eliminar un consumo, la cantidad de inventario se ajusta en tiempo real para reflejar cambios precisos.
- **Prevención de Duplicación de Productos:** Se verifica que los productos sean únicos en base a su nombre, evitando duplicaciones.
- **Restricciones para Productos Consumidos:** Los productos que ya han sido consumidos no pueden eliminarse o modificarse para mantener la integridad del historial.

#### Módulo de Especialistas y Especialidades

- **Validación de Edad Mínima para Especialistas (18 años):** Solo se permite el registro de especialistas mayores de 18 años.
- **Asignación Única de Especialidades a Especialistas:** No se permite que un especialista tenga asignada la misma especialidad más de una vez.
- **Validación de Campos Esenciales:** Los campos de nombre, especialidad, horario y estatus deben completarse.
- **Control de Solapamiento de Horarios:** Los horarios de especialistas no pueden solaparse, asegurando disponibilidad sin conflicto.
- **Validación del Estado del Especialista:** Solo especialistas activos pueden ser asignados a citas.
- **Restricción de Eliminación de Especialidades Asignadas:** Las especialidades asignadas no pueden eliminarse si están asociadas a citas.




## Retos y Funcionalidades Futuras
CRUD de Tratamientos: Se implementará un módulo de tratamientos, con validaciones para asegurar la integridad de los datos y el cumplimiento de las reglas de negocio.
Ajustes en Vistas por Rol: La interfaz de usuario se personalizará para que cada rol (administrador, supervisor o especialista) vea solo las opciones correspondientes a sus permisos, mejorando así la usabilidad y seguridad.

## Tabla de Contenidos
Cómo Instalar y Ejecutar el Proyecto
Pasos de Instalación
Cómo Usar el Proyecto
Créditos
Licencia

## Cómo Instalar y Ejecutar el Proyecto
Prerrequisitos
Antes de ejecutar el proyecto, asegúrate de tener instalados los siguientes elementos:

Node.js (versión 16.x o superior)
Angular CLI (versión 18.x o superior)
.NET SDK (versión 6 o superior)
SQL Server o MySQL para la base de datos
Git para clonar el repositorio

## Pasos de Instalación
Clona el repositorio: git clone https://github.com/juandavid003/Proyectos-2024.git
Navega al directorio del proyecto:cd proyecto-crud
Instala las dependencias necesarias: npm install
Abre la aplicación en el navegador: http://localhost:4200

## Instalación y Ejecución del Backend en ASP.NET
Clona el repositorio: git clone https://github.com/juandavid003/Proyectos-2024.git

Abre appsettings.json y configura la cadena de conexión a tu base de datos SQL. Un ejemplo:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TuBaseDeDatos;User Id=tuUsuario;Password=tuContraseña;"
  }
}

Abre https://localhost:44372/ en el navegador

## Cómo Usar el Proyecto

El proyecto está dividido en módulos que permiten a los usuarios realizar acciones específicas, dependiendo de sus permisos.

### Módulo de Administración de Usuarios y Roles

- **Iniciar Sesión:** Los usuarios deben ingresar con un nombre de usuario y contraseña válidos, cumpliendo con los requisitos de seguridad. Solo los usuarios activos y con permisos de administrador pueden acceder a la gestión de usuarios.
- **Crear Usuario:** En la sección "Crear", el administrador puede agregar nuevos usuarios, asegurándose de cumplir con validaciones como edad mínima y requisitos de contraseña.
- **Ver y Leer Usuarios:** La lista de usuarios registrados está visible solo para roles con permisos de visualización. Incluye filtros por estado y rol.
- **Actualizar Usuario:** Permite seleccionar un usuario y modificar detalles, con validaciones para evitar cambios en usuarios no autorizados.
- **Eliminar Usuario:** Opción disponible para eliminar usuarios, respetando las restricciones de permisos de rol.

### Módulo de Consumos y Productos

- **Crear Producto:** En la sección "Crear Producto", se pueden agregar nuevos productos al inventario, verificando que todos los datos esenciales estén completos.
- **Ver Productos:** La lista de productos disponibles muestra detalles como cantidad, costo y precio de venta. Se puede filtrar la lista por cantidad y nombre.
- **Editar Producto:** Permite actualizar detalles de los productos, y ajusta la cantidad automáticamente según el consumo registrado.
- **Eliminar Producto:** Solo es posible si el producto no ha sido consumido, para preservar el historial de inventario.
- **Registrar Consumo:** En la sección de "Consumos", se pueden registrar consumos de productos, ajustando automáticamente el inventario. También es posible editar y eliminar consumos, manteniendo el inventario actualizado.

### Módulo de Especialistas y Especialidades

- **Crear Especialista:** En la sección "Crear", se pueden añadir especialistas, asegurando la validación de edad mínima y completitud de campos.
- **Ver Especialistas y Especialidades:** Muestra la lista de especialistas y sus asignaciones de especialidades, filtrable por especialidad y estado.
- **Editar Especialista:** Permite actualizar detalles de un especialista, incluyendo su estado, horario y especialidad asignada.
- **Asignar Especialidades:** Permite asignar especialidades a especialistas, evitando asignaciones duplicadas y solapamientos de horarios.
- **Eliminar Especialista:** Un especialista solo puede eliminarse si no tiene citas asignadas a su especialidad.


## Créditos
Este proyecto fue desarrollado por Juan David Ramirez. Agradecimientos especiales a Isaac Cerda y David Guaman por sus contribuciones.

## Licencia
Este proyecto está licenciado bajo MIT, Este proyecto puede ser usado, modificado y distribuido siempre que se incluya esta notificación en su versión.
