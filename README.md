# Maggie — 2D RPG Game (Unity 6, C#)

## EN — English Description

**Maggie** is a 2D RPG project developed in **Unity 6** using **C#** and the **New Input System**.  
The project started as a tutorial-inspired foundation but evolved into a **fully custom RPG framework** that focuses on **fluid movement**, **responsive combat**, and **dynamic AI behavior**.

This project serves as a portfolio piece to demonstrate gameplay programming, combat design, and system architecture.  
All scripts include **English and Turkish comments** for clarity and educational value.

---

### 🎮 Current Features
- Built with **Unity 6** and the **New Input System**
- **Fluid 2D movement** and physics-based control  
- **Idle**, **walk**, **jump**, and **air control** mechanics  
- **Wall Slide**, **Wall Jump**, and **Dash**  
- Full **Combat System**:
  - **Basic Attack**  
  - **Combo Attack Chain**  
  - **Jump Attack** & **Wall Jump Attack**  
  - **Counterattack / Parry** system  
  - **On-Hit VFX** and knockback feedback  
  - **Damage & Death states** for both player and enemies  
- **Enemy AI (Skeleton)** with full State Machine:
  - Idle  
  - Patrol  
  - Detection  
  - Chase  
  - Attack  
  - Stunned  
- **Health Bar** for player and enemies  
- **Stat System foundations** including evasion and core attributes  
- **Chest Interaction System** (triggered via `IDamageable` interface)  
- **Parallax & Endless Backgrounds**, **Tilemap**, **Cinemachine** camera  
- **Demo Level** implemented  
- All assets are **free and license-safe**

---

## 📅 Development Progress

### ✅ Day 1 — Core Movement & Combat  
Established the player controller, movement, and initial combat systems.  
Built the foundation of the **State Machine** and responsive input architecture.

### ✅ Day 2 — Environment & Enemy AI  
Created the visual world and added a functional **Skeleton Enemy AI** using a modular state machine.

### ✅ Day 3 — Combat System Expansion  
Focused on combat depth, feedback, and player–enemy interaction systems.

**Implemented:**
- **Target detection** and full **damage system**  
- **VFX** for taking and dealing damage  
- **Knockback** for player and enemies  
- **Dead states** for both player and AI  
- **Chest interaction** via `IDamageable` interface  
- **Enemy stunned state** and **player counterattack / parry logic**  
- **Health bar** UI for both entities  
- **Stat System base** created and **evasion** logic added  

> ✅ **Day 3 Completed** — Combat system and AI are now fully functional and connected to player stats.

---

## 🎯 Day 4 Goals — Stat System & Progression  
Next, the focus will be on expanding the stat system and connecting it with gameplay.

**Planned:**
- Complete the **Stat System** (strength, agility, vitality, intelligence, etc.)  
- Make stats dynamically affect combat (damage, evasion, critical damage etc.)  

---

### 👨‍💻 Developer
**Emir Ata Yalçın**  
> Game Developer | Software Engineer | Passionate About RPGs and Creative Coding
