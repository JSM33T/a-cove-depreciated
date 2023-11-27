import { acToast, acInit, classesToTags, acPostData, acFormHandler, acGetData } from '../../global.js'
declare const bootstrap: any;


const gl_slug = document.getElementById("ip_slug") as HTMLInputElement;
const gl_tag = document.getElementById("ip_tags") as HTMLInputElement;

const likeStat = document.getElementById("likesStat") as HTMLElement;
const commentStat = document.getElementById("commentStat") as HTMLElement;
const likeIcon = document.getElementById("likeIcon") as HTMLElement;
const likeBtn = document.getElementById("likeBtn") as HTMLButtonElement;

const blogRole =  document.getElementById("ip_blogrole") as HTMLInputElement;
const blogCount = document.getElementById("blogCount") as HTMLElement;
const commentsHolder = document.getElementById("commentsHolder") as HTMLElement;

let delModal = new bootstrap.Modal(document.getElementById('mdlDelete'));
const delbtn = document.getElementById("delConfirm") as HTMLButtonElement;




//comment props
//reply props
//delete props

const global_slug = gl_slug.value;
const global_tags = gl_tag.value;


acInit([
    loadTags,
    loadAuthors,
    loadLikes,
    isLiked,
    loadComments,
    () =>  likeBtn.addEventListener('click', addLike),
    () => classesToTags('img', 'rounded-3'),
    () => classesToTags('p', 'fs-lg'),
    () => acFormHandler('comment-form', addComment),
    () => delbtn.addEventListener('click', delConfirm) 
]);
 


async function applyEvents()
{
    let buttons = document.querySelectorAll('.deletecontent') as NodeListOf<HTMLButtonElement>;

    buttons.forEach(function (button) {
        button.addEventListener('click', function () {
            // Get the data-id attribute value
            let dataId = button.dataset.contentid;
            let dataType = button.dataset.type;
            delMdl(dataType, dataId);
        });
    });

    //buttons.forEach(function (button) {
    //    button.addEventListener('click', function () {
    //        delMdl();
    //    });
    //});
    //console.log("delete events attached");
}



//check if the article is likes or not and append <i> tag's class
async function isLiked() {

   //check if its likes by the logged in user
   const data = {
       slug: global_slug
   };

const isliked = await acPostData('/api/blog/likestat',data)
    const isBlogLiked = isliked.data;
    if(isliked.type == "ok")
    {
        if (isBlogLiked == true) {
            likeIcon.classList.remove("ai-heart");
            likeIcon.classList.add("ai-heart-filled");
        }
        else {
            likeIcon.classList.remove("ai-heart-filled");
            likeIcon.classList.add("ai-heart");
        }
    }
    else{
        console.log("notliked + error");
    }
          
}

//load no of liekes
async function loadLikes() {
    //get no of likes

    const likes = await acGetData('/api/blog/' + global_slug + '/likes');
    console.log(likes);

    if(likes.type == "ok"){
        likeStat.innerHTML = likes.data;
    }
    else{
        likeStat.innerHTML = "0";
    }

           
 
}

//add/remove like
async function addLike() {
    const likedata = {
        Slug: global_slug,
    }
    const resp = await acPostData('/api/blog/addlike', likedata);
    console.log(resp);

    loadLikes();
    isLiked();
}

//split and arrange authors from blog-model's author property
async function loadAuthors() {
    const apiUrl = '/api/blog/' + global_slug + '/authors';

    try {
        const response = await acGetData(apiUrl);

        if (response.type === 'ok') {
            const data = response.data;
            let authorsdat = "";

            if (data.length !== 0) {
                for (let i = 0; i < data.length; i++) {
                    authorsdat += '<a href="/author/' + data[i].userName + '" class="no-decor">' + data[i].firstName + ' ' + data[i].lastName + '</a>, ';
                }
                document.getElementById('authorsPlaceholder')!.innerHTML = authorsdat.slice(0, authorsdat.lastIndexOf(',')) + authorsdat.slice(authorsdat.lastIndexOf(',') + 1);
            }
        } else {
            console.error('Error:', response.data);
            document.getElementById('authorsPlaceholder')!.innerHTML = "--";
        }
    } catch (error) {
        console.error('Error:', error);
        document.getElementById('authorsPlaceholder')!.innerHTML = "--";
    }
}

//add comment and refresh comment span
async function addComment() {
   const blgcmnt = document.getElementById('blog_comment') as HTMLInputElement;
   if (blgcmnt.value.length < 2) {
       acToast("error", "comment too short");
   }
   else {
       const cmntBtn = document.getElementById('commentbutton') as HTMLButtonElement;
       cmntBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>posting.. ';
       cmntBtn.classList.add('pe-none');

    const  commentData = {
        Slug: global_slug,
        Comment: blgcmnt.value,
    };
       const resp = await acPostData('/api/blog/comment/add', commentData);
       try {
           acToast(resp.type, resp.data);
           cmntBtn.innerHTML = 'Post Comment';
           loadComments();
           cmntBtn.classList.remove('pe-none');
           blgcmnt.value = "";
        
       } catch (error: any) {
           console.log("catch block"+ error);
       }
       finally {
        cmntBtn.innerHTML = 'Post Comment';
        cmntBtn.classList.remove('pe-none');
       }
   }
}

