// https://www.chessprogramming.org/Direction
public enum Direction : sbyte {
    // Ray Directions
    NORTH = 8,
    WEST = -1,
    SOUTH = -8,
    EAST = 1,
    NORTH_EAST = 9,
    NORTH_WEST = 7,
    SOUTH_WEST = -9,
    SOUTH_EAST = -7,
    NONE = 0,
    
    // Knight Directions
    KNIGHT_NORTH_NORTH_WEST = 15,
    KNIGHT_NORTH_NORTH_EAST = 17,
    KNIGHT_NORTH_EAST_EAST = 10,
    KNIGHT_SOUTH_EAST_EAST = -6,
    KNIGHT_SOUTH_SOUTH_EAST = -15,
    KNIGHT_SOUTH_SOUTH_WEST = -17,
    KNIGHT_SOUTH_WEST_WEST = -10,
    KNIGHT_NORTH_WEST_WEST = 6
}

public static class DirectionExtension {

    public static sbyte GetValue(this Direction direction) {
        return (sbyte) direction;
    }
    
}