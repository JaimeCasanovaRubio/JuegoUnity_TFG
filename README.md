# Primigenia 🌿

Videojuego 2D roguelite desarrollado como Trabajo de Fin de Grado del ciclo **DAM (Desarrollo de Aplicaciones Multiplataforma)**.

> **Autores:** Olga Marco Ugarte · Jaime Casanova Rubio

---

## Sobre el juego

**Primigenia** es un juego de exploración y combate con perspectiva semi-cenital ambientado en un bosque de fantasía. Cada partida es única gracias al sistema roguelite: si mueres, empiezas de cero, pero el conocimiento del mundo y ciertas mejoras persisten entre runs.

### Características principales

- Exploración de biomas con estética diferenciada
- Sistema de combate con habilidades activas y pasivas
- Progresión del personaje con afinidades y variantes de armadura
- Jefes con mecánicas propias
- Arte dibujado a mano con tableta gráfica (Krita + XP-Pen Artist 16)
- Configuración de teclas en tiempo de ejecución
- HUD con vida, habilidades y efectos de estado

---

## Tecnologías

| Herramienta | Uso |
|-------------|-----|
| Unity 2022.3 LTS | Motor principal |
| C# | Lenguaje de scripting |
| Visual Studio | IDE de desarrollo |
| Krita | Arte 2D y animaciones |
| GitHub | Control de versiones |

---

## Estructura del repositorio

```

JuegoUnity_TFG/   ← Proyecto Unity
    ├── Assets/
    │   ├── Scripts/
    │   │   ├── Core/       ← GameManager, InputHandler, SceneGestor...
    │   │   ├── Entities/   ← Player, Enemy, Boss (jerarquía OOP)
    │   │   ├── UI/         ← Menús, HUD, selección de personaje
    │   │   └── Items/      ← Sistema de objetos
    │   ├── Scenes/         ← Menú principal y escena jugable
    │   ├── Sprites/        ← Arte del juego
    │   ├── Maps/           ← Diseño de biomas
    │   ├── Animations/     ← Animaciones de personajes y enemigos
    │   └── Prefabs/        ← Objetos reutilizables
    └── ProjectSettings/
```

---

## Cómo ejecutar

1. Instala **Unity 2022.3 LTS** desde [Unity Hub](https://unity.com/download)
2. Clona el repositorio:
3. Abre Unity Hub → **Add project from disk** → selecciona la carpeta.
4. Abre la escena `Assets/Scenes/Menu` y pulsa **Play**

---

## Reparto de trabajo

| Área | Responsable |
|------|-------------|
| Arquitectura y sistemas de juego | Jaime Casanova |
| Arte, UI e integración visual | Olga Marco |


---

*TFG · DAM · Curso 2025-2026*