//load comment span

async function loadComments() {
        const roleData = blogRole.value;
        const data = {
            Slug: global_slug
        };

    const response = await acPostData('/api/blog/comments/load',data)
    console.log(response);
              //  response.data.sort((a, b) => a.id - b.id);
                let commentsHTML = "";
                let c = 0;
                if (response.data.length == 0) {
                    commentsHTML = "";
                }
                else {
                    for (let i = 0; i < response.data.length; i++) {
                        const comment = response.data[i];
                        c = i + 1;
                        commentsHTML += '<div class="border-bottom py-4 mt-2 mb-4">' +
                            '<div class="d-flex align-items-center pb-1 mb-3"> <img class="rounded-circle" src="/assets/images/avatars/default/' + comment.avatar + '.png" width="48" alt="Comment author">' +
                            '<div class="ps-3">' +
                            '<h6 class="mb-0">' + comment.fullname + ' </h6><span class="fs-sm text-muted">' + comment.date + '</span>' +
                            '</div>' +
                            '</div>' +
                            '<div class="btn-group-sm me-2 ml-2 mx-2" style="float:right;" role="group" aria-label="Settings group">' +
                            (roleData !== 'guest' ? '<button type="button" id="repl_' + comment.id + '" onclick="cmntReply(' + comment.id + ')" class="btn btn-secondary btn-icon px-2"><i class="ai-redo"></i></button>&nbsp;' : '') +
                            (comment.edit ? '<button type="button" id="edt_' + comment.id + '" onclick="cmntEdit(' + comment.id + ')" class="btn btn-secondary btn-icon px-2"><i class="ai-edit"></i></button>&nbsp;' : '') +
                            (comment.edit ? '<button type="button" data-type="comment" data-contentid="' + comment.id + '" class="deletecontent btn btn-secondary btn-icon px-2"><i class="ai-trash"></i></button>&nbsp;' : '') +
                            '</div>' +
                            '<span class="pb-2 mb-0" id="cm_' + comment.id + '">' + comment.comment + '</span>';

                        if (comment.replies && comment.replies.length > 0) {
                            comment.replies.sort((a, b) => a.replyId - b.replyId);
                            for (let j = 0; j < comment.replies.length; j++) {
                                const reply = comment.replies[j];
                                commentsHTML +=
                                    '<div class="card card-body border-0 bg-secondary mt-4">' +
                                    '    <div class="d-flex align-items-center pb-1 mb-3">' +
                                    '        <img class="rounded-circle" src="/assets/images/avatars/default/' + reply.replyAvatar + '.png" width="48" alt="Comment author">' +
                                    '        <div class="ps-3">' +
                                    '            <h6 class="mb-0">' + reply.replyFullName + '</h6><span class="fs-sm text-muted">' + reply.replyDate + '</span>' +
                                    '        </div>' +
                                    '    </div>' +
                                    '    <div class="d-flex align-items-center justify-content-between mb-3" role="group" aria-label="Settings group">' +
                                    '        <p class="mb-0"><a class="fw-bold text-decoration-none" href="#">@@' + comment.username + '</a>&nbsp;&nbsp;<span id="reply_' + reply.replyId + '">' + reply.replyComment + '</span></p>' +
                                    '        <div>' +
                                (reply.replyEdit ? '<button type="button" id="edt' + reply.replyId + '" onclick="replyEdit(' + reply.replyId + ')" class="btn btn-sm btn-secondary btn-icon px-2"><i class="ai-edit"></i></button>&nbsp;' : '') +
                                (reply.replyEdit ? '<button type="button" data-type="reply" data-contentid="' + reply.replyId + '" class="deletecontent btn btn-sm btn-secondary btn-icon px-2"><i class="ai-trash"></i></button>&nbsp;' : '') +
                                    '        </div>' +
                                    '    </div>' +
                                    '</div>';

                            }
                        }
                        commentsHTML += `</div>`;
                    }
                }
                commentsHolder.innerHTML = commentsHTML;
    blogCount.innerHTML = c.toString();
    applyEvents();

    }


//load tags related to slug
function loadTags() {
    var input = global_tags;
    var parts = input.split(',');
    var tags = "";
    parts.forEach(function (part) {
        var words = part.split(' ');
        for (var i = 0; i < words.length; i++) {
            tags = tags + `<a class="btn btn-outline-secondary btn-sm rounded-pill mt-2 ms-2" href="/blogs/browse/tag/${words[i]}">${words[i]}</a>`;

        }
    });
    document.getElementById('tagsPlaceholder')!.innerHTML = tags;
}

///////////////// ls based system ///////////////////

