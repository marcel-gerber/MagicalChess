public class Move {
    
    private Square from;
    private Square to;

    public Move(Square from, Square to) {
        this.from = from;
        this.to = to;
    }

    public Square GetFrom() {
        return from;
    }

    public Square GetTo() {
        return to;
    }
}