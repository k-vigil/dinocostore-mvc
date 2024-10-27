### DinocoStore

DinocoStore es un ejemplo práctico de ASP.NET Core MVC. Consiste en una simple aplicación donde se pueden registrar productos y estos pueden ser agregados a una factura. En palabras sencillas es un pequeño ejemplo de maestro-detalle.

#### Ejecutar el proyecto

Para correr el proyecto, ejecuta el siguiente comando en la terminal

```
dotnet run --project src/DinocoStore.Web
```

#### Base de datos

Antes de correr las migraciones asegurate de editar correctamente los datos de conexión a la base de datos en el archivo ``` appsettings.json ```. Para correr las migraciones, ejecuta el siguiente comando en la terminal. Ten en cuenta que tienes que tener instalado globalmente la herramienta de ``` dotnet-ef ```. Por defecto, esta aplicación utliza ``` PostgreSQL ``` como motor de base de datos

```
dotnet ef migrations add <MIGRATION_NAME> --project src/DinocoStore.Web -o Data/Migrations
```

Luego actualiza la base de datos

```
dotnet ef database update --project src/DinocoStore.Web
```

#### Consideraciones

Ten en cuenta que este proyecto esta desarrollado con .NET 8. Si quieres añadirle más complejidas según sea tu necesidad, adelante, puedes descargarlo y no olvides dejar una estrellita 😉

#### Autor

Enrique Vigil
