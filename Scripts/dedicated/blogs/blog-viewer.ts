import { acToast, acInit ,classesToTags, acPostData, acFormHandler, acGetData} from '../../global.js'

const gl_slug = document.getElementById("ip_slug") as HTMLInputElement;
const gl_tag = document.getElementById("ip_tags") as HTMLInputElement;

const likeStat = document.getElementById("likesStat") as HTMLElement;
const commentStat = document.getElementById("commentStat") as HTMLElement;
const likeIcon = document.getElementById("likeIcon") as HTMLElement;
const likeBtn = document.getElementById("likeBtn") as HTMLButtonElement;

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
    () =>  likeBtn.addEventListener('click', addLike),
    () => classesToTags('img', 'rounded-3'),
    () => acFormHandler('comment-form', addComment)
]);
 

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





    //    axios({
    //        method: 'POST',
    //        url: '/api/blog/comment/add',
    //        headers: {
    //            'Content-Type': 'application/json',
    //        },
    //        data: {
    //            Slug: global_slug,
    //            Comment: blgcmnt.value,
    //        },
    //    })
    //        .then(function (response) {
    //            cmntBtn.innerHTML = 'Post Comment';
    //            loadComments();
    //            cmntBtn.classList.remove('pe-none');
    //            acToast('success', 'Comment added');
    //            blgcmnt.value = "";
    //        })
    //        .catch(function (error) {
    //            console.error('Error:', error);
    //            cmntBtn.innerHTML = 'Post Comment';
    //            cmntBtn.classList.remove('pe-none');
    //            acToast('error', 'Something went wrong');
    //        });
   }
}

//load comment span
async function loadComments(){
    console.log("comment load function called")
    acToast('check','comment added ig check db');
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

