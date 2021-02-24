using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.ViewModels
{
    public class PostLikesViewModel
    {
        public int Id { get; set; }
        public bool IsLike { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}