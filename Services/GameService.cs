using SuperbetBeclean.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperbetBeclean.Services
{
    public class GameService
    {
        const int FULL = 8;
        private static List<User> internTableUsers = new List<User>();
        private static List<User> juniorTableUsers = new List<User>();
        private static List<User> seniorTableUsers = new List<User>();

        public GameService() { 
            
        }

        public void disconnectUser(User u)
        {
            internTableUsers.Remove(u);
            juniorTableUsers.Remove(u);
            seniorTableUsers.Remove(u);
        }
        public bool joinInternTable(User u)
        {
            if (isInternFull()) return false;
            internTableUsers.Add(u);
            return true;
        }

        public bool joinJuniorTable(User u)
        {
            if (isJuniorFull()) return false;
            juniorTableUsers.Add(u);
            return true;
        }

        public bool joinSeniorTable(User u)
        {
            if (isSeniorFull()) return false;
            seniorTableUsers.Add(u);
            return true;
        }

        public bool isInternFull()
        {
            return internTableUsers.Count == FULL;
        }

        public bool isJuniorFull()
        {
            return juniorTableUsers.Count == FULL;
        }

        public bool isSeniorFull()
        {
            return seniorTableUsers.Count == FULL;
        }

        public int occupiedIntern()
        {
            return internTableUsers.Count;
        }

        public int occupiedJunior()
        {
            return juniorTableUsers.Count;
        }

        public int occupiedSenior()
        {
            return seniorTableUsers.Count;
        }
    }
}
