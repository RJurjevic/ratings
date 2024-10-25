# ChessRatingCalculator

**A chess rating adjustment tool designed to calculate post-game ratings with stability control, ensuring accurate, stretch-free adjustments to player grades.** This program prevents unintended "grade stretching" by controlling the adjustment factor, allowing users to simulate stable chess rating calculations based on single or multiple games.

## Features

- Calculates adjusted ratings (`An` and `Bn`) based on initial ratings, game results, and player activity status.
- Uses a stability adjustment factor `K` to ensure ratings do not diverge uncontrollably.
- Provides feedback on rating calculations, ensuring no "grade stretching" occurs.
- Configurable to handle single games or cumulative results over multiple games.

## Installation

1. **Download and Unzip**:
   - Download the `ChessRatingCalculator.zip` file.
   - Unzip it to a desired folder, which will result in two files:
     - `ChessRatingCalculator.exe`
     - `ChessRatingCalculator.exe.config`

2. **Run the Program**:
   - Open **Command Prompt** or **Windows Terminal**.
   - Use the `cd` command to navigate to the folder where you unzipped the files.
     ```cmd
     cd path\to\ChessRatingCalculator
     ```

## Usage

### Command-Line Arguments

To use the program, input the following arguments in this order:

```cmd
ChessRatingCalculator.exe <A> <aA> <B> <aB> <q> [<n>]
```

- **`A`**: Initial rating of player A (range: 0 to 3000).
- **`aA`**: Activity status of player A (`true` if active, `false` otherwise).
- **`B`**: Initial rating of player B (range: 0 to 3000).
- **`aB`**: Activity status of player B (`true` if active, `false` otherwise).
- **`q`**: Game result or performance from player A’s perspective (`0` if player A loses, `50` if a draw, `100` if player A wins).
- **`n`** _(optional)_: Number of games played (defaults to 1).

### Example Command

For a single game where player A has a rating of `1530`, player B has a rating of `1450`, both are active, and the game is a draw:

```cmd
ChessRatingCalculator.exe 1530 true 1450 true 50 1
```

### Expected Output

The program will display the following information in the Command Prompt:

- **Adjusted Ratings**: `An` and `Bn` values after the rating adjustment.
- **Stability Checks**:
  - `K <= 1 check`: Ensures no grade stretching.
  - `Sum of rating changes check`: Confirms ratings balance out.
  - `Total grade preservation check`: Confirms total rating preservation.

## Explanation of Key Parameters

- **`q` (Game Result)**: Represents player A's result from the perspective of player A:
  - `0` = player A lost
  - `50` = draw
  - `100` = player A won
  - For multiple games, `q` represents the cumulative performance of player A across the match.

- **Adjustment Factor `K`**: Ensures gradual, stable rating adjustments, with a maximum of `1`. The value of `K` is determined by `n / 30`, allowing the program to accurately reflect cumulative game results without causing “grade stretching.”

## Integration for ECF Rating Team

The **ECF rating team** can incorporate `ChessRatingCalculator.exe` into their existing rating suite to automate grade adjustment calculations. This tool can be easily integrated into any Windows-based program or script that the team currently uses, allowing it to interact seamlessly with existing player data. 

### Integration Steps

1. **Calling `ChessRatingCalculator.exe` from Your Program**:
   - ECF can call `ChessRatingCalculator.exe` as a subprocess in Windows, passing in the necessary parameters for player ratings, activity status, game result, and game count.

2. **Parsing Output**:
   - Capture and parse the output to extract:
     - `Adjusted Rating An`
     - `Adjusted Rating Bn`
   - Additional checks, such as grade stretching prevention, can be used to ensure alignment with ECF’s historical grade calculations.

3. **Using with Existing Data**:
   - While `ChessRatingCalculator.exe` does not interact with a database, it can be called in conjunction with a database system. This allows ECF to automate adjustments by fetching initial ratings from the database, running calculations, and updating adjusted ratings directly in the database.
   - This setup mirrors the causation model used in historical ECF grades, ensuring the results are accurate, stable, and stretch-free.

## Acknowledgments

Created by the **M&R Research Team London**. Special thanks to the chess community and all contributors for their insights into rating adjustment methods.
