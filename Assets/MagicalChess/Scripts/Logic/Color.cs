namespace Chess {
    
    /// <summary>
    /// Repräsentiert die beiden Farben Schwarz und Weiß im Schachspiel. 
    /// </summary>
    public enum Color : byte {
        WHITE,
        BLACK,
        NONE
    }

    public static class ColorExtension {

        public static Color GetOpposite(this Color color) {
            switch (color) {
                case Color.WHITE:
                    return Color.BLACK;
                case Color.BLACK:
                    return Color.WHITE;
                default:
                    return Color.NONE;
            }
        }
    
    }
}