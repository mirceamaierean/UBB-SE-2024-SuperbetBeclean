using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperbetBeclean.Model
{
    public class User
    {
        private int _userID;
        private string _userName;
        private int _userCurrentFont;
        private int _userCurrentTitle;
        private string _userCurrentIconPath;
        private int _userCurrentTable;
        private int _userChips;
        private int _userStack;
        private int _userStreak;
        private int _userHandsPlayed;
        private int _userLevel;
        private int _userStatus; /// Inactive, Waiting, Playing
        private int _userBet;
        private DateTime _userLastLogin;

        private Card[] _userCurrentHand;

        public User(int userID = 0, string userName = "", int userCurrentFont = 0, int userCurrentTitle = 0, string userCurrentIconPath = 0, int userCurrentTable = 0, int userChips = 0, int userStack = 0, int userStreak = 0, int userHandsPlayed = 0, int userLevel = 0, DateTime userLastLogin = default(DateTime))
        {
            _userID = userID;
            _userName = userName;
            _userCurrentFont = userCurrentFont;
            _userCurrentTitle = userCurrentTitle;
            _userCurrentIconPath = userCurrentIconPath;
            _userCurrentTable = userCurrentTable;
            _userChips = userChips;
            _userStack = userStack;
            _userStreak = userStreak;
            _userHandsPlayed = userHandsPlayed;
            _userLevel = userLevel;
            _userLastLogin = userLastLogin;
            _userStatus = 0;
            _userBet = 0;
            _userCurrentHand = new Card[2];
        }

        public int UserID { get { return _userID; } set { _userID = value; } }
        public string UserName { get { return _userName; } set { _userName = value; } }
        public int UserCurrentFont { get { return _userCurrentFont; } set { _userCurrentFont = value; } }
        public int UserCurrentTitle { get { return _userCurrentTitle; } set { _userCurrentTitle = value; } }
        public string UserCurrentIconPath { get { return _userCurrentIconPath; } set { _userCurrentIconPath = value; } }
        public int UserCurrentTable { get { return _userCurrentTable; } set { _userCurrentTable = value; } }
        public int UserChips { get { return _userChips; } set { _userChips = value; } }
        public int UserStack { get { return _userStack; } set { _userStack = value; } }
        public int UserStreak { get { return _userStreak; } set { _userStreak = value; } }
        public int UserHandsPlayed { get { return _userHandsPlayed; } set { _userHandsPlayed = value; } }
        public int UserLevel { get { return _userLevel; } set { _userLevel = value; } }
        public DateTime UserLastLogin { get { return _userLastLogin; } set { _userLastLogin = value; } }
        public int UserStatus { get { return _userStatus; } set { _userStatus = value; } }

        public Card[] UserCurrentHand { get { return _userCurrentHand; } set { _userCurrentHand = value; } }
    }
}
