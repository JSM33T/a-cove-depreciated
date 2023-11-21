import { Email } from '../Interfaces/email.interface.js';
import { acInit, acGetData, acPostData, acToast, validateEmail } from '../global.js';


const mailForm = document.querySelector('.subscription-form') as HTMLFormElement;

acInit([
    loadTopBlogs,
    formSubmitEvent
])


async function formSubmitEvent() {
    

    mailForm.addEventListener('submit', async function (event) {
        event.preventDefault();

        const emailInput = document.getElementById('subscr-email') as HTMLInputElement;
        const emailData = {
            email: emailInput.value,
            origin: "HomePage",
        };

      await  postEmailToAPI(emailData);

    });
}

async function postEmailToAPI(emailData: Email) {

    if (!validateEmail(emailData.email)) {
        acToast("error", "Invalid email")
        return;
    }
    const submitBtn = document.getElementById('submitMail') as HTMLButtonElement;
    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>&nbsp;loading...';
    const apiUrl = '/api/mailinglist/subscribe';
    const resp = await acPostData(apiUrl, emailData);
    try {
        acToast(resp.type, resp.data);
     mailForm.reset();
    } catch (error: any) {
        acToast(resp.type, error.data);
    }
    finally {
        submitBtn.textContent = "Subscribe";
    }
}




async function loadTopBlogs() {
    const resp = await acGetData("/api/topblogs/get");
    try {
        await constructArticles(resp.data);
    } catch (error: any) {
        console.log(error.data);
    }
}


//async function loadTopBlogs() {
//    acGetData("/api/topblogs/get")
//        .then(async (data) => {
//            console.log('Data received:', data);
//            await constructArticles(data);
//        })
//        .catch((error) => {
//            console.error('Error:', error.data);
//        });


//}

async function constructArticles(responsedata:any)
{
    var articleTags = responsedata.map(post => `
        <article class="col">
            <div class="pb-4 pt-2 pt-xl-3 ms-md-3 border-bottom">
                <h3 class="h4">
                    <a href="/blog/${post.datePosted.substring(0,4)}/${post.urlHandle}">${post.title}</a>
                </h3>
                
                <p class="mb-4">${post.description}</p>
                <div class="d-flex align-items-center">
                    <span class="fs-sm text-body-secondary">${post.dateFormatted}</span>
                    <span class="fs-xs opacity-20 mx-3">|</span>
                    <a class="badge text-nav fs-xs border" href="#">${post.category}</a>
                </div>
            </div>
        </article>
    `).join('');


    const artcl = document.getElementById('articlePlaceholder');
    if (artcl) {
        artcl.innerHTML = articleTags;
    }
}