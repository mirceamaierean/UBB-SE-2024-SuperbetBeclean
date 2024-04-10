using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperbetBeclean.Model
{
    public class Challenge
    {
        private int _challengeID;
        private string _challengeDescription;
        private string _challengeRule;
        private int _challengeAmount;
        private int _challengeReward;

        public Challenge(int challengeID = 0,  string challengeDescription = "", string challengeRule = "", int challengeAmount = 0, int challengeReward = 0)
        {
            _challengeID = challengeID;
            _challengeDescription = challengeDescription;
            _challengeRule = challengeRule;
            _challengeAmount = challengeAmount;
            _challengeReward = challengeReward;
        }

        public int ChallengeID { get { return _challengeID; } set { _challengeID = value; } }
        public string ChallengeDescription { get { return _challengeDescription; } set { _challengeDescription = value; } }
        public string ChallengeRule { get { return _challengeRule; } set { _challengeRule = value; } }
        public int ChallengeAmount {  get { return _challengeAmount; } set { _challengeAmount = value; } }
        public int ChallengeReward {  get { return _challengeReward; } set { _challengeReward = value; } }
    }
}
