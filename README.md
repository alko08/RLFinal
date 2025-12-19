# Adversarial Tag, You're It
In this project, we train two agents to play tag in Unity using the Unity ML-Agents Toolkit and reinforcement learning algorithms, specifically PPO.
[Github](https://github.com/alko08/RLFinal/tree/main)

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

## Installation

There are many different guides out there to install Unity and the ML-Agents Toolkit. As instructed in the current guides we used Unity 6 and Python 3.10.12 for this project:
- [Installation guide from ML-Agents](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md)
- [Unity manual installation documentation](https://docs.unity3d.com/Packages/com.unity.ml-agents@4.0/manual/Installation.html)
- [Old Code Monkey Introduction to ML-Agents Video](https://youtu.be/zPFU30tbyKs?si=LsOX_CYjLRNPns28)

There are multiple scenes and scripts included in this Github. To reproduce the results from this paper run "TagScene2" scene, with the ML-Agents config "tagAgentScripted.yaml". Specifically you can just run the final neural network "TagAgentsScriptedFinal.onnx". Everything can be found in their respective folder in "Assets".
