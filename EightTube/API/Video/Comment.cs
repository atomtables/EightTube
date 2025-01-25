using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EightTube.API
{
    
    [DataContract]
    public class CommentsData: Invidious
    {
        [DataMember(IsRequired = false)]
        public int commentCount { get; set; }
        [DataMember]
        public string videoId { get; set; }
        [DataMember]
        public List<Comment> comments { get; set; }
        [DataMember]
        public string continuation { get; set; }
    }

    [DataContract]
    public class Comment
    {
        [DataMember]
        public string commentId { get; set; }

        [DataMember]
        public string author { get; set; }
        [DataMember]
        public List<Image> authorThumbnails { get; set; }
        [IgnoreDataMember]
        public Image authorThumbnail { get { return authorThumbnails.Last(); } }
        [DataMember]
        public string authorId { get; set; }
        [DataMember]
        string authorUrl { get; set; }
        [IgnoreDataMember]
        public Uri authorURL { get { return new Uri(authorUrl); } }

        [DataMember]
        public bool isEdited { get; set; }
        [DataMember]
        public bool isPinned { get; set; }
        [DataMember(IsRequired = false)]
        public bool? isSponsor { get; set; }
        [IgnoreDataMember]
        public bool isParent { get; set; }
        [IgnoreDataMember]
        public int lineLimit { get { return isParent ? int.MaxValue : 4; } }
        [DataMember(IsRequired = false)]
        public string sponsorIconUrl { get; set; }

        [IgnoreDataMember]
        public string modifiers
        {
            get
            {
                string result = "";
                if (isEdited) result += " • Edited";
                if (isPinned) result += " • Pinned";
                if (isSponsor != null && (bool)isSponsor) result += " • Member";
                return result;
            }
        }

        [DataMember]
        public string content { get; set; }
        [DataMember]
        public string contentHtml { get; set; }
        [DataMember]
        public long? published { get; set; }
        [IgnoreDataMember]
        public string publishedFormattedDate { get { return Invidious.FormatDate(Invidious.ConvertEpochToDateTime(published ?? 0)); } }
        [DataMember]
        public string publishedText { get; set; }
        [DataMember]
        public int likeCount { get; set; }
        [IgnoreDataMember]
        public string likeCountFormatted { get { return Invidious.FormatNumber(likeCount); } }
        
        [DataMember]
        public bool authorIsChannelOwner { get; set; }
        [DataMember]
        public CreatorHeart creatorHeart { get; set; }
        [DataMember]
        public CommentRepliesInfo replies { get; set; }

        [IgnoreDataMember]
        public int replyCount { get { return replies?.replyCount ?? 0; } }

        public static List<Comment> Sort(List<Comment> comments)
        {
            for (int i = 0; i < comments.Count; i++)
            {
                if (comments[i].isPinned)
                {
                    Comment comment = comments[i];
                    comments.Remove(comments[i]);
                    comments.Insert(0, comment);
                    i = 1;
                }
            }
            return comments;
        }
    }

    [DataContract]
    public class CreatorHeart
    {
        [DataMember]
        public string creatorThumbnail { get; set; }
        [DataMember]
        public string creatorName { get; set; }
    }

    [DataContract]
    public class CommentRepliesInfo
    {
        [DataMember]
        public int? replyCount { get; set; }
        [DataMember]
        public string continuation { get; set; }
    }
}
