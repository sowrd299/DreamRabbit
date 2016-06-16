using System;

class GameDataException : Exception {
    public GameDataException() :
        base() {}
    public GameDataException(string s):
        base(s) {}
}

class CardDataException : GameDataException {
    public CardDataException() :
        base () {}
    public CardDataException(string s) :
        base(s) {}
}

class SearchException : Exception {
    public SearchException() :
        base() { }
    public SearchException(string s):
        base(s) { }
}
