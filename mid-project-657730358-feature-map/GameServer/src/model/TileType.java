package model;

public enum TileType {
    NONE(0),
    WALL(1);

    private final int value;

    TileType(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }
}
