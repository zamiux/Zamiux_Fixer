using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZamiuxFixer.DataLayer.Context;
using ZamiuxFixer.Domain.Questions;

namespace ZamiuxFixer.WEB.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ZamiuxFixerDbContext _context;

        #region Ctor
        public QuestionController(ZamiuxFixerDbContext context)
        {
            _context = context;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        #region Create Question
        [Route("Create-Question")]
        [Authorize]
        [HttpGet]
        public IActionResult CreateQuestion()
        {
            return View();
        }

        [Route("Create-Question")]
        [Authorize]
        [HttpPost]
        public IActionResult CreateQuestion(Question question, string Tags)
        {
            // aval info oon user ke mikhad follow kone ro peida kon
            int UserId = int.Parse(User.FindFirstValue("UserId"));
            var user_data = _context.Users.Find(UserId);

            question.UserId = UserId;

            if (ModelState.IsValid)
            {
                question.IsDelete = false;
                question.CreateDate = DateTime.Now;

                _context.Questions.Add(question);
                _context.SaveChanges();

                //tags manage
                if (!string.IsNullOrEmpty(Tags))
                {
                    string[] tags = Tags.Split('-', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var tag in tags)
                    {
                        _context.QuestionTags.Add(new QuestionTag()
                        {
                            QuestionId = question.QuestionId,
                            Tag = tag.Trim()
                        });
                    }

                    _context.SaveChanges();

                    return Redirect("/Show-Question/" + question.QuestionId);
                }
            }

            return View(question);
        }
        #endregion

        #region Show Question
        [HttpGet]
        [Route("Show-Question/{id}")]

        public IActionResult ShowQuestion(int id)
        {
            var question = _context.Questions
                .Include(q => q.User)
                .Include(q => q.QuestionTags)
                .Include(q => q.QuestionVotes)
                .Include(q => q.QuestionFavorites)
                .FirstOrDefault(q => q.QuestionId == id);

            if (question == null)
            {
                return NotFound(); 
            }

            //afzaye tedad bazdid
            question.QuestionVisit += 1;

            _context.Questions.Add(question);
            // _context.SaveChanges();

            return View(question);
        }
        #endregion

        #region Edit-Question
        [Authorize]
        [HttpGet]
        [Route("Edit-Question/{id}")]
        public IActionResult EditQuestion(int id)
        {
            int UserId = int.Parse(User.FindFirstValue("UserId"));

            var question = _context.Questions
                .Include(q => q.QuestionTags)
                .FirstOrDefault(q => q.QuestionId == id);

            if (question == null || question?.UserId != UserId)
            {
                return NotFound();
            }

            return View(question);
        }

        [Authorize]
        [HttpPost]
        [Route("Edit-Question/{id}")]
        public IActionResult EditQuestion(Question questionModel,string Tags)
        {
            int UserId = int.Parse(User.FindFirstValue("UserId"));

            var editquestion = _context.Questions.Find(questionModel.QuestionId);

            if (editquestion.UserId != UserId)
            {
                return BadRequest();
            }

            editquestion.Description = questionModel.Description;
            editquestion.Title = questionModel.Title;
            editquestion.ModifiedDate = DateTime.Now;

            _context.Questions.Update(editquestion);

            var tags = _context.Questions.Where(t=>t.QuestionId == questionModel.QuestionId).ToList();   

            // remove old tags
            foreach (var tag in tags)
            {
                _context.Questions.Remove(tag);
            }

            _context.SaveChanges();

            //add new tags
            if (!string.IsNullOrEmpty(Tags))
            {
                string[] new_tags = Tags.Split('-', StringSplitOptions.RemoveEmptyEntries);

                foreach (var new_tag in new_tags)
                {
                    _context.QuestionTags.Add(new QuestionTag()
                    {
                        QuestionId = questionModel.QuestionId,
                        Tag = new_tag.Trim()
                    });
                }

                _context.SaveChanges();

                return Redirect("/Show-Question/" + questionModel.QuestionId);
            }

            return View(editquestion);
        }
        #endregion

        #region short link
        // https://localhost:7235/q/@Model.QuestionId

        [Route("q/{id}")]
        public IActionResult Shortlink(int id)
        {
            return RedirectPermanent("/Show-Question/" + id);
        }
        #endregion

        #region Vote
        //url: /Question/AddToFav/1
        [Authorize]
        public int AddToFav(int id)
        {
            //current user
            int UserId = int.Parse(User.FindFirstValue("UserId"));

            var fav = _context.QuestionFavorites
                .FirstOrDefault(f => f.UserId == UserId  && f.QuestionId == id);

            if(fav == null)
            {
                _context.QuestionFavorites.Add(new QuestionFavorite() { 
                    QuestionId = id,
                    UserId = UserId
                }); 
            }
            else
            {
                _context.Remove(fav);
            }
            _context.SaveChanges();

            return _context.QuestionFavorites.Count(f=>f.QuestionId == id);
        }
        #endregion

        #region Add Vote

        [Authorize]
        public int AddVote(int id,string vote)
        {
            //current user
            int UserId = int.Parse(User.FindFirstValue("UserId"));

            var vote_data = _context.QuestionVotes
                .FirstOrDefault(f => f.UserId == UserId && f.QuestionId == id);

            if(vote_data != null)
            {
                if (vote_data.Vote == bool.Parse(vote))
                {
                    _context.QuestionVotes.Remove(vote_data);
                }
                else
                {
                    vote_data.Vote = bool.Parse(vote);
                    _context.QuestionVotes.Update(vote_data);
                }
            }
            else
            {
                _context.QuestionVotes.Add(new QuestionVote()
                {
                    QuestionId = id,
                    UserId = UserId,
                    Vote = bool.Parse(vote)
                });
            }
            _context.SaveChanges();

            int Voute_plus = _context.QuestionVotes.Count(f=>f.QuestionId == id && f.Vote == true);
            int Voute_Negative = _context.QuestionVotes.Count(f => f.QuestionId == id && f.Vote == false);

            int vote_reult = Voute_plus - Voute_Negative;
 
            return vote_reult;
        }

        #endregion

        
    }
}
