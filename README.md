# RLFinal
In this project, we train two agents to play tag in Unity using the Unity ML-Agents Toolkit

## Adversarial Tag: Emergent Strategies via Staged Curriculum Learning

This project implements a competitive Pursuit-Evasion Game (PEG) in a physics-based 3D environment using the Unity ML-Agents toolkit. We utilize Proximal Policy Optimization (PPO) to train antagonistic agents that must co-adapt in a non-stationary environment.

## The Core Challenge: The "Cold Start"

In high-dimensional 3D spaces, randomized agents rarely achieve the sparse +1000 reward signal required for a "tag". This creates a "cold start" problem where learning never begins. We solved this by pivoting from pure self-play to a Staged Curriculum Learning framework.

## Staged Curriculum

The agents were trained through 6 distinct iterations to bootstrap locomotion and strategy:

1. Passive Baseline: Opponent at 1/6 speed; stationary when not "it".
2. Basic Evasion: Opponent at 2/6 speed with escape logic.
3. Physical Calibration: Fixed a 1/5 speed-cap bug and upped opponent to 4/6 speed.
4. Full Interaction: Parity speed and "tag-backs" to prevent behavioral clumping.
5. Strategic Anticipation: Scripted AI path prediction forced the agent to move beyond circular flight.
6. Heuristic Hardening: Resolved wall-stuck bugs, forcing the agent to find robust tactical solutions.

## Key Results

1. Emergent Strategy: The agent discovered a "Corner Trap"—intentionally luring the opponent into confined geometry to secure a tag and maximize the subsequent escape reward.
2. Skill Ceiling: Final ELO ratings reached ~1075, demonstrating mastery over the hardened scripted heuristic.
3. Actionable Intelligence: Decision frequency was set to once every 5 frames to allow for smooth, physics-consistent movements. 

## Technical Stack

- Engine: Unity 3D
- Toolkit: ML-Agents (C# SDK / Python API)
- Algorithm: PPO (Actor-Critic)
- State Space: Continuous (Relative positions and local environment data)