﻿using EuroSpaceCenter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace EuroSpaceCenter.Controllers {
    public class AttractionsController : ApiController {

        /// <summary>
        /// Get all of the attractions in Euro Space Center
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(List<Attraction>))]
        public IHttpActionResult Attractions() {
            var items = item.GetAllDisposed().Where(i => i.attraction != null).Select(i => new Attraction() {
                id = i.id,
                title = i.title,
                image = i.image,
                alt = i.alt,
                min_height = i.attraction.min_height,
                max_height = i.attraction.max_height,
                rating = i.ratings.Any() ? i.ratings.Average(r => r.rating1) : double.NaN,
                ratings = i.ratings.Select(r => new {
                    users_id = r.users_id,
                    datetime = r.datetime,
                    rating = r.rating1,
                    message = r.message
                })
            });

            return Json(items);
        }
    }

    internal class Attraction {
        public Attraction() {
        }

        public string alt { get; set; }
        public int id { get; set; }
        public string image { get; set; }
        public int? max_height { get; set; }
        public int? min_height { get; set; }
        public double rating { get; set; }
        public System.Collections.Generic.IEnumerable<object> ratings { get; set; }
        public string title { get; set; }
    }
}
