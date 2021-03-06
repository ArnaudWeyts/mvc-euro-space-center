﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace EuroSpaceCenter.Models {
    public partial class user {
        internal static bool Create(user u) {
            using (var db = new DataClassesDataContext()) {
                try {
                    u.password = util.Encryption.Hash.CreateHash(u.password);
                    db.users.InsertOnSubmit(u);
                    db.SubmitChanges();
                    var a = activation.create(u);
                    if (a == null) {
                        return false;
                    }
                    return util.Email.sendVerification(u, a);
                } catch (Exception e) {
                    throw e;
                }
            }
        }

        internal static void Update(user old, user newUser) {
            using (var db = new DataClassesDataContext()) {
                user usr = db.users.SingleOrDefault(us => us.email == old.email);
                usr.email = newUser.email;
                usr.name = newUser.name;
                db.SubmitChanges();
            }
        }

        internal static void SetPassword(string email, string password) {
            using (var db = new DataClassesDataContext()) {
                user u = db.users.SingleOrDefault(user => user.email == email);
                u.password = util.Encryption.Hash.CreateHash(password);
                db.SubmitChanges();
            }
        }

        internal static user Get(string email) {
            using (var db = new DataClassesDataContext()) {
                return db.users.SingleOrDefault(u => u.email == email);
            }
        }

        internal static user Get(int id) {
            using (var db = new DataClassesDataContext()) {
                return db.users.SingleOrDefault(u => u.id == id);
            }
        }

        internal static user GetDeferred(string email) {
            using (var db = new DataClassesDataContext()) {
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<user>(t => t.ratings);
                options.LoadWith<rating>(t => t.item);
                db.LoadOptions = options;
                return db.users.SingleOrDefault(u => u.email == email);
            }
        }

        internal static bool HasRating(string email, int rating_id) {
            using (var db = new DataClassesDataContext()) {
                return db.users.SingleOrDefault(u => u.email == email).ratings.Any(r => r.id == rating_id);
            }
        }

        internal static user Get(string email, string password) {
            using (var db = new DataClassesDataContext()) {
                user user = db.users.SingleOrDefault(u => u.email == email);
                if (user != null && util.Encryption.Hash.ValidatePassword(password, user.password)) {
                    return user;
                } else {
                    return null;
                }
            }
        }
    }
}