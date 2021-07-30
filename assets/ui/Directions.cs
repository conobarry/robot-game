using System;

public enum OrdinalDirection: int
{
    None = 0,
    North = 1,
    East = 3,
    South = -1,
    West = -3,
    NorthEast = 4,
    NorthWest = -2,
    SouthEast = 2,
    SouthWest = -4,
}

public enum CardinalDirection: int
{
    None = 0,
    North = 1,
    East = 3,
    South = -1,
    West = -3
}

static class DirectionMethods
{
    public static CardinalDirection ToCardinal(this OrdinalDirection ordinal)
    {
        if (Enum.IsDefined(typeof(CardinalDirection), (int) ordinal))
        {
            return (CardinalDirection) ordinal;
        }
        else
        {
            return CardinalDirection.None;
        }        
    }

    public static OrdinalDirection ToOrdinal(this CardinalDirection cardinal)
    {
        return (OrdinalDirection) cardinal;
    }

    public static CardinalDirection[] GetCardinalComponents(this OrdinalDirection ordinal)
    {
        switch (ordinal)
        {
            case OrdinalDirection.NorthEast:
                return new CardinalDirection[2] { CardinalDirection.North, CardinalDirection.East };
            case OrdinalDirection.SouthEast:
                return new CardinalDirection[2] { CardinalDirection.South, CardinalDirection.East };
            case OrdinalDirection.SouthWest:
                return new CardinalDirection[2] { CardinalDirection.South, CardinalDirection.West };
            case OrdinalDirection.NorthWest:
                return new CardinalDirection[2] { CardinalDirection.North, CardinalDirection.West };
            default:
                return new CardinalDirection[1] { ordinal.ToCardinal() };
        }
    }
}