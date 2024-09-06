using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet.BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.BookStore.Controllers
{
    public class LikeRatingController : Controller
    {
        private readonly ILikeRatingService _likeRatingService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public LikeRatingController(ILikeRatingService likeRatingService, IBookService bookService, IUserService userService)
        {
            _likeRatingService = likeRatingService;
            _bookService = bookService;
            _userService = userService;
        }
        public IActionResult Index()
        {

            return View();
        }
    }
}