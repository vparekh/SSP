using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;
using System.Data;

namespace SSPWebUI.Service
{
    public class CommentsController : ApiController
    {


        public List<Comment> GetComment(decimal ProtocolVersion, string DraftVersion)
        {

            try
            {
                List<Comment> comments = new List<Comment>();

                Comment comment = new Comment();
                DataTable dt = comment.GetComments(ProtocolVersion, DraftVersion);
                foreach (DataRow dr in dt.Rows)
                {
                    int commentid = int.Parse(dr["Id"].ToString());
                    decimal userckey = decimal.Parse(dr["UserCKey"].ToString());
                    string usercomment = dr["Comment"].ToString();
                    string section = dr["ReviewItem"].ToString();
                    DateTime dtAdded = DateTime.Parse(dr["DateAdded"].ToString());
                    string userid = SSPUser.GetUserByCKey(userckey).UserID;
                    string reviewitemckey = dr["ReviewItemCKey"].ToString();

                    comment = new Comment { Id = commentid, UserCKey = userckey, UserId = userid, DateAdded = dtAdded.ToShortDateString() + " " + dtAdded.ToShortTimeString(), ReviewItem = section, ReviewItemCKey = reviewitemckey, ProtocolVersionCKey = ProtocolVersion, UserComment = usercomment };
                    comments.Add(comment);
                }
                return comments;

            }
            catch(Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                {

                    ReasonPhrase = ex.Message

                };

                throw new HttpResponseException(resp);
            }
            
            
        }

       
        public List<Comment> GetComments(string protocolckey, string draftversion, string reviewitem, string reviewitemckey, string reviewerckey)
        {
            try
            {
                List<Comment> comments = new List<Comment>();

                Comment comment = new Comment();
                DataTable dt = new DataTable();
                if (reviewerckey == null)
                {
                    if (reviewitem != null)
                    {
                        dt = comment.GetComments(decimal.Parse(protocolckey), reviewitem);
                    }
                    else
                    {
                        dt = comment.GetComments(decimal.Parse(protocolckey), draftversion);
                    }
                }
                else
                {
                    if (reviewitemckey != null)
                    {
                        dt = comment.GetCommentsByReviewer(decimal.Parse(protocolckey), draftversion, reviewitem, reviewitemckey, reviewerckey);
                    }
                    else if (reviewitem != null)
                    {
                        dt = comment.GetCommentsByReviewer(decimal.Parse(protocolckey), draftversion, reviewitem, reviewerckey);
                    }
                    else
                    {
                        dt = comment.GetCommentsByReviewer(decimal.Parse(protocolckey), draftversion, decimal.Parse(reviewerckey));
                    }
                }


                foreach (DataRow dr in dt.Rows)
                {
                    int commentid = int.Parse(dr["Id"].ToString());
                    decimal userckey = decimal.Parse(dr["UserCKey"].ToString());
                    string usercomment = dr["Comment"].ToString();
                    string reviewitm = dr["ReviewItem"].ToString();
                    DateTime dtAdded = DateTime.Parse(dr["DateAdded"].ToString());
                    string userid = SSPUser.GetUserByCKey(userckey).UserID;
                    string reviewitmckey = dr["ReviewItemCKey"].ToString();

                    comment = new Comment { Id = commentid, UserCKey = userckey, UserId = userid, DateAdded = dtAdded.ToShortDateString() + " " + dtAdded.ToShortTimeString(), ReviewItem = reviewitm, ReviewItemCKey = reviewitmckey, ProtocolVersionCKey = decimal.Parse(protocolckey), UserComment = usercomment };
                    comments.Add(comment);
                }
                return comments;

            }
            catch(Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                {

                    ReasonPhrase = ex.Message

                };

                throw new HttpResponseException(resp);
            }
            
            
        }

        // POST api/<controller>
        [HttpPost]
        public string AddComment(Comment value)
        {

            try
            {
                value.DateAdded = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

                value.Add();

                return "success";
            }
            catch(Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                {

                    ReasonPhrase = ex.Message

                };

                throw new HttpResponseException(resp);
            }
           

            
        }

    }
}