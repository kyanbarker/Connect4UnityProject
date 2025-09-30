# Connect 4 Unity Game

A complete Connect 4 game built in Unity with mouse controls, win detection, and turn-based gameplay.

## Demo Video

Watch the gameplay demonstration: https://www.youtube.com/watch?v=Koau-1sKJoA&list=PLKz7QOwPaa35za3Q2WB6QacrwIFppwwSh&index=5

## Features

- Classic 7x6 Connect 4 board
- Two-player alternating turns (Red vs Yellow)
- Real-time win detection for horizontal, vertical, and diagonal connections
- Preview token shows where your piece will drop
- Game end detection and winner announcement
- Clean UI with TextMeshPro messaging

## How to Play

1. Open the project in Unity
2. Play the SampleScene
3. Hover over columns to see token preview
4. Click to drop your token
5. First player to connect 4 tokens wins

## Project Structure

**Scripts:**
- `Main.cs` - Core game logic, board management, and win detection
- `Token.cs` - Individual token visual properties and color management
- `MouseObserver.cs` - Mouse input handling for column selection

**Prefabs:**
- `Token.prefab` - Reusable token GameObject
- `MouseObserver.prefab` - Column interaction handler

## Technical Details

The game uses a 2D array to track token positions and implements efficient win detection by checking all possible 4-in-a-row patterns. The preview system uses a semi-transparent token that follows mouse movement. Game state is managed through a singleton pattern with the Main class.

## Setup Instructions

1. Clone or download the repository
2. Open in Unity (2020.3 LTS or newer recommended)
3. Open `Assets/Scenes/SampleScene.unity`
4. Press Play to start the game

## Key Methods

- `Drop(int x)` - Places token and checks for wins
- `IsConnect4` - Scans board for winning patterns
- `SwitchColor()` - Alternates between players

## License

Open source - feel free to use and modify.
