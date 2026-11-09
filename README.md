# Universidad del Valle
## Carrera de Ingeniería de Sistemas / Informática
### Asignatura: Programación Web III
### Proyecto: Segundo Momento Evaluativo - Portal de Microcursos y Tutoriales

---

### Integrantes del Grupo
* Juan Vesto - Usuario GitHub: Zudokatotk (juavesto@gmail.com)
* Sebastián - Usuario GitHub: darkaswht (sebastianolo748@gmail.com)

---

### 1. Justificación de la Arquitectura: ¿Por qué elegimos ASP.NET Core MVC en lugar de Razor Pages?

Para el desarrollo del proyecto evaluamos ambas alternativas propuestas por el docente (Razor Pages y MVC), y decidimos utilizar el patrón **ASP.NET Core MVC** por las siguientes razones:

1. **Separación de responsabilidades clara para el trabajo en equipo:**  
   Al ser un trabajo en pareja, la arquitectura MVC (Modelos, Vistas y Controladores) nos permitió dividir el trabajo de manera organizada. Mientras uno trabajaba en el modelado de datos y la persistencia en `AppDbContext`, el otro avanzaba en la creación de los controladores y la estructura de las vistas Razor con Bootstrap.

2. **Control estructurado de flujos mediante Controladores:**  
   En MVC, las solicitudes HTTP llegan primero a las acciones de los controladores (`CursosController`, `LeccionesController`, `InscripcionesController`), lo que permite validar los datos y coordinar el acceso al `AppDbContext` antes de renderizar la vista correspondiente.

3. **Facilidad de comprensión y defensa técnica:**  
   El patrón MVC (Modelo-Vista-Controlador) es el estándar ampliamente utilizado en el desarrollo web empresarial y el modelo conceptual que mejor dominamos para explicar el ciclo de vida de una petición ante el docente.

---

### 2. Estado de Implementación del Proyecto (Hasta Paso 5: Scaffolding)

Siguiendo el avance práctico de la materia (Pasos 1 al 5 de la planificación):
1. **Creación del proyecto ASP.NET Core MVC.**
2. **Definición de Modelos de datos:** `Curso`, `Leccion`, `Inscripcion` y `LeccionCompletada`.
3. **Configuración de Base de Datos:** `AppDbContext` heredando de `DbContext` con Entity Framework Core.
4. **Persistencia y Migraciones:** Mapeo de relaciones y claves foráneas en `OnModelCreating`.
5. **Scaffolding de Controladores y Vistas:** Generación completa de los módulos CRUD para la administración de cursos, lecciones e inscripciones.

*(Nota: Los pasos posteriores referentes al módulo de Identity y autenticación de usuarios quedaron pendientes de las pautas específicas del docente, priorizando tener la arquitectura relacional y el CRUD completamente operativos y funcionales).*

---

### 3. Modelo de Datos y Entidades

* **`Curso`:** Almacena el título, descripción, nivel de dificultad (Principiante, Intermedio, Avanzado), duración en minutos y estado de publicación.
* **`Leccion`:** Vinculada a un curso mediante `CursoId`, con título, orden numérico de presentación y contenido técnico.
* **`Inscripcion`:** Vincula a un estudiante (nombre y correo) con un curso específico, con restricción de unicidad para evitar duplicados.
* **`LeccionCompletada`:** Permite registrar las lecciones que un estudiante ha marcado como finalizadas.

---

### 4. Módulos y Funcionalidades del Sistema

* **Módulo de Cursos (`CursosController`):**
  * Catálogo de cursos con buscador por texto y filtro por nivel.
  * Creación, edición, consulta de detalles y eliminación de cursos.
  * Navegación directa hacia las lecciones de cada curso.
* **Módulo de Lecciones (`LeccionesController`):**
  * Listado de lecciones filtradas por curso.
  * Formulario de creación con selector de curso y orden numérico.
  * Visualización del contenido completo de la lección y edición.
* **Módulo de Inscripciones (`InscripcionesController`):**
  * Registro de inscripción de estudiantes a cursos publicados.
  * Validación de no duplicidad de inscripción por correo.
  * Anulación y cancelación de inscripciones.

---

### 5. Configuración de Base de Datos y Conexión

* **Motor:** Microsoft SQL Server / LocalDB.
* **Cadena de conexión:** Centralizada en `appsettings.json` bajo la clave `DefaultConnection`:
  `Server=(localdb)\mssqllocaldb;Database=PortalMicrocursos_UnivalleDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True`
* Al ejecutar la solución, el sistema asegura automáticamente la creación de la base de datos y de todas sus tablas relacionales.

---

### 6. Instrucciones para Ejecutar la Solución

1. Abrir la solución `Segundo_Momento_Evaluativo.slnx` en **Visual Studio**.
2. Verificar que el proyecto de inicio sea `Segundo_Momento_Evaluativo`.
3. Presionar **F5** (o botón de Depurar).
4. El sistema creará la base de datos y abrirá el navegador en el catálogo de cursos.
