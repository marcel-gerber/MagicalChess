public class Parser {
    private static Parser _instance;

    private Parser() {
        
    }

    public static Parser Instance() {
        if (_instance == null) {
            return new Parser();
        }
        return _instance;
    }
}