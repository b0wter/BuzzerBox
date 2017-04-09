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
        public JsonResult PostVote([RequiredFromQuery] string sessionToken, int questionId, int responseId, [FromQuery] bool includeFullQuestionAsResult = false)
        {
            try
            {
                var token = ValidateSessionToken(sessionToken);

                if (token.User.Level == UserLevels.Guest)
                    throw new PermissionDeniedException();

                if (!context.Questions.Any(q => q.Id == questionId))
                    throw new InvalidEntityException(questionId, "question");

                var question = context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).ThenInclude(v => v.User).First(q => q.Id == questionId);
                if (question.IsActive == false)
                    throw new QuestionClosedException();

                // check if the question contains a response with the responseId
                if(question.Responses.Any(r => r.Id == responseId) == false)
                    throw new InvalidEntityException(responseId, "response");


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
                if (includeFullQuestionAsResult)
                {
                    return new JsonResult(question);
                }
                else
                {
                    return new JsonResult(result);
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

        private FilteredUser FetchUserVotes(FilteredUser user)
        {
            var detailedUser = context.Users.Include(u => u.Votes).ThenInclude(v => v.Response).First(u => user.Id == u.Id);
            return detailedUser;
        }

        private void ClearOldVotes(Question question, User user, int responseId)
        {
            // explicit loading is only supported on .Net Core 1.1 so this needs to be done manually
            var detailedUser = FetchUserVotes(user);
            ClearOldVotesWithMatchingResponseId(detailedUser, responseId);
            ClearOldVoteIfNotMultipleChoice(question, detailedUser);
        }

        private void ClearOldVotesWithMatchingResponseId(FilteredUser user, int responseId)
        {
            var oldVotes = user.Votes.Where(v => v.ResponseId == responseId).ToList();
            oldVotes.ForEach(o => context.Votes.Remove(o));
        }

        private void ClearOldVoteIfNotMultipleChoice(Question question, FilteredUser user)
        {
            if (question.AllowMultipleVotes == false)
            {
                var oldVotes = user.Votes.Where(v => question.Responses.Any(r => r.Id == v.ResponseId)).ToList();
                oldVotes.ForEach(o => context.Votes.Remove(o));
            }
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
