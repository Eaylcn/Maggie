# Maggie — 2D RPG Game (Unity 6, C#)

## EN — English Description

**Maggie** is a 2D RPG project developed in **Unity 6** using **C#** and the **New Input System**.  
Inspired by a tutorial series but expanded with original ideas, Maggie aims to build a focused RPG experience that showcases gameplay systems, combat, animation, and AI behavior.

This project is part of a portfolio and is documented with English comments for clarity.

---

### 🎮 Current Features
- Built with **Unity 6** and the **New Input System**
- Smooth and responsive movement system
- Idle, walk, and jump animations
- Air control for mid-air movement
- Wall Slide and Wall Jump
- Dash, Basic Attack, Combo Attack, Jump Attack, Wall Jump Attack
- Dynamic attack direction and flexible combat handling
- Enhanced wall detection and slide behavior
- Parallax and Endless Background systems
- Tilemap environment and Demo Level
- Cinemachine camera setup for smooth tracking
- Skeleton Enemy AI with full State Machine:
  - Idle  
  - Patrol  
  - Detection  
  - Chase  
  - Attack
- All assets are free and license-safe

---

## 📅 Development Progress

### ✅ Day 1 — Core Movement & Combat
The foundation of Maggie’s movement and combat was built: a fully functional player controller with fluid motion and early combat mechanics.

Highlights:
- Player movement and jump logic  
- State Machine structure (Grounded / OnAir)  
- Wall Slide & Wall Jump  
- Dash, Basic, Combo, Jump, and Wall Jump Attacks  
- Directional combo control and animation polish  
- Bug fixes for wall walking and state transitions  

> ✅ Day 1 Completed — Core systems stable and ready for world integration.

---

### ✅ Day 2 — World & Enemy AI
Focus shifted to environment design, camera systems, and enemy implementation.

Completed:
- Added Parallax and Endless Background  
- Built Tilemap and Demo Level  
- Integrated Cinemachine for dynamic camera behavior  
- Created Skeleton Enemy with full AI State Machine (Idle, Patrol, Detection, Chase, Attack)  

> ✅ Day 2 Completed — The world is alive with a functional enemy AI system.

---

### 🎯 Day 3 Goals — Combat & Interaction Systems
Next step: refining combat feedback and player–world interaction.

Planned:
- Expand Combat System (damage, hit detection, death handling)
- Implement Parry and Interface systems  
- Add Item Chest and Health Bar  
- Integrate on-hit VFX and damage indicators

> Focus will shift toward polishing gameplay feel and player feedback loops.

---

## 🔧 Latest Updates (Today)

### What was implemented today
- Target detection and a working damage system have been created.
- VFX for when characters take damage (hit reactions) have been implemented to improve feedback.
- Knockback effect added and applied for both player and enemies on hit.
- Player and enemy "dead" states implemented to handle death behavior and cleanup.
- A chest object was created; it can be opened by attacking it and plays an open animation.
  - Implementation detail: chests implement IDamageable — when attack logic calls IDamageable.TakeDamage on a chest, the chest responds by playing its open animation. This reuses the damage interface to trigger interactable behavior.
- There is currently no item/loot system, so chests only play the open animation; loot pickup will be added later.

---

## 🚧 Next Tasks (Planned next work)

### Short-term plans
- Add an enemy "stunned" state to handle temporary incapacitation on certain hits.
- Implement player counterattack/parry logic so skilled timing can negate or reverse damage.
- Add a visible health bar system for player and enemies.
- Create VFX for when an attack successfully hits (separate from damage-taken VFX), to reinforce impact.

---

### 👨‍💻 Developer
**Emir Ata Yalçın**  
> Game Developer | Software Engineer | Passionate About RPGs and Creative Coding
