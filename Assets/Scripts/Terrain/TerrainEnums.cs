using System;

public enum TerrainType
{
	None,
	Grass,
	Lava,
	Forest,
	Rock,
	Empty
}

[Flags]
public enum Direction
{
	None = 0,
	Up = 1,
	Down = 2,
	Left = 4,
	Right = 8,
	AllDirections = Direction.Up | Direction.Down | Direction.Left | Direction.Right
}