import { User } from '../../Interfaces/user.interface.js';
import { acToast, acPostData } from '../../global.js'
declare const axios: any;
const chk = document.getElementById('rememberMe') as HTMLInputElement;
const userName = document.getElementById('userName') as HTMLInputElement;
const pass = document.getElementById('password') as HTMLInputElement;
const submitBtn = document.getElementById('submitBtn') as HTMLButtonElement;

    document.addEventListener('DOMContentLoaded', function () {
        const form = document.querySelector('.login-form') as HTMLFormElement;
        form.addEventListener('submit', async function (event) {
            event.preventDefault();
            await subMitLoginForm();
            console.log(chk.value + " & " + userName.value + " & " + pass.value);
        });
    });

//    async function subMitLoginForm()
//    {
//        if (userName.value.length <= 2) {
//            await showToast("error", "username too short");
//        }
//        else if (pass.value.length == 0) {
//            await showToast("error", "please enter the password");
//        }
//        else {
//                const apiUrl = '/api/account/login';
//                const data: User = {
//                    username: userName.value,
//                    password: pass.value

//                };

//             await postData((apiUrl, data)
//                 .then((response) => {
//                     console.log(response.statuscode);
//                     if (response.ok == true) {
//                         showToast('success', 'Handling successful response');
//                         window.location.href = "/";
//                     }
//                     else {
//                         console.log("something went wrong");
//                     }
//                })
//                .catch((error) => {
//                    console.error('Handling error:', error.response.data);
//                });
//        }
//}


const subMitLoginForm = async () => {
    const dataToSend = {
        username: userName.value,
        password: pass.value,
    };

    const result = await acPostData("/api/login", dataToSend);
    console.log(result);
};

