Proyecto de Videojuego 3D - Plantilla Unity
==========================================

Este proyecto está estructurado para cumplir con los requisitos solicitados:

1. Interfaz de inicio:
   - Escena `MainMenu` (debes crearla en Unity) con el script `MainMenuController`.
   - Botones para seleccionar dificultad (Fácil, Normal, Difícil) y salir del juego.

2. Sistema de navegación funcional:
   - El `GameManager` controla la navegación entre escenas (`MainMenu`, `Level1`, `Level2`, `Level3`).
   - Métodos: `LoadMainMenu`, `NextLevel`, `RestartLevel`.

3. Entorno 3D con iluminación y materiales:
   - Debes configurarlos en el editor (iluminación, materiales PBR, postprocesamiento).
   - Los scripts no limitan el uso en 3D; el `PlayerController` y la `CameraFollow` están preparados para entorno 3D.

4. Mecánica principal de juego:
   - Recoger objetos (`Collectible`) para ganar puntos mientras se evitan enemigos (`EnemyAI` + `EnemyDamage`).

5. Tres niveles de dificultad:
   - Enum `Difficulty` con: `Easy`, `Normal`, `Hard`.
   - Se usa en `GameManager`, `EnemyDamage`, `EnemyAI` y `LevelManager` para ajustar daño, velocidad y tiempos de aparición.

6. Seguimiento y registro del avance:
   - `GameManager` guarda: nivel actual, puntuación, vidas.
   - `SaveSystem` guarda progreso y mejor puntuación con `PlayerPrefs` (compatible con móvil).

7. Efectos para enriquecer la experiencia:
   - Script `Collectible` soporta `ParticleSystem` y `AudioSource`.
   - Debes asignar los efectos en el editor (sonidos, partículas, postprocesos).

8. Dinámica de juego 3D, curva de dificultad progresiva y guardado:
   - Movimiento en 3D con `CharacterController` (`PlayerController`).
   - `LevelManager` incrementa dificultad al subir de nivel.
   - Guardado de datos con `SaveSystem`.

9. Exportable a dispositivo móvil:
   - El uso de `PlayerPrefs`, `CharacterController` y UI estándar de Unity es compatible con Android/iOS.
   - Solo debes configurar el Build en Unity para tu plataforma objetivo.

10. Visita y retroalimentación:
   - No depende de código; puedes agregar textos de créditos o un panel en el menú donde registres comentarios.

11. Información clara y ortografía:
   - Todos los textos de ejemplo en UI pueden configurarse en español y revisarse en el editor.

12. Herramientas oficiales y versiones recientes:
   - El proyecto asume uso de Unity en versión actual, descargado desde el sitio oficial.

Cómo usar
---------

1. Importa esta carpeta como proyecto de Unity o copia el contenido de `Assets` y `ProjectSettings` a tu proyecto.
2. Crea las escenas:
   - `MainMenu`
   - `Level1`
   - `Level2`
   - `Level3`
3. En `MainMenu`:
   - Crea un Canvas con botones que llamen a:
     - `MainMenuController.PlayEasy()`
     - `MainMenuController.PlayNormal()`
     - `MainMenuController.PlayHard()`
     - `MainMenuController.QuitGame()`
4. En cada nivel:
   - Coloca al jugador (con `CharacterController` + `PlayerController`).
   - Coloca la cámara y asigna el script `CameraFollow`.
   - Añade el objeto con `GameManager` (marcarlo como persistent en la primera escena).
   - Crea un Canvas con el script `UIManager` y asigna textos y paneles.
   - Añade un objeto con `LevelManager` y configura `spawnPoints`, `enemyPrefab` y `collectiblePrefab`.

Recuerda siempre probar el juego en modo Play y, al final, generar tu build para móvil desde:
File -> Build Settings -> Android o iOS.
