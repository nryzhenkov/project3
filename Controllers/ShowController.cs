using System;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    public class ShowController : Controller
    {
        
        private ArticleContext db;
        public ShowController(ArticleContext context)
        {
            db = context;
        }
        public IActionResult Index(int page=1)
        {
            int pageSize = 5;
            IQueryable<Article> source = db.Articles;//= db.Articles.Include(x =>x);
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Articles = items
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AddArticle(Article article)
        {
            if (article.Text == null)
                article.Text = "";
            if (article.Theme == null)
                article.Theme = "";
            article.Date = DateTime.Now;
            db.Articles.Add(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Search(string text, string key = "false")
        {
            if (text == null)
            {
               return View();
            }
            Regex Etalon = new Regex(@".*" + text + @".*", RegexOptions.IgnoreCase);
            List<Article> save = new List<Article>();
            foreach(var Art in db.Articles)
            {
                if (string.Compare(key, "true") == 0)
                {
                    if (Etalon.IsMatch(Art.Theme))
                    {
                        save.Add(Art);
                    }
                }
                else
                {
                    if (Etalon.IsMatch(Art.Text))
                    {
                        save.Add(Art);
                    }
                }
            }
           // if (result != 0 && result1 != 0)
            //    return Json("No exist");
          //  else
              return View(save);
        }
    }
}
