﻿using EuroSpaceCenter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System;
using System.Web.Http.Description;

namespace EuroSpaceCenter.Controllers {
    public class ItemsController : ApiController {

        /// <summary>
        /// Get all of the items in Euro Space Center
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Item>))]
        public IHttpActionResult Items() {
            var items = item.GetAllDisposed().Select(i => new Item() {
                id = i.id,
                title = i.title,
                image = i.image,
                alt = i.alt,
                payment_types = i.restaurant != null ? i.restaurant.payment_type.Replace(", ", ";").Split(';') : null,
                datetime = i.show != null ? i.show.datetime : null,
                min_height = i.attraction != null ? i.attraction.max_height : null,
                max_height = i.attraction != null ? i.attraction.min_height : null,
                rating = i.ratings.Any() ? (double?)i.ratings.Average(r => r.rating1) : null,
                ratings = i.ratings.Select(r => new RatingEntity() {
                    users_id = r.users_id,
                    datetime = r.datetime,
                    rating = r.rating1,
                    message = r.message
                }),
                url = Url.Content($"~/Detail?id={i.id}"),
                description = i.description
            });

            return Json(items);
        }

        /// <summary>
        /// Get details about a single item
        /// </summary>
        /// <param name="id">the id of the item</param>
        [HttpGet]
        [ResponseType(typeof(Item))]
        public IHttpActionResult Single(int id) {
            var i = item.Get(id);
            if (i == null) {
                return NotFound();
            }

            var result = new Item() {
                id = i.id,
                title = i.title,
                image = i.image,
                alt = i.alt,
                payment_types = i.restaurant != null ? i.restaurant.payment_type.Replace(", ", ";").Split(';') : null,
                datetime = i.show != null ? i.show.datetime : null,
                min_height = i.attraction != null ? i.attraction.max_height : null,
                max_height = i.attraction != null ? i.attraction.min_height : null,
                rating = i.ratings.Any() ? (double?)i.ratings.Average(r => r.rating1) : null,
                ratings = i.ratings.Select(r => new RatingEntity() {
                    users_id = r.users_id,
                    datetime = r.datetime,
                    rating = r.rating1,
                    message = r.message
                }),
                url = Url.Content($"~/Detail?id={i.id}"),
                description = i.description
            };

            return Json(result);
        }
    }

    internal class Item {

        public Item() {
        }

        /// <summary>
        /// Alt attribute for the image
        /// </summary>
        public string alt { get; set; }

        /// <summary>
        /// Moment of this item (for shows)
        /// </summary>
        public DateTime? datetime { get; set; }

        /// <summary>
        /// ID of this item
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// URL to an image for this item
        /// </summary>
        public string image { get; set; }

        /// <summary>
        /// Maximum allowed height (attraction)
        /// </summary>
        public int? max_height { get; set; }

        /// <summary>
        /// Minimum allowed height (attraction)
        /// </summary>
        public int? min_height { get; set; }

        /// <summary>
        /// Array of strings, methods to pay (restaurant)
        /// </summary>
        public string[] payment_types { get; set; }

        /// <summary>
        /// Average rating (out of five)
        /// </summary>
        public double? rating { get; set; }

        /// <summary>
        /// All the ratings given
        /// </summary>
        public IEnumerable<RatingEntity> ratings { get; set; }

        /// <summary>
        /// The identifiable title
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// URL to a detail page for this item
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Plain text description for this item
        /// </summary>
        public string description { get; set; }
    }
}
