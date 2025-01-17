﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libraryMeneger.user;
using System.Configuration;

namespace libraryMeneger.Data.UserRepository
{
    public abstract class IUserRepository
    {

        public IUserRepository() { }
        public abstract bool DoesSuchUserExist(string loginToCheck);
        public abstract bool IsGivenUserAdmin(string loginToCheck);
        public abstract bool IsPasswordCorrect(string loginToCheck, string Password);
        public abstract string getName(string loginToCheck);
        public abstract string getSurname(string loginToCheck);
        public abstract string getPhoneNumber(string loginToCheck);
        public abstract string getEmail(string loginToCheck);
        public abstract string getPassword(string loginToCheck);
        public abstract bool updateUser(RegularUser updatedOne);
        public abstract bool insertNewUser(RegularUser newOne);
}
}