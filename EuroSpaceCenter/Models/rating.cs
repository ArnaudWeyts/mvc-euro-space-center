﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;

namespace EuroSpaceCenter.Models {
    [MetadataType(typeof(RatingValidation))]
    public partial class rating {
        internal static void Rate(rating r) {
            using (var db = new DataClassesDataContext()) {
                db.ratings.InsertOnSubmit(r);
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Get ratings of a certain item
        /// </summary>
        /// <param name="id">items_id</param>
        /// <returns>list of all ratings</returns>
        internal static List<rating> Get(int id) {
            using (var db = new DataClassesDataContext()) {
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<rating>(t => t.user);
                db.LoadOptions = options;
                return db.ratings.Where(r => r.items_id == id).OrderByDescending(r => r.datetime).ToList();
            }
        }

        internal static List<rating> GetAll() {
            using (var db = new DataClassesDataContext()) {
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<rating>(t => t.user);
                options.LoadWith<rating>(t => t.item);
                db.LoadOptions = options;
                return db.ratings.ToList();
            }
        }

        internal static void Delete(int id) {
            using (var db = new DataClassesDataContext()) {
                var it = db.ratings.SingleOrDefault(i => i.id == id);
                db.ratings.DeleteOnSubmit(it);
                db.SubmitChanges();
            }
        }
    }

    public class RatingValidation {
        [Required(ErrorMessage = "You need to enter a rating")]
        [Range(1, 5, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        [Display(Name = "rating")]
        public int rating1;

        [Required(ErrorMessage = "A rating is done on a certain date")]
        [Display(Name = "date")]
        public DateTime datetime;

        [Required(ErrorMessage = "You need to explain why you rated")]
        public string message;

        [Required(ErrorMessage = "You need to be logged in to rate")]
        public int users_id;

        [Required(ErrorMessage = "You need to rate a particular item")]
        public int items_id;
    }
}