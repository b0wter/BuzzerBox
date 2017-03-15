using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuzzerBox.Data;
using BuzzerBox.Helpers;
using BuzzerBox.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;
using BuzzerEntities.Models;

namespace BuzzerBox.Controllers
{
    [Route("api/[controller]")]
    public class QuestionsController : BaseController
    {
        public QuestionsController(BuzzerContext context) : base(context)
        {
            //
        }

        // GET: api/values
        [HttpGet]
        public JsonResult Get([RequiredFromQuery] string sessionToken, [FromQuery] long timeStamp = 0)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                return new JsonResult(context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).Where(q => q.Timestamp >= timeStamp).AsNoTracking().ToList());
            }
            catch(ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch(Exception ex)
            {
                return ex.ToJsonResult();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get([RequiredFromQuery] string sessionToken, int id)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                var question = context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).AsNoTracking().First(q => q.Id == id);
                return new JsonResult(question);
            }
            catch(ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch(Exception ex)
            {
                return ex.ToJsonResult();
            }
        }

        [HttpPost("{questionId}/vote/{responseId}")]
        public JsonResult PostVote([RequiredFromQuery] string sessionToken, int questionId, int responseId)
        {
            try
            {
                var token = ValidateSessionToken(sessionToken);

                if (token.User.Level == UserLevels.Guest)
                    throw new PermissionDeniedException();

                if (!context.Questions.Any(q => q.Id == questionId))
                    throw new InvalidEntityException(questionId, "question");

                if(!context.Responses.Any(r => r.Id == responseId))
                    throw new InvalidEntityException(responseId, "response");

                var question = context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).ThenInclude(v => v.User).First(q => q.Id == questionId);
                if (question.IsActive == false)
                    throw new QuestionClosedException();

                
                ClearOldVotes(question, token.User, responseId);
                

                var vote = new Vote
                {
                    ResponseId = responseId,
                    Timestamp = DateTime.Now.ToUtcUnixTimestamp(),
                    UserId = token.UserId,
                };

                // if a new vote is cast we have to delete the old one
                context.Votes.ToList().RemoveAll(v => v.ResponseId == vote.ResponseId && v.UserId == vote.UserId);

                var result = context.Votes.Add(vote).Entity;
                context.SaveChanges();

                return new JsonResult(result);
            }
            catch (ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch (Exception ex)
            {
                return ex.ToJsonResult();
            }
        }

        private void ClearOldVotes(Question q, User u, int responseId)
        {
            context.Entry(u). .Collection(u => u.Votes).Load();
            
            /*
            Es müssen noch alte Stimmen gelöscht werden, nach folgenden Bedingungen:
                - Wenn die Frage multiple choice erlaubt muss nur sichergestellt werden das es nicht bereits eine Stimme mit der gleichen Response Id gibt.
                - Wenn die Frage kein multiple choice erlaubt müssen alle alten Stimmen gelöscht werden.
            */
            List<Vote> oldVotes = new List<Vote>();
            if(q.AllowMultipleVotes)
            {
                oldVotes.AddRange(q.Responses.SelectMany(r => r.Votes).Where(v => v.ResponseId == responseId && v.UserId == u.Id));
            }
            else
            {
                oldVotes.AddRange(q.Responses.SelectMany(r => r.Votes).Where(v => v.UserId == u.Id));
            }
            oldVotes.ForEach(v => context.Votes.Remove(v));
            context.SaveChanges();
        }

        [HttpPost("{questionId}/close")]
        public JsonResult CloseQuestion([RequiredFromQuery] string sessionToken, int questionId)
        {
            try
            {
                var token = ValidateSessionToken(sessionToken);
                var question = context.Questions.First(q => q.Id == questionId);

                if (token.User.Level == UserLevels.Admin || token.UserId == question.UserId)
                {
                    question.IsActive = false;
                    context.SaveChanges();
                    return new JsonResult(question);
                }
                else
                {
                    throw new PermissionDeniedException($"You cannot close this question since it wasn't posted by you and you are not an admin.");
                }
            }
            catch (ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch (Exception ex)
            {
                return ex.ToJsonResult();
            }
        }
    }
}