function delMdl(contenttype,contentid)
{
    let c_type = document.getElementById("mdlContentType") as HTMLElement;
    if (contenttype == "comment")
    {       
        c_type.innerHTML = "comment";
        delModal!.show();
    }
    else if (contenttype == "reply")
    {
        c_type.innerHTML = "reply";
        delModal!.show();
    }

    localStorage.setItem('content-type', contenttype);
    localStorage.setItem('content-id', contentid);
    localStorage.setItem('action', "delete");
}

async function delConfirm()
{
    console.log("reached");
    if(localStorage.getItem('action') == "delete")
    {
        let did = localStorage.getItem('content-type');
        const cid = localStorage.getItem('content-id');
        if (did == "comment") {
            const resp = await acPostData('/api/blog/comment/delete', {
                id: cid
            })
            delModal.hide();
            acToast(resp.type, resp.data)
        }
        else if (did == "reply") {
            const resp = await acPostData('/api/blog/reply/delete', {
                replyid: cid
            })
            delModal.hide();
            acToast(resp.type, resp.data)
        }
    }
    loadComments();
    localStorage.removeItem('action');
}

//function replyEdit(id) {
//    localStorage.removeItem("replyid");
//    localStorage.setItem('replyid', id);
//    const v = localStorage.getItem('replyid');
//    //$('#mdlReply').modal('show');
//    mdlOpen('mdlReply');
//    var replid = "reply_" + id;
//    var p = document.getElementById(replid)!.innerHTML;
//    replyEdt.value = p;
//    if (p == "" || p == null) {
//        replyEdt.value = p;
//        alert("entity");
//    }
//}

//function cmntEditSave() {
//    var editId = localStorage.getItem('editid');
//    var comment = commentEdt.value;
//    axios.post('/api/blog/comment/edit', {
//        id: editId,
//        comment: comment
//    })
//        .then(function (response) {
//            console.log(response.data);
//            loadComments();
//            document.querySelector("#blog_commentEdit").value = "";
//            //$('#mdlEdit').modal('hide');
//            mdlClose('mdlEdit');
//            localStorage.removeItem("editid");
//            toaster("success", "changes saved");
//        })
//        .catch(function (error) {
//            console.error(error);
//            toaster("error", "something went wrong");
//        });
//}


//function replyEditSave() {
//    let replyId = localStorage.getItem('replyid');
//    let editedReply = replyEdt.value;

//    axios({
//        method: 'POST',
//        url: '/api/blog/reply/edit',
//        headers: {
//            'Content-Type': 'application/json',
//        },
//        data: {
//            replyId: replyId,
//            reply: editedReply,
//        },
//    })
//        .then(function (response) {
//            loadComments();
//            mdlClose('mdlReply');
//            document.getElementById('blog_replyEdit').value = '';
//            toaster('Success', 'Reply edited successfully');
//            localStorage.removeItem('replyid');
//        })
//        .catch(function (error) {
//            console.log(error);
//            toaster('error', 'Something went wrong');
//        });

//}

//function cmntDelete(id) {
//    localStorage.setItem('delid', id);
//    const v = localStorage.getItem('delid');
//    //$('#mdlDelete').modal('show');
//    mdlOpen('mdlDelete');
//}

//function cmntDeleteConfirm() {
//    axios.post('/api/blog/comment/delete', {
//        id: localStorage.getItem('delid')
//    })
//        .then(function (response) {
//            console.log(response.data);
//            loadComments();
//            //$('#mdlEdit').modal('hide');
//            //$('#mdlDelete').modal('hide');
//            mdlClose('mdlEdit');
//            mdlClose('mdlDelete');

//            localStorage.removeItem('delid');
//            toaster("success", "comment deleted");
//        })
//        .catch(function (error) {
//            console.error(error);
//            //$('#mdlDelete').modal('hide');
//            mdlClose('mdlDelete');
//            localStorage.removeItem('delid');
//            toaster("error", "something went wrong");
//        })
//}


//function cmntReply(id) {
//    localStorage.setItem('replyto', id);
//    const v = localStorage.getItem('replyto');
//    document.getElementById("addReplybox").value = "";
//    //$('#mdladdReply').modal('show');
//    mdlOpen('mdladdReply');
//}




//function replAdd() {
//    axios({
//        method: "POST",
//        url: "/api/blog/reply/add",
//        headers: {
//            "Content-Type": "application/json",
//        },
//        data: {
//            Slug: global_slug,
//            ReplyText: document.getElementById("addReplybox").value,
//            CommentId: localStorage.getItem("replyto"),
//        },
//    })
//        .then(function (response) {
//            loadComments();
//            mdlClose("mdladdReply");
//            toaster("Success", "Reply added");
//        })
//        .catch(function (error) {
//            console.error("Error:", error);
//            toaster("Error", error.response.data);
//        });
//}

//function replyDelete(id) {
//    localStorage.setItem('delid', id);
//    const v = localStorage.getItem('delid');
//    mdlOpen("mdlreplyDelete");
//}
