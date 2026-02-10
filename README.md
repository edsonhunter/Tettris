Tetris: Pure C# Logic Engine
A technical demonstration of architectural decoupling in game development. This project implements the full classic Tetris logic using Pure C#, completely independent of the Unity3D engine.

The goal of this repository is to demonstrate how to build a robust, unit-testable game core that treats the game engine (Unity) merely as a "View" or "Plugin." This approach ensures that the game's fundamental rules, state management, and math are not tied to MonoBehaviors or engine-specific lifecycles.

#Architectural Approach

The Core: It contains no references to UnityEngine.

Grid Management: A 2D array system handling collision detection and line clearing.

Tick System: A custom-built timing mechanism that handles gravity and input processing intervals.

State Machine: Manages game flows (Menu, Playing, Paused, GameOver).

#View

Synchronization: Unity listens to events or polls the Core state to update the visual representation.

Input Bridge: Translates Unity’s Input or InputSystem calls into commands for the Core logic.

Visuals: Handles particles, animations, and sound effects based on triggers from the Core.

Zero-GameObject Logic: All calculations (rotation matrices, collision, scoring) happen in memory before a single frame is rendered.

Event-Driven Communication: The Core communicates with the View via C# Actions/Delegates, maintaining a clean one-way dependency.

Unit Testing: Because the logic is pure C#, I’ve implemented a suite of tests to verify line-clearing logic and Tetromino rotations without needing to open the Unity Editor.

High Performance: Minimal garbage collection by reusing data structures and avoiding heavy engine-side calls.

Command Pattern: For handling player inputs (Rotate, Move, Drop).
Observer Pattern: To decouple the UI and Audio systems from the game logic.
Strategy Pattern: (Optional/Planned) For different scoring algorithms or difficulty curves.
