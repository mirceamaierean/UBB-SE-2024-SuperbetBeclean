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
        private int _userCurrentIcon;
        private int _userCurrentTable;
        private int _userChips;
        private int _userStack;
        private int _userStreak;
        private int _userHandsPlayed;
        private int _userLevel;
        private DateTime _userLastLogin;

        public User(int userID = 0, string userName = "", int userCurrentFont = 0, int userCurrentTitle = 0, int userCurrentIcon = 0, int userCurrentTable, int userChips, int userStack, int userStreak, int userHandsPlayed, int userLevel, DateTime userLastLogin)
        {
            _userID = userID;
            _userName = userName;
            _userCurrentFont = userCurrentFont;
            _userCurrentTitle = userCurrentTitle;
            _userCurrentIcon = userCurrentIcon;
            _userCurrentTable = userCurrentTable;
            _userChips = userChips;
            _userStack = userStack;
            _userStreak = userStreak;
            _userHandsPlayed = userHandsPlayed;
            _userLevel = userLevel;
            _userLastLogin = userLastLogin;
        }

        public int UserID { get { return _userID; } set { _userID = value; } }
        public string UserName { get { return _userName; } set { _userName = value; } }
        public int UserCurrentFont {  get { return _userCurrentFont; } set { _userCurrentFont = value; } }
        public int UserCurrentTitle {  get { return _userCurrentTitle; } set { _userCurrentTitle = value; } }
        public int UserCurrentIcon {  get { return _userCurrentIcon; } set { _userCurrentIcon = value; } }
        public int UserCurrentTable {  get { return _userCurrentTable; } set { _userCurrentTable = value; } }
        public int UserChips {  get { return _userChips; } set { _userChips = value; } }
        public int UserStack { get { return _userStack; } set { _userStack = value; } }
        public int UserStreak {  get { return _userStreak; } set { _userStreak = value; } }
        public int UserHandsPlayed { get {  return _userHandsPlayed; } set { _userHandsPlayed = value;} }
        public int UserLevel { get { return _userLevel; } set { _userLevel = value; } }
        public DateTime UserLastLogin {  get { return _userLastLogin; } set { _userLastLogin = value; } }
    }
}
