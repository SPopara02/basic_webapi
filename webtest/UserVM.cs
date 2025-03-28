namespace webtest
{
    public class UserVM
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string username { get; set; }

        public UserVM(int id,string name,string surname, string username)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.username = username;
        }
    }
}
