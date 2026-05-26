# AGENTS.md

## Project Direction

This Unity project is an SLG learning demo.

The main product loop is:

1. Base produces resources.
2. Player spends resources to upgrade buildings.
3. Buildings unlock or improve resources, troops, and technology.
4. Player trains troops and creates a march formation.
5. Player attacks a map stronghold.
6. Battle result gives rewards, losses, and progression.
7. Player returns to base and continues growth.

## Current Priority

Focus on Stage 8: SLG base and resource minimum loop.

Priority order:

1. Resource configs and resource inventory.
2. Building configs and building runtime state.
3. Resource production over time.
4. Building upgrade costs and effects.
5. Base UI showing resources, building levels, and upgrade buttons.

## Requirement Guardrails

Before implementing any feature, confirm it supports at least one SLG core system:

- Resource production, spending, capacity, or offline gain.
- Building construction, upgrade, unlocks, or effects.
- Technology research, prerequisites, or global bonuses.
- Troop training, inventory, formation, or power calculation.
- Map strongholds, march, battle result, rewards, or losses.
- Save data, versioning, or long-term progression.
- Configuration validation for buildings, resources, troops, tech, map, or battle formulas.

If a task does not support one of these, pause and explain why it may be off-track.

## What To Avoid For Now

Do not expand these as mainline features:

- Runner gameplay.
- Multiplier gates.
- Roguelike upgrade choices.
- Tower defense or horde-survival gameplay.
- Complex action combat, projectile variety, or boss skill depth.
- PVP, alliance, chat, gacha, shop, ads, or monetization.
- Large commercial world map simulation.
- Pure visual polish that does not support the SLG loop.

Existing runner/combat/editor systems are legacy validation modules. Keep them unless removal is explicitly requested, but do not extend them as the main direction.

## Implementation Rules

- Prefer data-driven design with ScriptableObject configs.
- Put new SLG code in focused modules: `Resource`, `Building`, `Technology`, `Troop`, `Battle`, `Map`, `Save`, `Base`, `UI`, and `Data`.
- Keep old runner/combat code stable unless adapting a small piece for the SLG loop.
- Update `Docs/DevelopmentPlan.md` and `Docs/TechnicalNotes.md` whenever a new system, design decision, or learning point is added.
- For each new feature, state which stage and which SLG loop step it serves before implementation.

## Current Next Step

Build the Stage 8 minimum loop:

- Resource inventory.
- Resource production.
- Building runtime state.
- Building upgrade.
- Base UI.
