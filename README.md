# Tic Tac Toe Multiplayer

A multiplayer Tic Tac Toe game developed in Unity using Photon PUN.

## Table of Contents
- [Overview](#overview)
- [Installation](#installation)
- [Usage](#usage)
- [Scripts](#scripts)
  - [Grid Section](#grid-section)
    - [GridCellBase](#gridcellbase)
    - [GridCell](#gridcell)
  - [Data Classes](#data-classes)
    - [DataHolder](#dataholder)
  - [Constants](#constants)
  - [Manager Classes](#manager-classes)
    - [AIManager](#aimanager)
    - [GameManager](#gamemanager)
    - [GridManager](#gridmanager)
    - [PhotonManager](#photonmanager)
    - [ScoreManager](#scoremanager)
    - [UIManager](#uimanager)
- [Programming Patterns](#programming-patterns)
  - [Component-Based Programming](#component-based-programming)
  - [Delegate Pattern](#delegate-pattern)
  - [Private Singleton Pattern](#private-singleton-pattern)
  - [Public Singleton Pattern](#public-singleton-pattern)
- [Game Modes and Grid Size](#game-modes-and-grid-size)
  - [Game Mode Selection](#game-mode-selection)
  - [Dynamic Grid Size](#dynamic-grid-size)
- [Contributing](#contributing)
- [Development Branch](#development-branch)

## Overview

This project is a multiplayer implementation of the classic Tic Tac Toe game using Unity (version 2021.3.5f1) and Photon PUN (version 2.0.0). The game is designed for online multiplayer functionality, allowing players to compete against each other.

## Installation

To run the project locally, follow these steps:

1. Clone the repository: `git clone https://github.com/your-username/tic-tac-toe-multiplayer.git`.
2. Open the project in Unity Hub using Unity version 2021.3.5f1.
3. Ensure Photon PUN 2.0.0 is imported into the project.
4. Open the "tic-tac-toe" scene.

## Usage

1. Launch the game in Unity Editor.
2. Build and deploy the game to your preferred platform.
3. Players can join a game room or create their own.
4. Enjoy a game of Tic Tac Toe with friends!

## Scripts

### Grid Section

#### `GridCellBase`

An abstract class providing a base structure for a grid cell. It contains common functionality shared by all grid cells.

#### `GridCell`

A script inheriting from `GridCellBase`, containing details specific to a particular grid cell in the Tic Tac Toe grid.

### Data Classes

#### `DataHolder`

A data holder class containing serialized data classes. These data classes serve as models for convenient use throughout the game.

### Constants

A constants class containing all the constants used in the game, including keys and Photon event codes.

### Manager Classes

#### `AIManager`

Responsible for handling all AI-related functionalities in the game.

#### `GameManager`

Manages all gameplay-related aspects and callbacks to other systems when events occur.

#### `GridManager`

Holds data for each grid cell and serves as the parent to all grids, acting as the main manager for the grid system.

#### `PhotonManager`

Handles all Photon callbacks, including connection, room joining, and parsing of data throughout the multiplayer services.

#### `ScoreManager`

Handles scoring throughout the game, tracking which player has which side and managing player sides when playing against AI.

#### `UIManager`

Manages the game's UI, handling tasks such as enabling panels and displaying text.

## Programming Patterns

### Component-Based Programming

Implemented in the `GridCell` script to ensure that each cell is only responsible for its own data.

### Delegate Pattern

Utilized for communication between `GridCell` instances and the `GameManager`, allowing actions on grid cells to inform the manager without exposing unnecessary details.

### Private Singleton Pattern

Applied in the `GameManager` to restrict direct access to variables, allowing controlled access through exposed functions and variables.

### Public Singleton Pattern

Used in the `UIManager` to make it easily accessible as a single instance for UI management.

## Game Modes and Grid Size

### Game Mode Selection

Players can select the game mode based on the button pressed - AI or multiplayer. The game logic adjusts accordingly.

### Dynamic Grid Size

The `TicTacToeGridManager` allows manipulation of the grid size by specifying the number of rows and columns, adapting the game accordingly.

## Contributing

Contributions to improve the game or fix issues are welcome! Follow these steps to contribute:

1. Fork the repository.
2. Create a new branch: `git checkout -b feature-name`.
3. Make your changes and commit them: `git commit -m 'Add some feature'`.
4. Push to the branch: `git push origin feature-name`.
5. Submit a pull request.

## Development Branch

The main development branch for this project is named `development`. Please base your contributions and pull requests on this branch.

