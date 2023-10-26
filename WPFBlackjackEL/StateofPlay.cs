namespace WPFBlackjackEL
{
    //game states that can be useed to control which buttons are enabled
    public enum StateofPlay
    {
        NewGame, //everything cleared, new hand available
        NewHand, //deal available
        AfterDeal, //hit, stand, surrender available
        AfterHit, //surrender disabled
        PlayerStanding, //hit, stand disabled
        WinnerDeclared //new hand enabled
    }
}
