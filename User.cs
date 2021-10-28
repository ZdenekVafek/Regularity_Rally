using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Regularity_Rally
{
    public enum UserRole : Int32
    {
        None = 0,
        [Description("TrackCommissar")]
        TrackCommissar = 1,
        [Description("Admin")]
        Admin = 2,
        [Description("Timekeeper")]
        Timekeeper = 4,
        [Description("Reporter")]
        Reporter = 8,
        [Description("Padock")]
        Padock = 16
    }

    public enum UserPermission : Int32
    {
        None = 0,
        [Description("View")]
        View = 1,
        [Description("Update")]
        Update = 2,
        [Description("All")]
        All = 4,
        [Description("System")]
        System = 8
    }

    public class User
    {
        UInt32 Database_ID = 0;                 // holder id
        string Login = string.Empty;
        string Password;
        string Name;
        string LastName;
        string Email;
        Int32 _Permission;
        Int32 _Role;
        bool Active;  // dissable acc

        public User()
        {
        }

        public User(string _login, string _pass, string _name, string _last_name, string _email, bool _active, Int32 _role, Int32 _permission = 0)
        {
            this.Login = _login;
            this.Password = _pass;
            this.Name = _name;
            this.LastName = _last_name;
            this.Email = _email;
            this.Active = _active;
            this._Role = _role;
            this._Permission = (_permission != 0) ? _permission : PermissionByRole((UserRole)_role);
            // load database id?
        }

        public string Nick { get { return Login; } }
        public UserPermission Permission { get { return (UserPermission)this._Permission; } }
        public UserRole Role {  get { return (UserRole)this._Role; } }

        private Int32 PermissionByRole(UserRole role)
        {
            Int32 ret = (Int32)UserPermission.View;
            switch(role)
            {
                // fill in gaps


                case UserRole.Admin:
                    ret = (Int32)UserPermission.System;
                    break;
            }
            return ret;
        }

        public Int32 ID
        {
            get { return (Int32)Database_ID; }
        }

        private bool RequestID()
        {
            if (this.Login == string.Empty)
                return false;

            MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["UserSelectID"], this.Login, this.Password));
            MySqlDataReader rdr = cmd.ExecuteReader();
            UInt32 ID = 0;

            while (rdr.Read())
            {
                ID = rdr.GetUInt32("ID_User");
                break;
            }
            rdr.Close();

            if (ID == 0)
                return false;

            this.Database_ID = ID;
            return true;
        }

        public bool IsActive
        {
            get { return this.Active; }
        }

        public bool HasRole(UserRole role)
        {
            return (((UserRole)this._Role) & role) == role;
        }

        public bool HasPermision(UserPermission permission)
        {
            return (((UserPermission)this._Permission) & permission) == permission;
        }

        public static User Construct(UInt32 ID, string login, string pass, string name, string lastname, string email, Int32 perm, Int32 role, bool active)
        {
            User user = new User();

            user.Database_ID = ID;
            user.Login = login;
            user.Password = pass;
            user.Name = name;
            user.LastName = lastname;
            user.Email = email;
            user._Permission = perm;
            user._Role = role;
            user.Active = active;

            return user;
        }

        public static List<User> GetUserCollection(string filter = "%"/*wildcard*/)
        {
            List<User> ret = new List<User>();
            MySqlCommand cmd = Database.Database.CreateCommand(string.Format( Database.Database.QueryStack["UserSelectList"], filter));
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                User user = 
                User.Construct(
                    rdr.GetUInt32("ID_User"),
                    rdr.GetString("Login"),
                    rdr.GetString("Password"),
                    rdr.GetString("Name"),
                    rdr.GetString("Lastname"),
                    rdr.GetString("E_mail"),
                    rdr.GetInt32("Permisson"),
                    rdr.GetInt32("Role"),
                    rdr.GetBoolean("Active")
                );
                ret.Add(user);
            }
            rdr.Close();

            return ret;
        }

        public static User LogIn(string _login, string _pass_confirm)
        {
            // protect injection
            MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["UserSelect"], MySqlHelper.EscapeString(_login)));
            MySqlDataReader rdr = cmd.ExecuteReader();

            UInt32 ID = 0;
            string _pass = "",
                   salt = "";
            bool active = false;

            while (rdr.Read())
            {
                ID = rdr.GetUInt32("ID_User");
                _pass = rdr.GetString("Password");
                salt = rdr.GetString("Salt");
                active = rdr.GetBoolean("Active");

                break; // interested for first match only, no alter
            }
            rdr.Close();

            if (active == false)
                return null;

            string salted_pass = Regularity_Rally.Password.CretePasswordHash(_pass_confirm, salt);

            if (salted_pass != _pass)
                return null;

            // load user data
            MySqlCommand data_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["UserSelectData"], _login, salted_pass));
            MySqlDataReader data_rdr = data_cmd.ExecuteReader();
            User user = new User();

            while (data_rdr.Read())
            {
                user.Database_ID = data_rdr.GetUInt32("ID_User");
                user.Login = data_rdr.GetString("Login");
                user.Password = data_rdr.GetString("Password");
                user.Active = data_rdr.GetBoolean("Active");
                user.Name = data_rdr.GetString("Name");
                user.LastName = data_rdr.GetString("Lastname");
                user.Email = data_rdr.GetString("E_mail");
                user._Role = (Int32)data_rdr.GetUInt32("Role");
                user._Permission = data_rdr.GetInt32("Permisson");
                break; // interested for first match only, no alter
            }
            data_rdr.Close();

            if (user.Database_ID == 0)
                return null;

            return user;
        }

        /**
         * @throw error
         * 
         * **/
        public static bool UpdateUser(User user, Dictionary<string, string> update)
        {
            if (user.Database_ID == 0)
                return false;

            foreach(KeyValuePair<string, string> row in update)
            {
                string querry =  string.Format("UPDATE `user` SET `{0}`='{1}' WHERE `ID_User`='{2}';", row.Key, row.Value, user.Database_ID);
                MySqlCommand cmd = Database.Database.CreateCommand(querry);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new System.InvalidOperationException(string.Format("Error updating user data, {0}", row.Key));
                }
            }

            return true;
        }

        /***
         * Create user object 
         * 
         * @return - null if creating user object failed
         **/
        public static User CreateUser(string _login, string _pass, string _name, string _last_name, string _email, bool _active, UserRole _role, Int32 _permission = 0)
        {
            User user = new User();

            // make sure user name is not already picked
            string selected_nick = string.Empty;
            MySqlCommand nick_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["UserSelectList"], MySqlHelper.EscapeString(_login)));
            MySqlDataReader nick_rdr = nick_cmd.ExecuteReader();
            while (nick_rdr.Read())
            {
                selected_nick = nick_rdr.GetString("Login");
            }
            nick_rdr.Close();

            if (selected_nick != string.Empty)
                return null;

            user.Login = _login;
            user.Password = _pass;
            user.Name = _name;
            user.LastName = _last_name;
            user.Email = _email;
            user.Active = true;
            user._Role = (Int32)_role;
            user._Permission = (_permission != 0) ? _permission : user.PermissionByRole((UserRole)_role);

            // create needed string
            string salt = Regularity_Rally.Password.CreateRS();
            string salted_pass = Regularity_Rally.Password.CretePasswordHash(_pass, salt);

            // return null if it fails
            MySqlCommand usr_cmd = Database.Database.CreateCommand(
                string.Format(
                    Database.Database.QueryStack["UserCreate"],
                    user.Login,
                    salted_pass,
                    salt,
                    user.Name,
                    user.LastName,
                    user.Email,
                    user._Permission,
                    user._Role
                    //user.Active
                ));

            // verify
            int row_ef = usr_cmd.ExecuteNonQuery();

            // select index for user if passed
            if (!(row_ef > 0) || !user.RequestID())
                return null;

            return row_ef > 0 ? user : null;
        }

        public Control.UserView ToUserView(bool direct = true)
        {
            return new Control.UserView(
                this.Database_ID,
                this.Login,
                this.Active,
                this.Name,
                this.LastName,
                this.Email,
                Utility.GetDescription<UserPermission>((UserPermission)this._Permission), // in case of language support replace swith somting dummy
                Utility.GetDescription<UserRole>((UserRole)this._Role),                  // in case of language support replace swith somting dummy
                (direct) ? this : null);
        }
    }
}
