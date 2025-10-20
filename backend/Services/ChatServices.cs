namespace ChatApi.Services
{
    public class ChatServices
    {
        private static readonly Dictionary<string, string> Users = new Dictionary<string, string>();
        public bool AddUserToList(string addedUser)
        {
            lock (Users)
            {
                foreach(var user in Users)
                {
                    if (user.Key.ToLower() == addedUser.ToLower())
                    {
                        return false;
                    }

                }
                Users.Add(addedUser, null);
                return true;

            }
        }
        public void AddUserConnectionId(string user,string connectionId)
        {
            lock (Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users[user] = connectionId;
                }
            }
        }
        public string GetUserByConnectionId(string connId)
        {
            lock (Users)
            {
                return Users.Where(x => x.Value == connId).Select(x => x.Key).FirstOrDefault();
            }

        }
        public string GetUserByKey(string user)
        {
            lock (Users)
            {
                return Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }

        }

        public void RemoveUser(string user)
        {
            lock (Users)
            {
                if(Users.ContainsKey(user))
                Users.Remove(user);
            }

        }
        public string[] GetOnLineUsers()
        {
            lock (Users)
            {
                return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }


    }
}
