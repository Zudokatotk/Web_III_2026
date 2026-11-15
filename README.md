# Universidad del Valle
## Programación Web III - Segundo Momento Evaluativo
### Proyecto: Portal de Microcursos y Tutoriales

**Integrantes:**
* Juan Vesto - `Zudokatotk` (juavesto@gmail.com)
* Sebastián - `darkaswht` (sebastianolo748@gmail.com)

---

### ¿Por qué elegimos ASP.NET Core MVC en lugar de Razor Pages?

Para el desarrollo del proyecto optamos por la arquitectura **ASP.NET Core MVC** por los siguientes motivos:

1. **Separación clara de responsabilidades para el trabajo en pareja:**  
   Al ser un proyecto colaborativo de dos integrantes, la separación entre Modelos (`Models`), Vistas (`Views`) y Controladores (`Controllers`) facilitó dividirnos las tareas de forma ordenada, evitando conflictos de código mientras uno configuraba las entidades y la base de datos, y el otro trabajaba en la lógica de negocio y las interfaces.

2. **Control centralizado de la lógica y la seguridad:**  
   A diferencia de Razor Pages (donde la lógica está fuertemente acoplada a cada página individual mediante PageModels), en MVC las solicitudes HTTP pasan primero por acciones centralizadas en los Controladores. Esto nos permitió gestionar de manera más limpia las consultas con Entity Framework Core, la validación de modelos (`ModelState.IsValid`) y la protección de rutas mediante roles con ASP.NET Core Identity (`[Authorize(Roles = "...")]`).

3. **Arquitectura estándar aprendida en clase:**  
   El patrón MVC es la base fundamental enseñada en la materia de Programación Web III. Trabajar con esta estructura nos brinda mayor dominio para explicar el ciclo de vida de cada petición durante la defensa ante el docente.
