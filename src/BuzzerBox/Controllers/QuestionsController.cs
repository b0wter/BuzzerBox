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
                return new JsonResult(Context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).Where(q => q.Timestamp >= timeStamp).AsNoTracking().ToList());
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
                var question = Context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).AsNoTracking().First(q => q.Id == id);
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

        [HttpGet("filter")]
        public JsonResult GetFilteredQuestions([RequiredFromQuery] string sessionToken, int id = int.MinValue, string title = null, bool? isActive = null, int roomId = int.MinValue, int sinceTimestamp = 0, int userId = int.MinValue)
        {
            try
            {
                ValidateSessionToken(sessionToken);

                var filtered = Context.Questions.AsNoTracking();

                if (id != int.MinValue)
                    filtered = filtered.Where(q => q.Id == id);

                if (title != null)
                    filtered = filtered.Where(q => q.Title.ToLower().Contains(title.ToLower()));

                if (isActive != null)
                    filtered = filtered.Where(q => q.IsActive == (bool)isActive);

                if (roomId != int.MinValue)
                    filtered = filtered.Where(q => q.RoomId == roomId);

                if (sinceTimestamp != 0)
                    filtered = filtered.Where(q => q.Timestamp >= sinceTimestamp);

                if (userId != int.MinValue)
                    filtered = filtered.Where(q => q.UserId == userId);

                return new JsonResult(filtered);
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
        public JsonResult PostVote([RequiredFromQuery] string sessionToken, int questionId, int responseId, [FromQuery] bool includeFullQuestionAsResult = false, [FromQuery] bool isDelete = false)
        {
            try
            {
                var token = ValidateSessionToken(sessionToken);

                if (token.User.Level == UserLevels.Guest)
                    throw new PermissionDeniedException();

                if (!Context.Questions.Any(q => q.Id == questionId))
                    throw new InvalidEntityException(questionId, "question");

                var question = Context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).ThenInclude(v => v.User).First(q => q.Id == questionId);
                if (question.IsActive == false)
                    throw new QuestionClosedException();

                // check if the question contains a response with the responseId
                if(question.Responses.Any(r => r.Id == responseId) == false)
                    throw new InvalidEntityException(responseId, "response");


                if (isDelete == false)
                {
                    ClearOldVotes(question, token.User, responseId);

                    var vote = new Vote
                    {
                        ResponseId = responseId,
                        Timestamp = DateTime.Now.ToUtcUnixTimestamp(),
                        UserId = token.UserId,
                    };

                    // if a new vote is cast we have to delete the old one
                    Context.Votes.ToList().RemoveAll(v => v.ResponseId == vote.ResponseId && v.UserId == vote.UserId);

                    var result = Context.Votes.Add(vote).Entity;
                    Context.SaveChanges();
                    if (includeFullQuestionAsResult)
                    {
                        return new JsonResult(question);
                    }
                    else
                    {
                        return new JsonResult(result);
                    }
                }
                else
                {
                    // Remove any vote that corresponds to the userId and responseId.
                    var votesToRemove = Context.Votes.Where(v => v.UserId == token.UserId && v.ResponseId == responseId);
                    votesToRemove.ToList().ForEach(v => Context.Votes.Remove(v));
                    Context.SaveChanges();
                    question.Responses.ForEach(r => votesToRemove.ToList().ForEach(v => r.Votes.Remove(v)));
                    return new JsonResult(question);
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
            var detailedUser = Context.Users.Include(u => u.Votes).ThenInclude(v => v.Response).First(u => user.Id == u.Id);
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
            oldVotes.ForEach(o => Context.Votes.Remove(o));
        }

        private void ClearOldVoteIfNotMultipleChoice(Question question, FilteredUser user)
        {
            if (question.AllowMultipleVotes == false)
            {
                var oldVotes = user.Votes.Where(v => question.Responses.Any(r => r.Id == v.ResponseId)).ToList();
                oldVotes.ForEach(o => Context.Votes.Remove(o));
            }
        }

        [HttpPost("{questionId}/close")]
        public JsonResult CloseQuestion([RequiredFromQuery] string sessionToken, int questionId)
        {
            try
            {
                var token = ValidateSessionToken(sessionToken);
                var question = Context.Questions.First(q => q.Id == questionId);

                if (token.User.Level == UserLevels.Admin || token.UserId == question.UserId)
                {
                    question.IsActive = false;
                    Context.SaveChanges();
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
