# Maggie — 2D RPG Game (Unity 6, C#)

## EN — English Description

**Maggie** is a 2D RPG project developed in **Unity 6** using **C#** and the **New Input System**.  
It began as a tutorial-inspired base and evolved into a **custom RPG framework** focused on **fluid movement**, **responsive combat**, and **dynamic AI**.

This project is a portfolio piece demonstrating gameplay programming, combat design, and system architecture.  
Scripts are documented with **English & Turkish comments** for clarity.

---

### 🎮 Current Features

**Core & Movement**
- Smooth 2D controller with **idle / walk / jump / air control**
- **Wall Slide**, **Wall Jump**, **Dash**
- Cinemachine camera, Parallax & Endless backgrounds, Tilemap, Demo level

**Player Combat**
- **Basic**, **Combo**, **Jump**, **Wall Jump** attacks
- **Counter / Parry**, **Dynamic attack direction**, **On-Hit VFX**, **Knockback**
- Unified interaction via `IDamageable` (e.g., opening **Chests** by attacking)

**Enemy AI**
- **Skeleton** with State Machine: **Idle**, **Patrol**, **Detection**, **Chase**, **Attack**, **Stunned**
- Target detection, damage & death handling

**Stats & UI**
- **Health Bar** (player & enemies)
- **Stat System foundations** (including **Evasion**)
- **Regen** (health) and **Attack Speed**
- **Training Dummy** for testing

**Damage Model**
- **Physical Damage** with **Critical Hits**
- **Armor Mitigation** & **Armor Penetration**
- **Elemental Damage Systems**:
  - **Fire** → Damage-over-Time (burn)
  - **Ice** → Global **slow** effect (movement & animations)
  - **Lightning** → **Charge** buildup; at max charge next hit triggers a **Lightning Strike** and **1s stun**

- All assets are **free and license-safe**

---

## 📅 Development Progress

### ✅ Day 1 — Core Movement & Combat  
Built the player controller, early combat, and Grounded/OnAir state architecture.

### ✅ Day 2 — World & Enemy AI  
World setup (Tilemap, Parallax/Endless, Cinemachine, Demo level) and **Skeleton AI**.

### ✅ Day 3 — Combat System Expansion  
Damage system, VFX, knockback, dead states, chest via `IDamageable`, **Stunned**, **Parry**, **Health Bar**, **Stat base** (+Evasion).

### ✅ Day 4 — Advanced Stats & Damage Model  
Focused on stats-driven combat and elemental systems.

**Implemented:**
- **Evasion** system improvements  
- **Physical Damage** + **Critical Attack** logic  
- **Armor Mitigation** & **Penetration**  
- **Elemental systems**: **Fire (DoT)**, **Ice (slow – affects movement & animations)**, **Lightning (charge → strike + 1s stun)**  
- **Health regeneration** and **Attack Speed**  
- **Training Dummy** for reliable testing

> ✅ Day 4 Completed — Combat is stats-aware and elementally reactive.

---

## 🎯 Day 5 Goals — Progression & Buff Systems
- Interactable **Buff Game Objects**
- **Stat Modifiers** (temporary/permanent)
- **Default Stat Setup** pass
- **Skill Tree** system & **UI**

---

### 👨‍💻 Developer
**Emir Ata Yalçın**  
> Game Developer | Software Engineer | Passionate About RPGs and Creative Coding
